using System.Net;
using System.Threading;
using Alex.API.Services;
using Alex.GuiDebugger.Common;
using Alex.GuiDebugger.Common.Services;
using JKang.IpcServiceFramework;
using JKang.IpcServiceFramework.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Alex.Services
{
	public class AlexIpcService : IBackgroundService
	{

		private IHost _host;

		private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

		public AlexIpcService()
		{
			_host = Host.CreateDefaultBuilder()
				.ConfigureServices(services =>
				{
					services.AddScoped<IGuiDebuggerService, GuiDebuggerService>();
				})
				.ConfigureIpcHost(builder =>
				{
					builder.AddNamedPipeEndpoint<IGuiDebuggerService>(options =>
					{
						
						options.PipeName = GuiDebuggerConstants.NamedPipeName;
						options.MaxConcurrentCalls = 2;
					});
				})
				.ConfigureLogging(builder =>
				{
					builder.SetMinimumLevel(LogLevel.Debug);
				})
				.Build();
		}

		public void Start()
		{
			_host.RunAsync(_cancellationTokenSource.Token);
		}

		public void Stop()
		{
			_cancellationTokenSource.Cancel();
		}

	}
}
