using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RocketUI.Serialization.Xaml
{
	static class TypeHelper
	{
		public static Assembly GetAssembly(this Type type)
		{
			return type.GetTypeInfo().Assembly;
		}

		public static Type GetBaseType(this Type type)
		{
			return type.GetTypeInfo().BaseType;
		}

		public static bool IsEnum(this Type type)
		{
			return type.GetTypeInfo().IsEnum;
		}

		public static MethodInfo GetGetMethod(this PropertyInfo propertyInfo)
		{
			return propertyInfo.GetMethod;
		}

		public static MethodInfo GetSetMethod(this PropertyInfo propertyInfo)
		{
			return propertyInfo.SetMethod;
		}

		public static T GetCustomAttribute<T>(this Type type, bool inherit)
			where T : Attribute
		{
			return type.GetTypeInfo().GetCustomAttribute<T>(inherit);
		}

		public static MethodInfo GetAddMethod(this EventInfo eventInfo)
		{
			return eventInfo.AddMethod;
		}

		public static MethodInfo GetRemoveMethod(this EventInfo eventInfo)
		{
			return eventInfo.RemoveMethod;
		}
		/// <summary>
		/// Determines whether the specified object is an instance of the current Type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="o">The object to compare with the current type.</param>
		/// <returns>true if the current Type is in the inheritance hierarchy of the 
		/// object represented by o, or if the current Type is an interface that o 
		/// supports. false if neither of these conditions is the case, or if o is 
		/// null, or if the current Type is an open generic type (that is, 
		/// ContainsGenericParameters returns true).</returns>
		public static bool IsInstanceOfType(this Type type, object o)
		{
			return o != null && IsAssignableFrom(type, o.GetType());
		}

		public static bool IsAssignableFrom(this Type type, Type c)
		{
			return c != null && type.GetTypeInfo().IsAssignableFrom(c.GetTypeInfo());
		}

		public static ConstructorInfo GetConstructor(this Type type, params Type[] args)
		{
			return type.GetTypeInfo().DeclaredConstructors.FirstOrDefault(r => r.GetParameters().Select(p => p.ParameterType).SequenceEqual(args));
		}
	}
}
