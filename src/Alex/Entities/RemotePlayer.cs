﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Alex.API.Graphics;
using Alex.API.Network;
using Alex.API.Utils;
using Alex.Graphics.Models.Entity;
using Alex.Net;
using Alex.Networking.Java.Packets.Play;
using Alex.ResourcePackLib;
using Alex.ResourcePackLib.Json;
using Alex.ResourcePackLib.Json.Converters;
using Alex.ResourcePackLib.Json.Models.Entities;
using Alex.ResourcePackLib.Json.Models.Items;
using Alex.Utils;
using Alex.Worlds;
using Alex.Worlds.Multiplayer.Bedrock;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MiNET;
using MiNET.Worlds;
using Newtonsoft.Json;
using NLog;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using LogManager = NLog.LogManager;
using Point = System.Drawing.Point;
using Skin = MiNET.Utils.Skins.Skin;

namespace Alex.Entities
{
	public class RemotePlayer : LivingEntity
	{
		private static readonly Logger   Log = LogManager.GetCurrentClassLogger(typeof(RemotePlayer));
		public  GameMode Gamemode { get; private set; }

		private EntityModel _model;

		public string Name { get; }
		public string GeometryName { get; set; }
		
		public PlayerSkinFlags SkinFlags { get; }

		public int Score   { get; set; } = 0;
		public int Latency { get; set; } = 0;

		private PooledTexture2D _texture;
		public RemotePlayer(string name, World level, NetworkProvider network, PooledTexture2D skinTexture, string geometry = "geometry.humanoid.customSlim") : base(63, level, network)
		{
			_texture = skinTexture;
			
			SkinFlags = new PlayerSkinFlags()
			{
				Value = 0xff
			};
			
			Name = name;

			Width = 0.6;
			//Length = 0.6;
			Height = 1.80;

			IsSpawned = false;

			NameTag = name;

			HideNameTag = false;
			IsAlwaysShowName = true;
			ShowItemInHand = true;
			
			IsInWater = false;
			//NoAi = true;
			
			Velocity = Vector3.Zero;

			GeometryName = geometry;

			_skinDirty = true;
			//UpdateSkin(skinTexture);

			MovementSpeed = 0.1f;//0000000149011612f;//0000000149011612f;
			FlyingSpeed = 0.4f;
			//	MovementSpeed = 0.1f;
			//		FlyingSpeed = 0.4f;

			IsAffectedByGravity = false;
			HasPhysics = false;
		}

		/// <inheritdoc />
		public override bool NoAi {
			get
			{
				return true;
			}
			set
			{
				
			} 
		}
		
		private static JsonSerializerSettings GeometrySerializationSettings = new JsonSerializerSettings()
		{
			Converters = new List<JsonConverter>()
			{
				new SingleOrArrayConverter<Vector3>(),
				new SingleOrArrayConverter<Vector2>(),
				new Vector3Converter(),
				new Vector2Converter()
			},
			MissingMemberHandling = MissingMemberHandling.Ignore
		};

		private Skin _skin      = null;
		private bool _skinDirty = false;
		public Skin Skin
		{
			get
			{
				return _skin;
			}
			set
			{
				_skin = value;
				_skinDirty = true;

				if (IsSpawned)
				{
					QueueSkinProcessing();
				}
			}
		}

		/// <inheritdoc />
		public override void OnSpawn()
		{
			base.OnSpawn();

			if (_skinDirty)
			{
				QueueSkinProcessing();
			}
		}

		private int _skinQueuedCount = 0;
		private void QueueSkinProcessing()
		{
			if (Interlocked.CompareExchange(ref _skinQueuedCount, 1, 0) == 0)
			{
				if (Level?.BackgroundWorker == null)
				{
					ProcessSkin();
				}
				else
				{
					Level.BackgroundWorker.Enqueue(ProcessSkin);
				}
			}
		}

		private void ProcessSkin()
		{
			_skinQueuedCount = 0;
			_skinDirty = false;
			if (_skin == null)
			{
				UpdateSkin(_texture);
			}
			else
			{
				LoadSkin(_skin);
			}
		}

