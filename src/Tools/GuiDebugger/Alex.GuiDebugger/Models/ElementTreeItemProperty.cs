using System;

namespace Alex.GuiDebugger.Models
{
    public class ElementTreeItemProperty
    {
        public Guid ElementId { get; }

        public string Name { get; }
        
        public string Type { get; }
        public string Category { get; }
        
        public object Value { get; set; }

        public ElementTreeItemProperty()
        {

        }

        public ElementTreeItemProperty(Guid elementId, string name, string type, string category, object value) : this()
        {
            ElementId = elementId;
            Name = name;
            Type = type;
            Category = category;
            Value = value;
        }

    }
}
