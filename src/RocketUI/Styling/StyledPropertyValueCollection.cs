using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RocketUI
{
    public class StyledPropertyValueCollection : Dictionary<string, StyledProperty>
    {
        internal static Dictionary<Type, Dictionary<string, StyledProperty>> TypeProperties = new Dictionary<Type, Dictionary<string, StyledProperty>>();

        private Dictionary<string, object> _values = new Dictionary<string, object>();

        internal StyledPropertyValueCollection(IEnumerable<StyledProperty> properties)
        {
            foreach (var property in properties)
            {
                Add(property.Name, property);
                _values.Add(property.Name, property.DefaultValue);
            }
        }

        internal static void Register(StyledProperty property)
        {
            if (!TypeProperties.TryGetValue(property.OwnerType, out var ownerTypeProperties))
            {
                ownerTypeProperties = new Dictionary<string, StyledProperty>();
                TypeProperties.Add(property.OwnerType, ownerTypeProperties);
            }

            ownerTypeProperties.Add(property.Name, property);
        }

        public void SetValue(string propertyName, object value)
        {
            _values[propertyName] = value;
        }

        public object GetValue(string propertyName)
        {
            if(_values.TryGetValue(propertyName, out var value))
            {
                return value;
            }

            if (TryGetValue(propertyName, out StyledProperty property))
            {
                return property.DefaultValue;
            }

            return null;
        }

        public bool TryGetValue(string propertyName, out object value)
        {
            return _values.TryGetValue(propertyName, out value);
        }

        public static StyledPropertyValueCollection Create(Type getType)
        {
            var properties = GetPropertiesForTypeRecursive(getType);
            return new StyledPropertyValueCollection(properties.ToArray());
        }

        private static IEnumerable<StyledProperty> GetPropertiesForTypeRecursive(Type type)
        {
            while (type != null && TypeProperties.TryGetValue(type, out var props))
            {
                foreach (var prop in props.Values)
                {
                    yield return prop.Clone();
                }

                type = type.BaseType;
            }
        }
    }

    public delegate void StyledPropertyChangedHandler(IGuiElement element, object oldValue, object newValue);

    public class StyledProperty
    {
        public string Name { get; }

        public Type ValueType { get; }
        public Type OwnerType { get; }

        public object DefaultValue { get; }

        public StyledPropertyChangedHandler PropertyChangedHandler;

        //public object Value { get; set; }

        public StyledProperty(string name, Type valueType, Type ownerType, object defaultValue = default, StyledPropertyChangedHandler propertyChangedHandler = null)
        {
            Name = name;
            ValueType = valueType;
            OwnerType = ownerType;
            DefaultValue = defaultValue;
            PropertyChangedHandler = propertyChangedHandler;
        }

        public StyledProperty Clone()
        {
            return new StyledProperty(Name, ValueType, OwnerType, DefaultValue, PropertyChangedHandler);
        }



        public static StyledProperty Register(string                       name, Type valueType, Type ownerType,
                                              object                       defaultValue           = default,
                                              StyledPropertyChangedHandler propertyChangedHandler = null)
        {
            var styledProperty = new StyledProperty(name, valueType, ownerType, defaultValue, propertyChangedHandler);
            StyledPropertyValueCollection.Register(styledProperty);
            return styledProperty;
        }
    }
}
