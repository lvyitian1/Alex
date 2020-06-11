using System;
using System.IO;
using System.Net;

namespace Alex
{
	public class LaunchSettings
	{
		public bool       ConnectOnLaunch = false;
		public IPEndPoint Server          = null;

		public string Username;
		public string UUID;
		public string AccesToken;
		public bool   ShowConsole = false;
		public string WorkDir;
		public bool   ConnectToBedrock = false;
		public bool   ModelDebugging   = false;
		
		public LaunchSettings()
		{
			var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create);
			WorkDir = Path.Combine(appData, "Alex");
		}
	}
}