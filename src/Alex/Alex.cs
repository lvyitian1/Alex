﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Alex.API;
using Alex.API.Data.Servers;
using Alex.API.Graphics;
using Alex.API.Graphics.Typography;
using Alex.API.Gui;
using Alex.API.Input;
using Alex.API.Input.Listeners;
using Alex.API.Resources;
using Alex.API.Services;
using Alex.API.Utils;
using Alex.Audio;
using Alex.Blocks.Minecraft;
using Alex.Entities;
using Alex.Gamestates;
using Alex.Gamestates.Debugging;
using Alex.Gamestates.InGame;
using Alex.Gamestates.Login;
using Alex.Graphics.Models.Blocks;
using Alex.Gui;
using Alex.Gui.Elements;
using Alex.Net;
using Alex.Networking.Java.Packets;
using Alex.Networking.Java.Packets.Play;
using Alex.Plugins;
using Alex.ResourcePackLib;
using Alex.ResourcePackLib.Json;
using Alex.ResourcePackLib.Json.Models.Entities;
using Alex.Services;
using Alex.Services.Discord;
using Alex.Utils;
using Alex.Utils.Auth;
using Alex.Worlds.Abstraction;
using Alex.Worlds.Multiplayer.Bedrock;
using Alex.Worlds.Multiplayer.Java;
using Alex.Worlds.Singleplayer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using NLog;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using GuiDebugHelper = Alex.Gui.GuiDebugHelper;
using Image = SixLabors.ImageSharp.Image;
using Point = Microsoft.Xna.Framework.Point;
using TextInputEventArgs = Microsoft.Xna.Framework.TextInputEventArgs;

namespace Alex
{
	public class Alex : Microsoft.Xna.Framework.Game
	{
		public static int  MipMapLevel = 8;
		public static bool InGame { get; set; } = false;

		public static EntityModel   PlayerModel   { get; set; }
		public static Image<Rgba32> PlayerTexture { get; set; }

		private static readonly Logger Log = LogManager.GetCurrentClassLogger(typeof(Alex));

		public static string Gpu             { get; private set; } = "";
		public static string OperatingSystem { get; private set; } = "";
		
#if DIRECTX
		public const string RenderingEngine = "DirectX";
#else
		public const string RenderingEngine = "OpenGL";
#endif

		public static string DotnetRuntime { get; } =
			$"{System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}";

		//public const string Version = "1.0 DEV";

		public static bool IsMultiplayer { get; set; } = false;

		public static IFont Font;

		private SpriteBatch _spriteBatch;

		public static Alex             Instance         { get; private set; }
		public        GameStateManager GameStateManager { get; private set; }

		public ResourceManager Resources { get; private set; }

		public InputManager   InputManager   { get; private set; }
		public GuiRenderer    GuiRenderer    { get; private set; }
		public GuiManager     GuiManager     { get; private set; }
		public GuiDebugHelper GuiDebugHelper { get; private set; }

		public GraphicsDeviceManager DeviceManager { get; }

		internal ConcurrentQueue<Action> UIThreadQueue { get; }

		private LaunchSettings LaunchSettings { get; }
		public  PluginManager  PluginManager  { get; }
		public  FpsMonitor     FpsMonitor     { get; }

		public new IServiceProvider Services { get; set; }

		// public DedicatedThreadPool ThreadPool { get; private set; }

		public StorageSystem     Storage           { get; private set; }
		public ServerTypeManager ServerTypeManager { get; private set; }
		public OptionsProvider   Options           { get; private set; }

