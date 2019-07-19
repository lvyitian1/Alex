using System;
using System.Collections.ObjectModel;
using Alex.GuiDebugger.Models;
using Alex.GuiDebugger.Services;
using Catel.Collections;

namespace Alex.GuiDebugger.ViewModels
{
	public class DesignElementTreeViewModel : ElementTreeViewModel
	{

		public DesignElementTreeViewModel() : base()
		{
		}

	}

	//public class DesignGuiDebugDataService : IGuiDebugDataService
	//{
	//	public GuiDebuggerData GuiDebuggerData { get; }

	//	public DesignGuiDebugDataService()
	//	{
	//		GuiDebuggerData = new GuiDebuggerData();
	//		GuiDebuggerData.Elements = new FastObservableCollection<GuiDebuggerElementInfo>()
	//		{
	//			new GuiDebuggerElementInfo()
	//			{
	//				Id          = Guid.NewGuid(),
	//				ElementType = "DemoGuiScreen",
	//				ChildElements = new ObservableCollection<GuiDebuggerElementInfo>()
	//				{
	//					new GuiDebuggerElementInfo()
	//					{
	//						Id          = Guid.NewGuid(),
	//						ElementType = "GuiStackContainer",
	//						ChildElements = new ObservableCollection<GuiDebuggerElementInfo>()
	//						{
	//							new GuiDebuggerElementInfo()
	//							{
	//								Id          = Guid.NewGuid(),
	//								ElementType = "GuiTextElement"
	//							},

	//							new GuiDebuggerElementInfo()
	//							{
	//								Id          = Guid.NewGuid(),
	//								ElementType = "GuiTextElement"
	//							},

	//							new GuiDebuggerElementInfo()
	//							{
	//								Id          = Guid.NewGuid(),
	//								ElementType = "GuiTextElement"
	//							}
	//						}
	//					}
	//				}
	//			}
	//		};
	//	}

	//	public void RefreshElements()
	//	{

	//	}

	//	public void RefreshProperties(GuiDebuggerElementInfo elementInfo)
	//	{

	//	}
	//}
}
