using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Alex.GuiDebugger.Common;
using Catel.Collections;
using Catel.Data;
using Orc.Search;

namespace Alex.GuiDebugger.Models
{
    public class GuiDebuggerData : ChildAwareModelBase
    {

        public FastObservableCollection<GuiDebuggerElementInfo> Elements { get; set; }


        public GuiDebuggerData()
        {
            Elements = new FastObservableCollection<GuiDebuggerElementInfo>();
        }

    }

    public class GuiDebuggerElementInfo : ModelBase
    {

        [SearchableProperty]
        public Guid Id { get; set; }

        [SearchableProperty]
        public string ElementType { get; set; }

        [SearchableProperty]
        public string ElementName { get; set; }

        public FastObservableCollection<GuiDebuggerElementInfo> ChildElements { get; set; }
        
        public FastObservableCollection<GuiDebuggerElementPropertyInfo> Properties { get; set; } 
        public FastObservableCollection<GuiDebuggerElementPropertyCategoryGroup> PropertyGroups { get; set; } 

        public GuiDebuggerElementInfo()
        {
            ChildElements = new FastObservableCollection<GuiDebuggerElementInfo>();
            Properties = new FastObservableCollection<GuiDebuggerElementPropertyInfo>();
            PropertyGroups = new FastObservableCollection<GuiDebuggerElementPropertyCategoryGroup>();
        }
    }
    public class GuiDebuggerElementPropertyCategoryGroup : ModelBase
    {

        public virtual string Name { get; set; }

        public virtual FastObservableCollection<GuiDebuggerElementPropertyInfo> Properties { get; set; }

        public GuiDebuggerElementPropertyCategoryGroup()
        {

        }
    }

    public class GuiDebuggerElementPropertyInfo : ModelBase
    {

        public virtual string Name { get; set; }
        public virtual string Category { get; set; }

        public virtual Type ValueType { get; set; }

        public virtual object Value { get; set; }


        public GuiDebuggerElementPropertyInfo()
        {

        }
    } 
}
