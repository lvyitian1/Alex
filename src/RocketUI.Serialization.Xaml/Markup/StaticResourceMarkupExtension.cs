using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Portable.Xaml;
using Portable.Xaml.Markup;

namespace RocketUI.Serialization.Xaml.Markup
{

	[MarkupExtensionReturnType(typeof(object))]
	public class StaticResourceExtension : MarkupExtension
	{
		[ConstructorArgument("resourceKey")]
		public string ResourceKey { get; set; }

		public StaticResourceExtension()
		{
		}

		public StaticResourceExtension(string resourceKey)
		{
			ResourceKey = resourceKey;
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			var schemaContextProvider = serviceProvider.GetService(typeof(IXamlSchemaContextProvider)) as IXamlSchemaContextProvider;
			if (schemaContextProvider == null)
				throw new InvalidOperationException("StaticResource requires a schema context");

			var schemaContext = schemaContextProvider.SchemaContext;

			var ambientProvider = serviceProvider.GetService(typeof(IAmbientProvider)) as IAmbientProvider;
			if (ambientProvider == null)
				throw new InvalidOperationException("StaticResource requires an ambient provider");
			var types = new[]
			{
				schemaContext.GetXamlType(typeof(PropertyStore))
			};
			var members = new[]
			{
				schemaContext.GetXamlType(typeof(VisualElement)).GetMember("Properties")
			};
			var values = ambientProvider.GetAllAmbientValues(null, true, types, members);
			foreach (var dictionary in values.Select(r => r.Value).OfType<PropertyStore>())
			{
				object val;
				if (dictionary.TryGetValue(ResourceKey, out val))
					return val;
			}
			return null;
		}
	}
}