		public Alex(LaunchSettings launchSettings)
		{
			EntityProperty.Factory = new AlexPropertyFactory();
			/*MiNET.Utils.DedicatedThreadPool fastThreadPool =
				ReflectionHelper.GetPrivateStaticPropertyValue<MiNET.Utils.DedicatedThreadPool>(
					typeof(MiNetServer), "FastThreadPool");

			fastThreadPool?.Dispose();
			fastThreadPool?.WaitForThreadsExit();
			
			ReflectionHelper.SetPrivateStaticPropertyValue<MiNET.Utils.DedicatedThreadPool>(
				typeof(MiNetServer), "FastThreadPool",
				new MiNET.Utils.DedicatedThreadPool(
					new MiNET.Utils.DedicatedThreadPoolSettings(8, "MiNETServer Fast")));*/

			ThreadPool.GetMaxThreads(out _, out var completionPortThreads);
			//ThreadPool.SetMaxThreads(Environment.ProcessorCount, completionPortThreads);
			
			Instance = this;
			LaunchSettings = launchSettings;

			OperatingSystem =
				$"{System.Runtime.InteropServices.RuntimeInformation.OSDescription} ({System.Runtime.InteropServices.RuntimeInformation.OSArchitecture})";

			DeviceManager = new GraphicsDeviceManager(this)
			{
				PreferMultiSampling = false,
				SynchronizeWithVerticalRetrace = false,
				GraphicsProfile = GraphicsProfile.Reach,
			};

			DeviceManager.PreparingDeviceSettings += (sender, args) =>
			{
				Gpu = args.GraphicsDeviceInformation.Adapter.Description;
				args.GraphicsDeviceInformation.PresentationParameters.DepthStencilFormat = DepthFormat.Depth24Stencil8;
				DeviceManager.PreferMultiSampling = true;
			};

			Content = new StreamingContentManager(base.Services, "assets");
			//	Content.RootDirectory = "assets";

			IsFixedTimeStep = false;
			// graphics.ToggleFullScreen();

			this.Window.AllowUserResizing = true;

			this.Window.ClientSizeChanged += (sender, args) =>
			{
				if (DeviceManager.PreferredBackBufferWidth != Window.ClientBounds.Width
				    || DeviceManager.PreferredBackBufferHeight != Window.ClientBounds.Height)
				{
					if (DeviceManager.IsFullScreen)
					{
						DeviceManager.PreferredBackBufferWidth =
							GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

						DeviceManager.PreferredBackBufferHeight =
							GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
					}
					else
					{
						DeviceManager.PreferredBackBufferWidth = Window.ClientBounds.Width;
						DeviceManager.PreferredBackBufferHeight = Window.ClientBounds.Height;
					}

					DeviceManager.ApplyChanges();

					//CefWindow.Size = new System.Drawing.Size(Window.ClientBounds.Width, Window.ClientBounds.Height);
				}
			};


			JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
			{
				Converters = new List<JsonConverter>() {new Texture2DJsonConverter(GraphicsDevice)},
				Formatting = Formatting.Indented
			};

			ServerTypeManager = new ServerTypeManager();
			PluginManager = new PluginManager();

			Storage = new StorageSystem(LaunchSettings.WorkDir);
			Options = new OptionsProvider(Storage);

			IServiceCollection serviceCollection = new ServiceCollection();
			serviceCollection.AddSingleton<Alex>(this);
			serviceCollection.AddSingleton<ContentManager>(Content);
			serviceCollection.AddSingleton<IStorageSystem>(Storage);
			serviceCollection.AddSingleton<IOptionsProvider>(Options);

			InitiatePluginSystem(serviceCollection);
			
			AudioEngine = new AudioEngine(Storage);

			ConfigureServices(serviceCollection);

			Services = serviceCollection.BuildServiceProvider();

			PluginManager.Setup(Services);

			PluginManager.LoadPlugins();

			ServerTypeManager.TryRegister("java", new JavaServerType(this));

			ServerTypeManager.TryRegister(
				"bedrock", new BedrockServerType(this, Services.GetService<XboxAuthService>()));

			UIThreadQueue = new ConcurrentQueue<Action>();

			FpsMonitor = new FpsMonitor();

			Resources = Services.GetRequiredService<ResourceManager>();

			// ThreadPool = new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount,
			//    ThreadType.Background, "Dedicated ThreadPool"));

			KeyboardInputListener.InstanceCreated += KeyboardInputCreated;

			TextureUtils.RenderThread = Thread.CurrentThread;
			TextureUtils.QueueOnRenderThread = action => UIThreadQueue.Enqueue(action);
		}

