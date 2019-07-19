using System;

namespace RocketUI
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DebuggerVisibleAttribute : Attribute
    {

        public bool Visible { get; set; } = true;

        public string Category { get; set; } = string.Empty;

        public DebuggerVisibleAttribute()
        {

        }


    }
}
