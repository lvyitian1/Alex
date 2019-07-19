using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Alex.GuiDebugger.Common;
using Alex.GuiDebugger.Common.Services;
using Alex.GuiDebugger.Models;
using Catel.Caching;
using Catel.Collections;
using Catel.IoC;
using Microsoft.Xna.Framework;
using RocketUI;

namespace Alex.GuiDebugger.Services
{
	public class GuiDebugDataService : IGuiDebugDataService
	{
		private readonly IGuiDebuggerService _guiDebuggerService;
		private readonly ICacheStorage<Guid, GuiDebuggerElementInfo> _guiDebuggerElementInfoCache;
		public           GuiDebuggerData     GuiDebuggerData { get; }

		public GuiDebugDataService(IGuiDebuggerService guiDebuggerService, ICacheStorage<Guid, GuiDebuggerElementInfo> guiDebuggerElementInfoCache)
		{
			_guiDebuggerService = guiDebuggerService;
			_guiDebuggerElementInfoCache = guiDebuggerElementInfoCache;

			GuiDebuggerData = new GuiDebuggerData();
		}
		
		public void RefreshElements()
		{
			var allElementInfos = _guiDebuggerService.GetAllGuiElementInfos();

			var elements = GuiDebuggerData.Elements;

			var newItems = allElementInfos.Select(ConvertGuiElementInfo).ToArray();

			//using (elements.SuspendChangeNotifications(SuspensionMode.MixedConsolidate))
			{
				elements.ReplaceRange(newItems);
			}
		}

		public void RefreshProperties(GuiDebuggerElementInfo elementInfo)
		{
			var propertyInfos = _guiDebuggerService.GetElementPropertyInfos(elementInfo.Id);

			foreach (var existingElementProp in elementInfo.Properties.ToArray())
			{
				if (propertyInfos.All(x => x.Name != existingElementProp.Name))
				{
					elementInfo.Properties.Remove(existingElementProp);
				}
			}

			foreach (var property in propertyInfos)
			{
				var elementProp = elementInfo.Properties.FirstOrDefault(x => x.Name == property.Name);
				if (elementProp == null)
				{
					elementProp = new GuiDebuggerElementPropertyInfo()
					{
						Name = property.Name
					};

					elementInfo.Properties.Add(elementProp);
				}

				var targetType = Type.GetType(property.Type);

				object value = ConvertPropertyToEditableIfNeeded(property.Value, targetType);

				elementProp.ValueType = value?.GetType() ?? targetType;
				elementProp.Value = value;
			}

		}

		private object ConvertPropertyToEditableIfNeeded(object propValue, Type type)
		{
			if (type == typeof(Thickness))
			{
				return (EditableThickness) ((Thickness)propValue);
			}

			if (type == typeof(Size))
			{
				return (EditableSize) ((Size)propValue);
			}

			if (type == typeof(Rectangle))
			{
				return (EditableRectangle) ((Rectangle) propValue);
			}

			if (type == typeof(Alignment))
			{
				return (EditableAlignment) ((Alignment) propValue);
			}

			if (type == typeof(Color))
			{
				return (EditableColor) ((Color)propValue);
			}

			if (propValue.GetType() != type)
			{
				return Convert.ChangeType(propValue, type);
			}

			return propValue;
		}

		private GuiDebuggerElementInfo ConvertGuiElementInfo(GuiElementInfo guiElementInfo)
		{
			var model = _guiDebuggerElementInfoCache.GetFromCacheOrFetch(guiElementInfo.Id, () => new GuiDebuggerElementInfo()
			{
				Id          = guiElementInfo.Id
			});

			model.ElementType = guiElementInfo.ElementType;
			model.ElementName = guiElementInfo.ElementName;

			if (guiElementInfo.ChildElements != null && guiElementInfo.ChildElements.Any())
			{
				var newChildElements = guiElementInfo.ChildElements.Select(ConvertGuiElementInfo).ToArray();

				//using (model.ChildElements.SuspendChangeNotifications(SuspensionMode.MixedConsolidate))
				{
					model.ChildElements.Clear();
					model.ChildElements.AddRange(newChildElements);
				}
			}

			return model;
		}


	}
}