		/// <inheritdoc />
		protected override void OnExiting(object sender, EventArgs args)
		{
			GpuResourceManager.ReportIncorrectlyDisposedBuffers = false;
			base.OnExiting(sender, args);
		}

		private void KeyboardInputCreated(object sender, KeyboardInputListener e)
		{
			var bindings = KeyBinds.DefaultBindings;

			if (Storage.TryReadJson($"controls", out Dictionary<InputCommand, Keys> loadedBindings))
			{
				bindings = loadedBindings;
			}

			foreach (var binding in bindings)
			{
				e.RegisterMap(binding.Key, binding.Value);
			}
		}

		public static EventHandler<TextInputEventArgs> OnCharacterInput;

		private void Window_TextInput(object sender, TextInputEventArgs e)
		{
			OnCharacterInput?.Invoke(this, e);
		}

		protected override void Initialize()
		{
			Window.Title = $"Alex - {VersionUtils.GetVersion()} - {RenderingEngine}";

			// InitCamera();
			this.Window.TextInput += Window_TextInput;

			if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				var currentAdapter = GraphicsAdapter.Adapters.FirstOrDefault(x => x == GraphicsDevice.Adapter);

				if (currentAdapter != null)
				{
					if (currentAdapter.IsProfileSupported(GraphicsProfile.HiDef))
					{
						DeviceManager.GraphicsProfile = GraphicsProfile.HiDef;
					}
				}
			}
			
			base.InactiveSleepTime = TimeSpan.Zero;

			GraphicsDevice.PresentationParameters.MultiSampleCount = 8;

			DeviceManager.ApplyChanges();

			base.Initialize();

			RichPresenceProvider.Initialize();
		}
		
