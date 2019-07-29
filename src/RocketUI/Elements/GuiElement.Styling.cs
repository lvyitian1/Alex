using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using RocketUI.Annotations;

namespace RocketUI
{
    public partial class GuiElement : INotifyPropertyChanged
    {
        public event EventHandler StyleChanged;

        public Style Style { get; set; }

        private StyledPropertyValueCollection _styledProperties = null;
        private StyledPropertyValueCollection StyledProperties
        {
            get
            {
                if (_styledProperties == null)
                {
                    _styledProperties = StyledPropertyValueCollection.Create(GetType());
                }

                return _styledProperties;
            }
        }

        protected object GetValue(StyledProperty styledProperty)
        {
            if (_styledProperties.TryGetValue(styledProperty.Name, out StyledProperty property))
            {
                return _styledProperties.GetValue(styledProperty.Name);
            }

            return styledProperty.DefaultValue;
        }

        protected void SetValue(StyledProperty styledProperty, object value)
        {
            if (_styledProperties.TryGetValue(styledProperty.Name, out StyledProperty property))
            {
                var oldValue = _styledProperties.GetValue(styledProperty.Name);
                if (oldValue != value)
                {
                    _styledProperties.SetValue(styledProperty.Name, value);
                    styledProperty.PropertyChangedHandler?.Invoke(this, oldValue, value);
                    OnPropertyChanged(styledProperty.Name, oldValue, value);
                }
            }
        }

        public void InvalidateStyle()
        {

        }

        protected virtual void OnStyleChanged() { }

        private void ApplyStyle(Style style)
        {
            foreach (var setter in style.Setters)
            {
                // TODO
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            PropertyChanged?.Invoke(this, new StyledPropertyChangedEventArgs(propertyName, oldValue, newValue));
        }
    }
}
