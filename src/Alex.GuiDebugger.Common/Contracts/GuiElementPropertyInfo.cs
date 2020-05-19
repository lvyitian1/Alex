using System;
using System.Runtime.Serialization;

namespace Alex.GuiDebugger.Common
{

	public class GuiElementPropertyInfo
	{
		
		public virtual string Name { get; set; }

		public virtual string Type     { get; set; }
		public virtual string Category { get; set; }

		public virtual object Value { get; set; }

		public virtual string StringValue { get; set; }
	}
}