		protected override void LoadContent()
		{
			Stopwatch loadingStopwatch = Stopwatch.StartNew();
			
			var builtInFont = ResourceManager.ReadResource("Alex.Resources.default_font.png");
			
			var image = Image.Load<Rgba32>(builtInFont);
			OnResourcePackPreLoadCompleted(image, McResourcePack.BitmapFontCharacters.ToList());
			
			var       options          = Services.GetService<IOptionsProvider>();
			options.Load();

			//	DebugFont = (WrappedSpriteFont) Content.Load<SpriteFont>("Alex.Resources.DebugFont.xnb");

			//	ResourceManager.EntityEffect = Content.Load<Effect>("Alex.Resources.Entityshader.xnb").Clone();
#if DIRECTX
			ResourceManager.BlockEffect = Content.Load<Effect>("Alex.Resources.Blockshader_dx.xnb").Clone();
			ResourceManager.LightingEffect = Content.Load<Effect>("Alex.Resources.Lightmap_dx.xnb").Clone();
#else
			ResourceManager.BlockEffect = Content.Load<Effect>("Alex.Resources.Blockshader.xnb").Clone();
			ResourceManager.LightingEffect = Content.Load<Effect>("Alex.Resources.Lightmap.xnb").Clone();
#endif
			//	ResourceManager.BlockEffect.GraphicsDevice = GraphicsDevice;

			_spriteBatch = new SpriteBatch(GraphicsDevice);
			InputManager = new InputManager(this);

			GuiRenderer = new GuiRenderer();
			//GuiRenderer.Init(GraphicsDevice);

			GuiManager = new GuiManager(this, Services, InputManager, GuiRenderer, options);
			GuiManager.Init(GraphicsDevice, Services);

			options.AlexOptions.VideoOptions.FancyGraphics.Bind(
				(value, newValue) =>
				{
					Block.FancyGraphics = newValue;
				});

			Block.FancyGraphics = options.AlexOptions.VideoOptions.FancyGraphics.Value;
			
			options.AlexOptions.VideoOptions.UseVsync.Bind((value, newValue) => { SetVSync(newValue); });

			if (options.AlexOptions.VideoOptions.UseVsync.Value)
			{
				SetVSync(true);
			}

			options.AlexOptions.VideoOptions.Fullscreen.Bind((value, newValue) => { SetFullscreen(newValue); });

			if (options.AlexOptions.VideoOptions.Fullscreen.Value)
			{
				SetFullscreen(true);
			}

			options.AlexOptions.VideoOptions.LimitFramerate.Bind(
				(value, newValue) =>
				{
					SetFrameRateLimiter(newValue, options.AlexOptions.VideoOptions.MaxFramerate.Value);
				});

			options.AlexOptions.VideoOptions.MaxFramerate.Bind(
				(value, newValue) =>
				{
					SetFrameRateLimiter(options.AlexOptions.VideoOptions.LimitFramerate.Value, newValue);
				});

			if (options.AlexOptions.VideoOptions.LimitFramerate.Value)
			{
				SetFrameRateLimiter(true, options.AlexOptions.VideoOptions.MaxFramerate.Value);
			}

			options.AlexOptions.VideoOptions.Antialiasing.Bind(
				(value, newValue) => { SetAntiAliasing(newValue > 0, newValue); });

			options.AlexOptions.MiscelaneousOptions.Language.Bind(
				(value, newValue) => { GuiRenderer.SetLanguage(newValue); });

			if (!GuiRenderer.SetLanguage(options.AlexOptions.MiscelaneousOptions.Language))
			{
				GuiRenderer.SetLanguage(CultureInfo.InstalledUICulture.Name);
			}

			options.AlexOptions.VideoOptions.SmoothLighting.Bind(
				(value, newValue) => { ResourcePackBlockModel.SmoothLighting = newValue; });

			ResourcePackBlockModel.SmoothLighting = options.AlexOptions.VideoOptions.SmoothLighting.Value;

			SetAntiAliasing(
				options.AlexOptions.VideoOptions.Antialiasing > 0, options.AlexOptions.VideoOptions.Antialiasing.Value);

			GuiDebugHelper = new GuiDebugHelper(GuiManager);

			OnCharacterInput += GuiManager.FocusManager.OnTextInput;

			GameStateManager = new GameStateManager(GraphicsDevice, _spriteBatch, GuiManager);

			var splash = new SplashScreen();
			GameStateManager.AddState("splash", splash);
			GameStateManager.SetActiveState("splash");

			WindowSize = this.Window.ClientBounds.Size;

			//	Log.Info($"Initializing Alex...");
			ThreadPool.QueueUserWorkItem(
				(o) =>
				{
					try
					{
						InitializeGame(splash);
					}
					catch (Exception ex)
					{
						Log.Error(ex, $"Could not initialize! {ex}");
					}
					finally
					{
						loadingStopwatch.Stop();
						Log.Info($"Startup time: {loadingStopwatch.Elapsed}");
					}
				});
		}

		private void InitiatePluginSystem(IServiceCollection serviceCollection)
		{
			string pluginDirectoryPaths =
				Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

			var pluginDir = Options.AlexOptions.ResourceOptions.PluginDirectory;

			if (!string.IsNullOrWhiteSpace(pluginDir))
			{
				pluginDirectoryPaths = pluginDir;
			}
			else
			{
				if (!string.IsNullOrWhiteSpace(LaunchSettings.WorkDir) && Directory.Exists(LaunchSettings.WorkDir))
				{
					pluginDirectoryPaths = LaunchSettings.WorkDir;
				}
			}

			List<string> paths = new List<string>();

			foreach (string dirPath in pluginDirectoryPaths.Split(
				new char[] {';'}, StringSplitOptions.RemoveEmptyEntries))
			{
				string directory = dirPath;

				if (!Path.IsPathRooted(directory))
				{
					directory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), dirPath);
				}

