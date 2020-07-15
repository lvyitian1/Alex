using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Alex.GuiDebugger.Common;
using Alex.GuiDebugger.Common.Services;
using Alex.GuiDebugger.Models;
using JKang.IpcServiceFramework;
using JKang.IpcServiceFramework.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Alex.GuiDebugger.Services
{
    public class AlexGuiDebuggerInteraction
    {
		#region Singleton

		private static AlexGuiDebuggerInteraction _instance;
		public static  AlexGuiDebuggerInteraction Instance
		{
			get
			{
				if (_instance == null)
					_instance = new AlexGuiDebuggerInteraction();
				return _instance;
			}
		}

		#endregion

		private readonly ServiceProvider _serviceProvider;
		private readonly IIpcClient<IGuiDebuggerService> _ipcServiceClient;
		
		public AlexGuiDebuggerInteraction()
		{
			_serviceProvider = new ServiceCollection()
				.AddNamedPipeIpcClient<IGuiDebuggerService>(nameof(Alex.GuiDebugger), pipeName: GuiDebuggerConstants.NamedPipeName)
				.BuildServiceProvider();
			var clientFactory = _serviceProvider.GetRequiredService<IIpcClientFactory<IGuiDebuggerService>>();
			_ipcServiceClient = clientFactory.CreateClient(nameof(Alex.GuiDebugger));
		}

		public async Task HighlightElement(Guid elementId)
		{
			await _ipcServiceClient.InvokeAsync(x => x.HighlightGuiElement(elementId), new CancellationTokenSource(5000).Token);
		}

		public async Task DisableHighlight()
		{
			await _ipcServiceClient.InvokeAsync(x => x.DisableHighlight(), new CancellationTokenSource(5000).Token);
		}

		public async Task<ICollection<ElementTreeItem>> GetElementTreeItems()
		{
			var items = await _ipcServiceClient.InvokeAsync(x => x.GetAllGuiElementInfos(), new CancellationTokenSource(10000).Token);
			return items?.Select(ConvertItem).ToList() ?? new List<ElementTreeItem>();
		}

		
		public async Task<ICollection<ElementTreeItemProperty>> GetElementTreeItemProperties(Guid elementId)
		{
			var items = await _ipcServiceClient.InvokeAsync(x => x.GetElementPropertyInfos(elementId), new CancellationTokenSource(30000).Token);
			return items?.Select(x => ConvertItem(elementId, x)).ToList() ?? new List<ElementTreeItemProperty>();
		}

		private ElementTreeItem ConvertItem(GuiElementInfo guiElementInfo)
		{
			return new ElementTreeItem(guiElementInfo.Id, guiElementInfo.ElementType)
			{
				Children = guiElementInfo.ChildElements?.Any() ?? false ? guiElementInfo.ChildElements.Select(ConvertItem).ToArray() : null
			};
		}

		private ElementTreeItemProperty ConvertItem(Guid elementId, GuiElementPropertyInfo guiElementPropertyInfo)
		{
			return new ElementTreeItemProperty(elementId, guiElementPropertyInfo.Name, guiElementPropertyInfo.Type,
											   guiElementPropertyInfo.Value);
		}
	}
}
