using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Alex.API.Graphics;
using Alex.API.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NLog;

namespace Alex.Worlds.Chunks
{
	public struct VertexData
	{
		public Vector3 Position;

		public ushort TexCoords;

		public uint Color;

		public byte BlockLight;

		public byte SkyLight;

		public VertexData(Vector3 position, ushort textureCoordinates, uint color, byte blockLight, byte skyLight)
		{
			Position = position;
			TexCoords = textureCoordinates;
			Color = color;
			BlockLight = blockLight;
			SkyLight = skyLight;
		}
	}
	public class ChunkRenderStage : IDisposable
	{
		private static ILogger Log         = LogManager.GetCurrentClassLogger();
		
		private const  int     DefaultSize = 64;

		private static SmartStorage<Vector2>                      TextureStorage { get; } = new SmartStorage<Vector2>();
		//private static SmartStorage<Color>                        ColorStorage { get; } = new SmartStorage<Color>();
		
		//private List<(ushort, ushort)>                         AvailableIndices { get; }
		private Dictionary<BlockCoordinates, List<VertexData>> BlockIndices     { get; set; }
		private PooledVertexBuffer                             Buffer           { get; set; }
		//private VertexData?[]                                  Vertices         { get; set; }

		//private List<Vector3> Positions          { get; set; } = new List<Vector3>();

		private bool                  HasChanges     { get; set; }
		private bool                  HasResized     { get; set; } = false;

		private ChunkData Parent { get; }
		private object    _writeLock   = new object();
		private long      _vertexCount = 0;
		public ChunkRenderStage(ChunkData parent)
		{
			Parent = parent;
			
			//TextureStorage = new SmartStorage<Vector2>();
			BlockIndices = new Dictionary<BlockCoordinates, List<VertexData>>();
			//AvailableIndices = new List<(ushort, ushort)>();// new LinkedList<KeyValuePair<int, int>>();
		}

		public void AddVertex(BlockCoordinates blockCoordinates, 
			Vector3 position,
			Vector2 textureCoordinates,
			Color color,
			byte blockLight,
			byte skyLight)
		{
			lock (_writeLock)
			{
				//Add(blockCoordinates, position, textureCoordinates, color, blockLight, skyLight);
				var textureIndex = TextureStorage.GetIndex(textureCoordinates);

				if (textureIndex == -1)
				{
					textureIndex = TextureStorage.Add(textureCoordinates);
				}

				TextureStorage.IncreaseUsage(textureIndex);

				var vertexData = new VertexData(
					position, (ushort) textureIndex, color.PackedValue, (byte) blockLight,
					skyLight);
				
				Interlocked.Increment(ref _vertexCount);
			
				if (BlockIndices.TryGetValue(blockCoordinates, out var list))
				{
					list.Add(vertexData);
				}
				else
				{
					BlockIndices.Add(blockCoordinates, new List<VertexData>()
					{
						vertexData
					});
				}
				
				HasChanges = true;
			}
		}

		public void Remove(BlockCoordinates coordinates)
		{
			lock (_writeLock)
			{
				if (BlockIndices.Remove(coordinates, out var indices))
				{
					foreach (var vertex in indices)
					{
						TextureStorage.DecrementUsage(vertex.TexCoords);
						Interlocked.Decrement(ref _vertexCount);
						//FreeIndex(index);
					}

					// ApplyIntermediate();

					HasChanges = true;
				}
			}
		}

		public bool Contains(BlockCoordinates coordinates)
		{
			lock (_writeLock)
			{
				return BlockIndices.ContainsKey(coordinates);
			}
		}

		internal MinifiedBlockShaderVertex[] BuildVertices()
		{
			lock (_writeLock)
			{
				var realVertices = new List<MinifiedBlockShaderVertex>((int) _vertexCount);

				foreach (var block in BlockIndices)
				{
					foreach (var vertex in block.Value)
					{
						realVertices.Add(
							new MinifiedBlockShaderVertex(
								vertex.Position, TextureStorage[vertex.TexCoords], new Color(vertex.Color))
							{
								SkyLight = vertex.SkyLight, BlockLight = vertex.BlockLight
							});
					}
				}

				return realVertices.ToArray();
			}
		}

		private int NextPowerOf2(int x)
		{
			double nextnum = Math.Ceiling(Math.Log2(x));
			var    result  = Math.Pow(2, nextnum);

			return (int) result;
		}

		private bool _previousKeepInMemory     = false;
		private int  _renderableVerticeCount = 0;
		public void Apply(GraphicsDevice device = null, bool keepInMemory = true)
		{
			lock (_writeLock)
			{
				if (!HasChanges && _previousKeepInMemory)
					return;

				_previousKeepInMemory = keepInMemory;

				var realVertices = BuildVertices();

				if (realVertices.Length == 0)
				{
					_renderableVerticeCount = 0;
					return;
				}

				var verticeCount = realVertices.Length;
				while (verticeCount % 3 != 0) //Make sure we have a valid triangle list.
				{
					verticeCount--;
				}
				
				_renderableVerticeCount = verticeCount;

				bool               callSetData = HasResized;
				PooledVertexBuffer oldBuffer   = null;
				PooledVertexBuffer buffer      = Buffer;

				if (buffer != null && buffer.VertexCount - realVertices.Length >= 256)
				{
					if (GpuResourceManager.TryGetRecycledBuffer(
						this, device, MinifiedBlockShaderVertex.VertexDeclaration, realVertices.Length,
						BufferUsage.WriteOnly, out var b))
					{
						oldBuffer = buffer;
						buffer = b;

						callSetData = true;
					}
				}
				
				if (buffer == null || buffer.VertexCount < realVertices.Length)
				{
					oldBuffer = buffer;

					buffer = GpuResourceManager.GetBuffer(
						this, device, MinifiedBlockShaderVertex.VertexDeclaration, realVertices.Length, BufferUsage.WriteOnly);

					callSetData = true;
				}

				//if (callSetData)
					buffer.SetData(realVertices, 0, realVertices.Length);

				Buffer = buffer;
				oldBuffer?.MarkForDisposal();
            
				HasResized = false;
				HasChanges = false;
			}
		}
        
		public virtual void Render(GraphicsDevice device, Effect effect)
		{
			var primitives = _renderableVerticeCount;

			if (Buffer == null || primitives == 0) return;
            
			device.SetVertexBuffer(Buffer);
			foreach (var pass in effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				device.DrawPrimitives(PrimitiveType.TriangleList, 0, primitives / 3);
			}
		}

		public void Dispose()
		{
			Buffer?.MarkForDisposal();
			BlockIndices.Clear();
		}
	}
}