		private void LoadSkin(Skin skin)
		{
			try
			{
				if (skin == null)
					return;

				EntityModel model = null;

				if (!skin.IsPersonaSkin)
				{
					if (!string.IsNullOrWhiteSpace(skin.GeometryData) && !skin.GeometryData.Equals(
						"null", StringComparison.InvariantCultureIgnoreCase))
					{
						try
						{
							if (string.IsNullOrWhiteSpace(skin.ResourcePatch) || skin.ResourcePatch == "null")
							{
								Log.Debug($"Resourcepatch null for player {Name}");
							}
							else
							{
								var resourcePatch = JsonConvert.DeserializeObject<SkinResourcePatch>(
									skin.ResourcePatch, GeometrySerializationSettings);
								
								Dictionary<string, EntityModel> models = new Dictionary<string, EntityModel>();
								BedrockResourcePack.LoadEntityModel(skin.GeometryData, models);

								var processedModels = BedrockResourcePack.ProcessEntityModels(models);

								if (processedModels == null || processedModels.Count == 0)
								{
									Log.Warn($"!! Model count was 0 for player {NameTag} !!");

									if (!Directory.Exists("failed"))
										Directory.CreateDirectory("failed");

									File.WriteAllText(
										Path.Combine(
											"failed",
											$"{Environment.TickCount64}-{resourcePatch.Geometry.Default}.json"),
										skin.GeometryData);
								}
								else
								{
									if (resourcePatch?.Geometry != null)
									{
										if (!processedModels.TryGetValue(resourcePatch.Geometry.Default, out model))
										{
											Log.Debug(
												$"Invalid geometry: {resourcePatch.Geometry.Default} for player {Name}");
										}
										else
										{
											
										}

									}
									else
									{
										Log.Debug($"Resourcepatch geometry was null for player {Name}");
									}
								}
							}
						}
						catch (Exception ex)
						{
							string name = "N/A";
							Log.Debug(ex, $"Could not create geometry ({name}): {ex.ToString()} for player {Name}");
						}
					}
					else
					{
						//Log.Debug($"Geometry data null for player {Name}");
					}
				}

				if (model == null)
				{
					ModelFactory.TryGetModel(
						skin.Slim ? "geometry.humanoid.custom" : "geometry.humanoid.customSlim", out model);

					/*model = skin.Slim ? (EntityModel) new Models.HumanoidCustomslimModel() :
						(EntityModel) new HumanoidModel();*/ // new Models.HumanoidCustomGeometryHumanoidModel();
				}

				if (model != null && ValidateModel(model, Name))
				{
					Image<Rgba32> skinBitmap = null;

					if (!skin.TryGetBitmap(model, out skinBitmap))
					{
						Log.Warn($"No custom skin data for player {Name}");

						if (Alex.Instance.Resources.TryGetBitmap("entity/alex", out var rawTexture))
						{
							skinBitmap = rawTexture;
						}
					}
					
					//if (!Directory.Exists("skins"))
					//	Directory.CreateDirectory("skins");

					//var path = Path.Combine("skins", $"{model.Description.Identifier}-{Environment.TickCount64}");
					//File.WriteAllText($"{path}.json", skin.GeometryData);
					//skinBitmap.SaveAsPng($"{path}.png");
					
					var modelTextureSize = new Point(
						(int) model.Description.TextureWidth, (int) model.Description.TextureHeight);

					var textureSize = new Point(skinBitmap.Width, skinBitmap.Height);

					if (modelTextureSize != textureSize)
					{
						if (modelTextureSize.Y > textureSize.Y)
						{
							skinBitmap = SkinUtils.ConvertSkin(skinBitmap, modelTextureSize.X, modelTextureSize.Y);
						}

						/*var bitmap = skinBitmap;
						bitmap.Mutate<Rgba32>(xx =>
						{
							xx.Resize(modelTextureSize.X, modelTextureSize.Y);
						//	xx.Flip(FlipMode.Horizontal);
						});
	
						skinBitmap = bitmap;*/
					}

					GeometryName = model.Description.Identifier;

					var modelRenderer = new EntityModelRenderer(
						model, TextureUtils.BitmapToTexture2D(Alex.Instance.GraphicsDevice, skinBitmap));

					if (modelRenderer.Valid)
					{
						ModelRenderer = modelRenderer;
					}
					else
					{
						modelRenderer.Dispose();
						Log.Debug($"Invalid model: for player {Name} (Disposing)");
					}
				}
				else
				{
					Log.Debug($"Invalid model for player {Name}");
				}
			}
			catch (Exception ex)
			{
				Log.Warn(ex, $"Error while handling player skin.");
			}
		}
		
		private bool ValidateModel(EntityModel model, string playername)
		{
			bool valid = true;

			if (model.Bones == null || model.Bones.Length == 0)
			{
				valid = false;
				Log.Debug($"Missing bones for player model for player: {playername}");
			}

			return valid;
		}

