using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alex.GuiDebugger.Models;
using Alex.GuiDebugger.Services;
using Catel.Collections;
using Microsoft.Xna.Framework;
using Orc.SelectionManagement;
using RocketUI;

namespace Alex.GuiDebugger.ViewModels
{
	public class DesignElementInspectorViewModel : ElementInspectorViewModel
	{

		public virtual FastObservableCollection<GuiDebuggerElementPropertyInfo> Properties
		{
			get => ElementInfo.Properties;
			set => ElementInfo.Properties = value;
		}

		public virtual FastObservableCollection<GuiDebuggerElementPropertyCategoryGroup> PropertyGroups
		{
			get => ElementInfo.PropertyGroups;
			set => ElementInfo.PropertyGroups = value;
		}

		public DesignElementInspectorViewModel() : base()
		{

			ElementInfo = new GuiDebuggerElementInfo()
			{
				ElementType = "GuiStackContainer",
				PropertyGroups = new FastObservableCollection<GuiDebuggerElementPropertyCategoryGroup>()
				{
					new GuiDebuggerElementPropertyCategoryGroup()
					{
						Name = "Layout",
						Properties = new FastObservableCollection<GuiDebuggerElementPropertyInfo>()
						{
							new GuiDebuggerElementPropertyInfo()
							{
								Category  = "Layout",
								ValueType = typeof(EditableSize),
								Value     = new EditableSize(0, 0),
								Name      = "MinSize"
							},
							new GuiDebuggerElementPropertyInfo()
							{
								Category  = "Layout",
								ValueType = typeof(EditableSize),
								Value     = new EditableSize(200, 100),
								Name      = "MaxSize"
							},

							new GuiDebuggerElementPropertyInfo()
							{
								Category  = "Layout",
								ValueType = typeof(EditableAlignment),
								Value     = new EditableAlignment(Alignment.Fill),
								Name      = "Anchor"
							},

							new GuiDebuggerElementPropertyInfo()
							{
								Category  = "Layout",
								ValueType = typeof(EditableAlignment),
								Value     = new EditableAlignment(Alignment.MiddleFill),
								Name      = "ChildAnchor"
							},

							new GuiDebuggerElementPropertyInfo()
							{
								Category = "Layout",
								ValueType = typeof(EditableThickness),
								Value = new EditableThickness(),
								Name = "Margin"
							},
							new GuiDebuggerElementPropertyInfo()
							{
								Category  = "Layout",
								ValueType = typeof(EditableThickness),
								Value     = new EditableThickness(),
								Name      = "Padding"
							},
						}
					},

					new GuiDebuggerElementPropertyCategoryGroup()
					{
						Name = "Appearance",
						Properties = new FastObservableCollection<GuiDebuggerElementPropertyInfo>()
						{
							new GuiDebuggerElementPropertyInfo()
							{
								Category  = "Appearance",
								ValueType = typeof(EditableGuiTexture),
								Value     = new EditableGuiTexture(),
								Name      = "Background"
							},
							new GuiDebuggerElementPropertyInfo()
							{
								Category  = "Appearance",
								ValueType = typeof(EditableGuiTexture),
								Value     = new EditableGuiTexture(),
								Name      = "BackgroundOverlay"
							},
							new GuiDebuggerElementPropertyInfo()
							{
								Category  = "Appearance",
								ValueType = typeof(EditableColor),
								Value     = new EditableColor(Color.FromNonPremultiplied(0xff, 0x50, 0x3f, 0xff)),
								Name      = "BorderBrush"
							},
							new GuiDebuggerElementPropertyInfo()
							{
								Category  = "Appearance",
								ValueType = typeof(EditableThickness),
								Value     = new EditableThickness(1,1,1,1),
								Name      = "BorderThickness"
							}
						}
					}
				}
			};

		}
	}
}
