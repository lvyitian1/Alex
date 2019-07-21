using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using Portable.Xaml;
using Portable.Xaml.Markup;
using Portable.Xaml.Schema;

namespace RocketUI.Serialization.Xaml
{
	internal class RocketXamlSchemaContext : XamlSchemaContext
	{

		public const string RocketNamespace = "http://schema.trudan.ninja/netfx/2019/xaml/ui";

		private readonly Dictionary<Type, XamlType> typeCache = new Dictionary<Type, XamlType>();
		
		public bool DesignMode { get; set; }

		private static readonly Assembly RocketAssembly = typeof(RocketUI.Platform).GetTypeInfo().Assembly;


		protected override XamlType GetXamlType(string xamlNamespace, string name, params XamlType[] typeArguments)
		{
			XamlType type = null;
			try
			{
				type = base.GetXamlType(xamlNamespace, name, typeArguments);
			}
			catch
			{
				if (!DesignMode || type != null)
					throw;
			}
			return type;
		}

		public override XamlType GetXamlType(Type type)
		{
			XamlType xamlType;
			if (typeCache.TryGetValue(type, out xamlType))
				return xamlType;

			var info = type.GetTypeInfo();

			if (
				info.IsSubclassOf(typeof(VisualElement))
				|| info.Assembly == RocketAssembly // struct
				|| (
					   // nullable struct
					   info.IsGenericType
					   && info.GetGenericTypeDefinition() == typeof(Nullable<>)
					   && Nullable.GetUnderlyingType(type).GetTypeInfo().Assembly == RocketAssembly
				   ))
			{
				xamlType = new RocketXamlType(type, this);
				typeCache.Add(type, xamlType);
				return xamlType;
			}

			return base.GetXamlType(type);
		}

		bool isInResourceMember;
		PropertyInfo resourceMember;

		internal bool IsResourceMember(PropertyInfo member)
		{
			if (member == null)
				return false;
			if (resourceMember == null)
			{
				if (isInResourceMember)
					return false;
				isInResourceMember = true;
				try
				{
					resourceMember = typeof(VisualElement).GetRuntimeProperty("Properties");
				}
				finally
				{
					isInResourceMember = false;
				}
			}

			return member.DeclaringType == resourceMember.DeclaringType
				   && member.Name == resourceMember.Name;
		}

		class PropertiesXamlMember : XamlMember
		{
			public PropertiesXamlMember(PropertyInfo propertyInfo, XamlSchemaContext context)
				: base(propertyInfo, context)
			{
			}

			protected override bool LookupIsAmbient() => true;
		}

		protected override XamlMember GetProperty(PropertyInfo propertyInfo)
		{
			if (IsResourceMember(propertyInfo))
			{
				return new PropertiesXamlMember(propertyInfo, this);
			}

			return base.GetProperty(propertyInfo);
		}

		protected override XamlMember GetEvent(EventInfo eventInfo)
		{
			if (DesignMode)
			{
				// in design mode, ignore wiring up events
				return new EmptyXamlMember(eventInfo, this);
			}

			return base.GetEvent(eventInfo);
		}
	}

	class EmptyXamlMember : XamlMember
	{
		public EmptyXamlMember(EventInfo eventInfo, XamlSchemaContext context)
			: base(eventInfo, context)
		{

		}

		class EmptyConverter : System.ComponentModel.TypeConverter
		{
			public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType) => true;

			public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, CultureInfo culture, object value) => null;
		}

		protected override XamlValueConverter<System.ComponentModel.TypeConverter> LookupTypeConverter()
		{
			return new XamlValueConverter<System.ComponentModel.TypeConverter>(typeof(EmptyConverter), Type);
		}
	}

	class RocketXamlType : XamlType
	{
		public RocketXamlType(Type underlyingType, XamlSchemaContext schemaContext)
			: base(underlyingType, schemaContext)
		{

		}


		T GetCustomAttribute<T>(bool inherit = true)
			where T : Attribute
		{
			return UnderlyingType.GetTypeInfo().GetCustomAttribute<T>(inherit);
		}

		XamlValueConverter<System.ComponentModel.TypeConverter> typeConverter;
		bool                                 gotTypeConverter;

		protected override XamlValueConverter<System.ComponentModel.TypeConverter> LookupTypeConverter()
		{
			if (gotTypeConverter)
				return typeConverter;

			gotTypeConverter = true;

			// convert from Eto.TypeConverter to Portable.Xaml.ComponentModel.TypeConverter
			var typeConverterAttrib = GetCustomAttribute<TypeConverterAttribute>();

			if (typeConverterAttrib == null
				&& UnderlyingType.GetTypeInfo().IsGenericType
				&& UnderlyingType.GetTypeInfo().GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				typeConverterAttrib = Nullable.GetUnderlyingType(UnderlyingType).GetTypeInfo().GetCustomAttribute<TypeConverterAttribute>();
			}

			//if (typeConverterAttrib != null)
			//{
			//	var converterType = Type.GetType(typeConverterAttrib.ConverterTypeName);
			//	if (converterType != null)
			//		typeConverter = new ValueConverter(converterType, this);
			//}
			if (typeof(MulticastDelegate).GetTypeInfo().IsAssignableFrom(UnderlyingType.GetTypeInfo()))
			{
				var context = SchemaContext as RocketXamlSchemaContext;
				if (context.DesignMode)
				{
					return null;
				}
			}

			if (typeConverter == null)
				typeConverter = base.LookupTypeConverter();
			return typeConverter;
		}

		protected override bool LookupIsAmbient()
		{
			if (this.UnderlyingType != null && UnderlyingType == typeof(PropertyStore))
				return true;
			return base.LookupIsAmbient();
		}

		bool       gotContentProperty;
		XamlMember contentProperty;

		protected override XamlMember LookupContentProperty()
		{
			if (gotContentProperty)
				return contentProperty;
			gotContentProperty = true;
			var contentAttribute = GetCustomAttribute<ContentPropertyAttribute>();
			if (contentAttribute == null || contentAttribute.Name == null)
				contentProperty = base.LookupContentProperty();
			else
				contentProperty = GetMember(contentAttribute.Name);
			return contentProperty;
		}

		XamlMember nameAliasedProperty;

		protected override XamlMember LookupAliasedProperty(XamlDirective directive)
		{
			if (directive == XamlLanguage.Name)
			{
				if (nameAliasedProperty != null)
					return nameAliasedProperty;

				var nameAttribute = GetCustomAttribute<RuntimeNamePropertyAttribute>();
				if (nameAttribute != null && nameAttribute.Name != null)
				{
					nameAliasedProperty = GetMember(nameAttribute.Name);
					return nameAliasedProperty;
				}

			}
			return base.LookupAliasedProperty(directive);
		}
	}
}
