using Alex.API.Blocks;
using Alex.ResourcePackLib.Json.Models.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Alex.Graphics.Models.Entity
{
	public sealed class Cube
	{
		private static readonly Color   DefaultColor = Color.White;
		private readonly        Vector2 _textureSize;

		private readonly EntityModel _model;
		private readonly bool        _mirror = false;

		public Cube(EntityModel model, EntityModelCube cube, Vector2 textureSize, bool mirrored, float inflation)
		{
			_model = model;
			_mirror = mirrored;
			//Mirrored = mirrored;
			var uv     = cube.Uv ?? new EntityModelUV();
			var size   = cube.InflatedSize(inflation);
			
			this._textureSize = textureSize;
			//		var xScale = (textureSize.X / model.Description.TextureWidth);
			//		var yScale = (textureSize.Y / model.Description.TextureHeight);

			//front verts with position and texture stuff
			_topLeftFront = new Vector3(0f, size.Y, 0f);
			_topLeftBack = new Vector3(0f, size.Y, size.Z); // * size;

			_topRightFront = new Vector3(size.X, size.Y, 0f); // * size;
			_topRightBack = new Vector3(size.X, size.Y, size.Z); // * size;

			// Calculate the position of the vertices on the bottom face.
			_btmLeftFront = new Vector3(0f, 0f, 0f); // * size;
			_btmLeftBack = new Vector3(0f, 0f, size.Z); //* size;
			_btmRightFront = new Vector3(size.X, 0f, 0f); // * size;
			_btmRightBack = new Vector3(size.X, 0f, size.Z); // * size;

			Front = Modify(
				cube,
				GetFrontVertex(
					uv.South.WithSize(size.X, size.Y),
					uv.South.Size.HasValue ? Vector2.Zero : new Vector2(cube.Size.Z, cube.Size.Z)));

			Back = Modify(
				cube,
				GetBackVertex(
					uv.North.WithSize(size.X, size.Y),
					uv.North.Size.HasValue ? Vector2.Zero : new Vector2(
						cube.Size.Z + cube.Size.Z + cube.Size.X, cube.Size.Z)));

			Left = Modify(
				cube,
				GetLeftVertex(
					uv.West.WithSize(size.Z, size.Y),
					uv.West.Size.HasValue ? Vector2.Zero : new Vector2(0, cube.Size.Z)));

			Right = Modify(
				cube,
				GetRightVertex(
					uv.East.WithSize(size.Z, size.Y),
					uv.East.Size.HasValue ? Vector2.Zero : new Vector2(cube.Size.Z + cube.Size.X, cube.Size.Z)));

			Top = Modify(
				cube,
				GetTopVertex(
					uv.Up.WithSize(size.X, size.Z), uv.Up.Size.HasValue ? Vector2.Zero : new Vector2(cube.Size.Z, 0)));

			Bottom = Modify(
				cube,
				GetBottomVertex(
					uv.Down.WithSize(size.X, size.Z),
					uv.Down.Size.HasValue ? Vector2.Zero : new Vector2(cube.Size.Z + cube.Size.X, 0)));
		}

		private (VertexPositionColorTexture[] vertices, short[] indexes) Modify(EntityModelCube cube,
			(VertexPositionColorTexture[] vertices, short[] indexes) data)
		{
			/*	MCMatrix cubeMatrix = MCMatrix.Identity;
				if (cube.Rotation.HasValue)
				{
					var rotation = cube.Rotation.Value;

					cubeMatrix = MCMatrix.CreateTranslation(-_pivot)
					             * MCMatrix.CreateRotationDegrees(rotation)
					             * MCMatrix.CreateTranslation(_pivot);
				}*/

			return (data.vertices, data.indexes);
		}

		public (VertexPositionColorTexture[] vertices, short[] indexes) Front, Back, Left, Right, Top, Bottom;

		private readonly Vector3 _topLeftFront;
		private readonly Vector3 _topLeftBack;
		private readonly Vector3 _topRightFront;
		private readonly Vector3 _topRightBack;
		private readonly Vector3 _btmLeftFront;
		private readonly Vector3 _btmLeftBack;
		private readonly Vector3 _btmRightFront;
		private readonly Vector3 _btmRightBack;

		private (VertexPositionColorTexture[] vertices, short[] indexes) GetLeftVertex(EntityModelUVData uv,
			Vector2 size)
		{
			//Vector3 normal = new Vector3(-1.0f, 0.0f, 0.0f) * Size;
			Color normal = Model.AdjustColor(DefaultColor, BlockFace.West);

			//var map = GetTextureMapping(uv + new Vector2(Size.Z + Size.X, Size.Z), Size.Z, Size.Y);
			var map = GetTextureMapping(uv.Origin + size, uv.Size.Value.X, uv.Size.Value.Y);

			// Add the vertices for the RIGHT face. 
			return (new VertexPositionColorTexture[]
			{
				new VertexPositionColorTexture(_topLeftFront, normal, map.TopRight),
				new VertexPositionColorTexture(_btmLeftFront, normal, map.BotRight),
				new VertexPositionColorTexture(_btmLeftBack, normal, map.BotLeft),
				new VertexPositionColorTexture(_topLeftBack, normal, map.TopLeft),
				//new VertexPositionNormalTexture(_topLeftFront , normal, map.TopLeft),
				//new VertexPositionNormalTexture(_btmLeftBack, normal, map.BotRight),
			}, new short[]
			{
				0, 1, 2, 3, 0, 2
				//0, 1, 2, 3, 0, 2
			});
		}

		private (VertexPositionColorTexture[] vertices, short[] indexes) GetRightVertex(EntityModelUVData uv,
			Vector2 size)
		{
			//Vector3 normal = new Vector3(1.0f, 0.0f, 0.0f) * Size;
			Color normal = Model.AdjustColor(DefaultColor, BlockFace.East);

			var map = GetTextureMapping(uv.Origin + size, uv.Size.Value.X, uv.Size.Value.Y);
			//var map = GetTextureMapping(uv + new Vector2(0, Size.Z), Size.Z, Size.Y);

			// Add the vertices for the RIGHT face. 
			return (new VertexPositionColorTexture[]
			{
				new VertexPositionColorTexture(_topRightFront, normal, map.TopLeft),
				new VertexPositionColorTexture(_btmRightBack, normal, map.BotRight),
				new VertexPositionColorTexture(_btmRightFront, normal, map.BotLeft),
				new VertexPositionColorTexture(_topRightBack, normal, map.TopRight),
				//new VertexPositionNormalTexture(_btmRightBack , normal, map.BotLeft),
				//new VertexPositionNormalTexture(_topRightFront, normal, map.TopRight),
			}, new short[] {0, 1, 2, 3, 1, 0});
		}

		private (VertexPositionColorTexture[] vertices, short[] indexes) GetFrontVertex(EntityModelUVData uv,
			Vector2 size)
		{
			//Vector3 normal = new Vector3(0.0f, 0.0f, 1.0f) * Size;
			Color normal = Model.AdjustColor(DefaultColor, BlockFace.South);

			var map = GetTextureMapping(uv.Origin + size, uv.Size.Value.X, uv.Size.Value.Y);

			// Add the vertices for the RIGHT face. 
			return (new VertexPositionColorTexture[]
			{
				new VertexPositionColorTexture(_topLeftFront, normal, map.TopLeft),
				new VertexPositionColorTexture(_topRightFront, normal, map.TopRight),
				new VertexPositionColorTexture(_btmLeftFront, normal, map.BotLeft),
				//new VertexPositionNormalTexture(_btmLeftFront , normal, map.BotLeft),
				//new VertexPositionNormalTexture(_topRightFront, normal, map.TopRight),
				new VertexPositionColorTexture(_btmRightFront, normal, map.BotRight),
			}, new short[]
			{
				0, 1, 2, 2, 1, 3
				//0, 2, 1, 2, 3, 1
			});
		}

		private (VertexPositionColorTexture[] vertices, short[] indexes) GetBackVertex(EntityModelUVData uv,
			Vector2 size)
		{
			//Vector3 normal = new Vector3(0.0f, 0.0f, -1.0f) * Size;
			Color normal = Model.AdjustColor(DefaultColor, BlockFace.North);

			var map = GetTextureMapping(uv.Origin + size, uv.Size.Value.X, uv.Size.Value.Y);

			// Add the vertices for the RIGHT face. 
			return (new VertexPositionColorTexture[]
			{
				new VertexPositionColorTexture(_topLeftBack, normal, map.TopRight),
				new VertexPositionColorTexture(_btmLeftBack, normal, map.BotRight),
				new VertexPositionColorTexture(_topRightBack, normal, map.TopLeft),
				//new VertexPositionNormalTexture(_btmLeftBack , normal, map.BotRight),
				new VertexPositionColorTexture(_btmRightBack, normal, map.BotLeft),
				//new VertexPositionNormalTexture(_topRightBack, normal, map.TopLeft),
			}, new short[]
			{
				0, 1, 2, 1, 3, 2
				//0, 1, 2, 1, 3, 2
			});
		}

		private (VertexPositionColorTexture[] vertices, short[] indexes) GetTopVertex(EntityModelUVData uv,
			Vector2 size)
		{
			//	Vector3 normal = new Vector3(0.0f, 1.0f, 0.0f) * Size;
			Color normal = Model.AdjustColor(DefaultColor, BlockFace.Up);

			var map = GetTextureMapping(uv.Origin + size, uv.Size.Value.X, uv.Size.Value.Y);

			// Add the vertices for the RIGHT face. 
			return (new VertexPositionColorTexture[]
			{
				new VertexPositionColorTexture(_topLeftFront, normal, map.BotLeft),
				new VertexPositionColorTexture(_topLeftBack, normal, map.TopLeft),
				new VertexPositionColorTexture(_topRightBack, normal, map.TopRight),
				//new VertexPositionNormalTexture(_topLeftFront , normal, map.BotLeft),
				//	new VertexPositionNormalTexture(_topRightBack , normal, map.TopRight),
				new VertexPositionColorTexture(_topRightFront, normal, map.BotRight),
			}, new short[] {0, 1, 2, 0, 2, 3});
		}

		private (VertexPositionColorTexture[] vertices, short[] indexes) GetBottomVertex(EntityModelUVData uv,
			Vector2 size)
		{
			//	Vector3 normal = new Vector3(0.0f, -1.0f, 0.0f) * Size;
			Color normal = Model.AdjustColor(DefaultColor, BlockFace.Down);
			var   map    = GetTextureMapping(uv.Origin + size, uv.Size.Value.X, uv.Size.Value.Y);

			// Add the vertices for the RIGHT face. 
			return (new VertexPositionColorTexture[]
			{
				new VertexPositionColorTexture(_btmLeftFront, normal, map.TopLeft),
				new VertexPositionColorTexture(_btmRightBack, normal, map.BotRight),
				new VertexPositionColorTexture(_btmLeftBack, normal, map.BotLeft),
				//new VertexPositionNormalTexture(_btmLeftFront , normal, map.TopLeft),
				new VertexPositionColorTexture(_btmRightFront, normal, map.TopRight),
				//new VertexPositionNormalTexture(_btmRightBack , normal, map.BotRight),
			}, new short[] {0, 1, 2, 0, 3, 1});
		}

		private TextureMapping GetTextureMapping(Vector2 textureOffset, float regionWidth, float regionHeight)
		{
			return new TextureMapping(
				new Vector2(_model.Description.TextureWidth, _model.Description.TextureHeight), _textureSize,
				textureOffset, regionWidth, regionHeight, _mirror);
		}

		private class TextureMapping
		{
			public Vector2 TopLeft  { get; }
			public Vector2 TopRight { get; }
			public Vector2 BotLeft  { get; }
			public Vector2 BotRight { get; }

			public TextureMapping(Vector2 modelTexture,
				Vector2 textureSize,
				Vector2 textureOffset,
				float width,
				float height,
				bool mirrored)
			{
				var texelWidth  = (1f / textureSize.X);
				var texelHeight = (1f / textureSize.Y);

				var x1 = texelWidth * textureOffset.X;
				var x2 = x1 + (width * texelWidth);
				var y1 = texelHeight * (textureOffset.Y);
				var y2 = y1 + (height * texelHeight);

				if (mirrored)
				{
					TopLeft = new Vector2(x2, y1);
					TopRight = new Vector2(x1, y1);
					BotLeft = new Vector2(x2, y2);
					BotRight = new Vector2(x1, y2);
				}
				else
				{
					TopLeft = new Vector2(x1, y1);
					TopRight = new Vector2(x2, y1);
					BotLeft = new Vector2(x1, y2);
					BotRight = new Vector2(x2, y2);
				}
			}
		}
	}
}