				paths.Add(directory);
				//PluginManager.DiscoverPlugins(directory);
			}

			PluginManager.DiscoverPlugins(paths.ToArray());
			PluginManager.ConfigureServices(serviceCollection);
		}

		private void SetAntiAliasing(bool enabled, int count)
		{
			UIThreadQueue.Enqueue(
				() =>
				{
					DeviceManager.PreferMultiSampling = enabled;
					GraphicsDevice.PresentationParameters.MultiSampleCount = count;

					DeviceManager.ApplyChanges();
				});
		}

		private void SetFrameRateLimiter(bool enabled, int frameRateLimit)
		{
			UIThreadQueue.Enqueue(
				() =>
				{
					base.IsFixedTimeStep = enabled;
					base.TargetElapsedTime = TimeSpan.FromSeconds(1d / frameRateLimit);
				});
		}

		private void SetVSync(bool enabled)
		{
			UIThreadQueue.Enqueue(
				() =>
				{
					base.IsFixedTimeStep = enabled;
					DeviceManager.SynchronizeWithVerticalRetrace = enabled;
					DeviceManager.ApplyChanges();
				});
		}

		private Point  WindowSize  { get; set; }
		public  AudioEngine AudioEngine { get; set; }

		private void SetFullscreen(bool enabled)
		{
			UIThreadQueue.Enqueue(
				() =>
				{
					if (this.DeviceManager.IsFullScreen != enabled)
					{
						if (enabled)
						{
							WindowSize = Window.ClientBounds.Size;
						}
						else
						{
							DeviceManager.PreferredBackBufferWidth = WindowSize.X;
							DeviceManager.PreferredBackBufferHeight = WindowSize.Y;
							this.DeviceManager.ApplyChanges();
						}

						this.DeviceManager.IsFullScreen = enabled;
						this.DeviceManager.ApplyChanges();
					}
				});
		}

		private void ConfigureServices(IServiceCollection services)
		{
			services.TryAddSingleton<ProfileManager>();

			services.TryAddSingleton<IListStorageProvider<SavedServerEntry>, SavedServerDataProvider>();

			services.TryAddSingleton<IServerQueryProvider>(new JavaServerQueryProvider(this));
			services.TryAddSingleton<IPlayerProfileService, PlayerProfileService>();

			services.TryAddSingleton<IRegistryManager, RegistryManager>();

			services.TryAddSingleton<ResourceManager>();
			services.TryAddSingleton<GuiManager>((o) => this.GuiManager);
			services.TryAddSingleton<ServerTypeManager>(ServerTypeManager);
			services.TryAddSingleton<XboxAuthService>();
			
			services.TryAddSingleton<BlobCache>();
			; //Storage = storage;
		}

		protected override void UnloadContent()
		{
			//ProfileManager.SaveProfiles();

			Services.GetService<IOptionsProvider>().Save();

			GuiDebugHelper.Dispose();

			PluginManager.UnloadAll();
		}

		protected override void Update(GameTime gt)
		{
			if (!UIThreadQueue.IsEmpty && UIThreadQueue.TryDequeue(out Action a))
			{
				try
				{
					a.Invoke();
				}
				catch (Exception ex)
				{
					Log.Warn($"Exception on UIThreadQueue: {ex.ToString()}");
				}
			}

			InputManager.Update(gt);

			GuiManager.Update(gt);
			GameStateManager.Update(gt);
			GuiDebugHelper.Update(gt);
		}

		protected override void Draw(GameTime gameTime)
		{
			//GraphicsDevice.Present();
			FpsMonitor.Update();
			GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;

			GameStateManager.Draw(gameTime);
			GuiManager.Draw(gameTime);

			this.Metrics = GraphicsDevice.Metrics;
		}

		public GraphicsMetrics Metrics { get; set; } = new GraphicsMetrics();

		private void InitializeGame(IProgressReceiver progressReceiver)
		{
			progressReceiver.UpdateProgress(0, "Initializing...");

			API.Extensions.Init(GraphicsDevice);
			MCPacketFactory.Load();
			//ConfigureServices();

			//var options = Services.GetService<IOptionsProvider>();

			//	Log.Info($"Loading resources...");
			if (!Resources.CheckResources(GraphicsDevice, progressReceiver, OnResourcePackPreLoadCompleted))
			{
				Console.WriteLine("Press enter to exit...");
				Console.ReadLine();
				Exit();

				return;
			}

			var profileManager = Services.GetService<ProfileManager>();
			profileManager.LoadProfiles(progressReceiver);

			//GuiRenderer.LoadResourcePack(Resources.ResourcePack, null);
			AnvilWorldProvider.LoadBlockConverter();

			PluginManager.EnablePlugins();

			var storage = Services.GetRequiredService<IStorageSystem>();

			if (storage.TryReadString("skin.json", out var str, Encoding.UTF8))
			{
				Dictionary<string, EntityModel> models = new Dictionary<string, EntityModel>();
				BedrockResourcePack.LoadEntityModel(str, models);
				models = BedrockResourcePack.ProcessEntityModels(models);
				
				if (models.Count > 0)
				{
					if (models.TryGetValue("geometry.humanoid.custom", out var entityModel) || models.TryGetValue(
						"geometry.humanoid.customSlim", out entityModel))
					{
						PlayerModel = entityModel;
						Log.Info($"Player model loaded...");
					}
				}
			}

			if (PlayerModel == null)
			{
				if (ModelFactory.TryGetModel("geometry.humanoid.customSlim", out var model))
				{
					//model.Name = "geometry.humanoid.customSlim";
					PlayerModel = model;
				}
			}

			if (PlayerModel != null)
			{
				//Log.Info($"Player model loaded...");
			}

			if (storage.TryReadBytes("skin.png", out byte[] skinBytes))
			{
				using (var skinImage = Image.Load<Rgba32>(skinBytes))
				{
					//var modelTextureSize = PlayerModel.Description != null ?
					//	new Point((int) PlayerModel.Description.TextureWidth, (int) PlayerModel.Description.TextureHeight) :
					//	new Point((int) PlayerModel.Texturewidth, (int) PlayerModel.Textureheight);

					var modelTextureSize = new Point(0, 0);

					if (PlayerModel.Description != null)
					{
						modelTextureSize.X = (int) PlayerModel.Description.TextureWidth;
						modelTextureSize.Y = (int) PlayerModel.Description.TextureHeight;
					}

					if (modelTextureSize.Y > skinImage.Height)
					{
						PlayerTexture = SkinUtils.ConvertSkin(skinImage, modelTextureSize.X, modelTextureSize.Y);
					}
					else
					{
						PlayerTexture = skinImage.Clone();//.Clone<Rgba32>();
					}

					/*
					var textureSize = new Point(skinImage.Width, skinImage.Height);

					if (modelTextureSize != textureSize)
					{
						int newHeight = modelTextureSize.Y > textureSize.Y ? textureSize.Y : modelTextureSize.Y;
						int newWidth  = modelTextureSize.X > textureSize.X ? textureSize.X: modelTextureSize.X;
					
						//skinImage.Mutate<Rgba32>(x => x.Resize(newWidth, newHeight));
					
						Image<Rgba32> skinTexture = new Image<Rgba32>(Math.Max(modelTextureSize.X, skinImage.Width), Math.Max(modelTextureSize.Y, skinImage.Height));
						skinTexture.Mutate<Rgba32>(
							c =>
							{
								c.DrawImage(skinImage, new SixLabors.ImageSharp.Point(0, 0), 1f);
							});
					
						PlayerTexture = skinTexture;
					}
					else
					{
						PlayerTexture = skinImage.Clone<Rgba32>();
					}*/
				}
			}
			else
			{
				if (Resources.TryGetBitmap("entity/alex", out var img))
				{
					PlayerTexture = img;
				}
			}

			if (PlayerTexture != null)
			{
				Log.Info($"Player skin loaded...");
			}

			if (LaunchSettings.ModelDebugging)
			{
				GameStateManager.SetActiveState<ModelDebugState>();
			}
			else
			{
				GameStateManager.SetActiveState<TitleState>("title");
			}

			GameStateManager.RemoveState("splash");
			
		}

		private void OnResourcePackPreLoadCompleted(Image<Rgba32> fontBitmap, List<char> bitmapCharacters)
		{
			UIThreadQueue.Enqueue(
				() =>
				{
					Font = new BitmapFont(GraphicsDevice, fontBitmap, 16, bitmapCharacters);

					GuiManager.ApplyFont(Font);
				});
		}

		public void ConnectToServer(ServerTypeImplementation serverType,
			ServerConnectionDetails connectionDetails,
			PlayerProfile profile)
		{
			try
			{
				WorldProvider   provider;
				NetworkProvider networkProvider;
				IsMultiplayer = true;

				if (serverType.TryGetWorldProvider(connectionDetails, profile, out provider, out networkProvider))
				{
					LoadWorld(provider, networkProvider, true);
				}
			}
			catch (Exception ex)
			{
				Log.Error(ex, $"FCL: {ex.ToString()}");
			}
		}

		public void LoadWorld(WorldProvider worldProvider, NetworkProvider networkProvider, bool isServer = false)
		{
			PlayingState playState = new PlayingState(this, GraphicsDevice, worldProvider, networkProvider);

			var               parentState   = GameStateManager.GetActiveState();

			if (parentState is PlayingState)
				parentState = null;
			
			LoadingWorldScreen loadingScreen = new LoadingWorldScreen();
			loadingScreen.ConnectingToServer = isServer;
			loadingScreen.CancelAction = () =>
			{
				GuiManager.RemoveScreen(loadingScreen);
				worldProvider?.Dispose();
			};

			GuiManager.AddScreen(loadingScreen);
			//GameStateManager.AddState("loading", loadingScreen);
			//GameStateManager.SetActiveState("loading");

			ThreadPool.QueueUserWorkItem(
				o =>
				{
					try
					{
						GameStateManager.RemoveState("play");
						
						var result = worldProvider.Load(loadingScreen.UpdateProgress);

						if (networkProvider.IsConnected && result == LoadResult.Done)
						{
							GameStateManager.AddState("play", playState);
							GameStateManager.SetActiveState("play");
							
							return;
						}

						if (result != LoadResult.Aborted)
						{
							if (!(GameStateManager.GetActiveState() is DisconnectedScreen))
							{
								var s = new DisconnectedScreen();
								s.DisconnectedTextElement.TranslationKey = "multiplayer.status.cannot_connect";
								s.ParentState = parentState;
								GameStateManager.SetActiveState(s, false);

								worldProvider.Dispose();
							}
						}
					}
					finally
					{
						GuiManager.RemoveScreen(loadingScreen);
						//GameStateManager.RemoveState("loading");
					}
				});
		}
	}

	public interface IProgressReceiver
	{
		void UpdateProgress(int percentage, string statusMessage);
		void UpdateProgress(int percentage, string statusMessage, string sub);
		
		void UpdateProgress(int done, int total, string statusMessage) =>
			UpdateProgress((int)(((double)done / (double)total) * 100D), statusMessage);
		
		void UpdateProgress(int done, int total, string statusMessage, string sub)=>
			UpdateProgress((int)(((double)done / (double)total) * 100D), statusMessage, sub);
    }
}