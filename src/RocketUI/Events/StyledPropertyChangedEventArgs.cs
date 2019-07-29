using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RocketUI
{
    public class StyledPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        public object OldValue { get; }
        public object NewValue { get; }

        internal StyledPropertyChangedEventArgs(string propertyName, object oldValue, object newValue) : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

    }
}
