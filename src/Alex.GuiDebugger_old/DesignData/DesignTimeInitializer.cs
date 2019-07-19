using Alex.GuiDebugger.DesignData;
using Alex.GuiDebugger.Fonts;
using Catel;
using Catel.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly:DesignTimeCode(typeof(AlexGuiDebuggerDesignTimeInitializer))]

namespace Alex.GuiDebugger.DesignData
{
	
	public class AlexGuiDebuggerDesignTimeInitializer : Catel.DesignTimeInitializer, IServiceLocatorInitializer
	{

		public AlexGuiDebuggerDesignTimeInitializer()
		{

		}

		protected override void Initialize()
        {
            var dependencyResolver = this.GetDependencyResolver();
            
            //FontMaterial.Initialize(false);
            //FontAwesome.Initialize();

            
            var serviceLocator = ServiceLocator.Default;
            Initialize(serviceLocator);
        }

		public void Initialize(IServiceLocator serviceLocator)
		{
            CatelEnvironment.RegisterDefaultViewModelServices();


            FontMaterial.Initialize(false);
            FontAwesome.Initialize(true);

            //if (!EnvironmentHelper.IsProcessCurrentlyHostedByTool(true))
                Orchestra.ThemeHelper.EnsureApplicationThemes($"pack://application:,,,/{typeof(Alex.GuiDebugger.DesignData.AlexGuiDebuggerDesignTimeInitializer).Assembly.FullName};Component/Styles/Controls.xaml", true);

            //if (!EnvironmentHelper.IsProcessCurrentlyHostedByTool(true))
                ThemeHelper.EnsureCatelMvvmThemeIsLoaded();
		}
	}
}