		/// <inheritdoc />
		protected override void UpdateItemPosition()
		{
			var renderer = ItemRenderer;

			if (renderer == null)
				return;
			
			var pos = renderer.DisplayPosition;
			//if (pos.HasFlag(DisplayPosition.FirstPerson) || pos.HasFlag(DisplayPosition.ThirdPerson))
			{
				if (IsLeftHanded)
				{
					if (!pos.HasFlag(DisplayPosition.LeftHand))
					{
						pos = (pos & ~(DisplayPosition.LeftHand | DisplayPosition.RightHand));
						pos |= DisplayPosition.LeftHand;
					}
				}
				else
				{
					if (!pos.HasFlag(DisplayPosition.RightHand))
					{
						pos = (pos & ~(DisplayPosition.LeftHand | DisplayPosition.RightHand));
						pos |= DisplayPosition.RightHand;
					}
				}

				if (IsFirstPersonMode)
				{
					if (!pos.HasFlag(DisplayPosition.FirstPerson))
					{
						pos = (pos & ~(DisplayPosition.FirstPerson | DisplayPosition.ThirdPerson));
						pos |= DisplayPosition.FirstPerson;
					}
				}
				else
				{
					if (!pos.HasFlag(DisplayPosition.ThirdPerson))
					{
						pos = (pos & ~(DisplayPosition.FirstPerson | DisplayPosition.ThirdPerson));
						pos |= DisplayPosition.ThirdPerson;
					}
				}

				renderer.DisplayPosition = pos;
			}
		}

		/// <inheritdoc />
		protected override void HandleJavaMeta(MetaDataEntry entry)
		{
			base.HandleJavaMeta(entry);

			if (entry.Index == 14 && entry is MetadataFloat flt)
			{
				//Additional hearts
				//HealthManager.MaxHealth 
			}
			else if (entry.Index == 15 && entry is MetadataVarInt score)
			{
				Score = score.Value;
			}
			else if (entry.Index == 16 && entry is MetadataByte data)
			{
				SkinFlags.Value = data.Value;

				if (ModelRenderer != null)
				{
					SkinFlags.ApplyTo(ModelRenderer);
				}
			}
			else if (entry.Index == 17 && entry is MetadataByte metaByte)
			{
				IsLeftHanded = metaByte.Value == 0;
			}
		}

		public void UpdateGamemode(GameMode gamemode)
		{
			Gamemode = gamemode;
		}

		/// <inheritdoc />
		protected override void OnModelUpdated()
		{
			base.OnModelUpdated();

			var modelRenderer = ModelRenderer;
			if (modelRenderer != null && SkinFlags != null)
				SkinFlags.ApplyTo(modelRenderer);
		}

		private        bool                ValidModel { get; set; }
		private static PooledTexture2D     _steve;
		private static PooledTexture2D     _alex;

		internal void UpdateSkin(PooledTexture2D skinTexture)
		{
			if (skinTexture != null && ModelRenderer != null)
			{
				ModelRenderer.Texture = skinTexture;

				return;
			}

			string geometry = "geometry.humanoid.customSlim";

			if (skinTexture == null)
			{
				string skinVariant = "entity/alex";
				skinTexture = _alex;

				var uuid = UUID.GetBytes();

				bool isSteve = (uuid[3] ^ uuid[7] ^ uuid[11] ^ uuid[15]) % 2 == 0;

				if (isSteve)
				{
					skinVariant = "entity/steve";
					geometry = "geometry.humanoid.custom";

					skinTexture = _steve;
				}

				if (skinTexture == null)
				{
					if (Alex.Instance.Resources.TryGetBitmap(skinVariant, out var rawTexture))
					{
						skinTexture = TextureUtils.BitmapToTexture2D(Alex.Instance.GraphicsDevice, rawTexture);

						if (isSteve)
							_steve = skinTexture;
						else
							_alex = skinTexture;

						//skinBitmap = rawTexture;
					}
				}

				if (skinTexture == null)
				{
					skinTexture = TextureUtils.BitmapToTexture2D(Alex.Instance.GraphicsDevice, Alex.PlayerTexture);
				}
			}


			if (ModelFactory.TryGetModel(geometry, out var m))
			{
				_model = m;
				ValidModel = true;
				ModelRenderer = new EntityModelRenderer(_model, skinTexture);

				_texture = skinTexture;
				//UpdateModelParts();
			}
		}
	}
}
