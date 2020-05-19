using System;

namespace Alex.GuiDebugger.Common
{
	public class GuiElementInfo
	{
		public Guid Id { get; set; }
		
		public string ElementType { get; set; }
		public string ElementName { get; set; }
		
		public GuiElementInfo[] ChildElements { get; set; }

		public GuiElementInfo()
		{

		}

		public GuiElementInfo(Guid id, string elementType, string elementName) : this()
		{
			Id = id;
			ElementType = elementType;
			ElementName = elementName;
		}
	}
}
