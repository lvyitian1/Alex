using Alex.API.Blocks;
using Alex.API.Utils;
using Alex.API.World;
using Alex.Blocks.State;
using Alex.Worlds;
using Alex.Worlds.Abstraction;
using Microsoft.Xna.Framework;

namespace Alex.Blocks.Minecraft
{
	public class Fence : Block
	{
		public Fence()
		{
			Transparent = true;
			Solid = true;
			IsFullCube = false;
			RequiresUpdate = true;
		}

		/// <inheritdoc />
		public override BlockState BlockPlaced(IBlockAccess world, BlockState state, BlockCoordinates position)
		{/*
			var current = BlockState;
			current = Check(world, position, position + BlockCoordinates.North, current);
			current = Check(world, position, position + BlockCoordinates.East, current);
			current = Check(world, position, position + BlockCoordinates.South, current);
			current = Check(world, position, position + BlockCoordinates.West, current);
			current = Check(world, position, position + BlockCoordinates.Up, current);
			current = Check(world, position, position + BlockCoordinates.Down, current);
			return current;*/
			return base.BlockPlaced(world, state, position);
		}

		/// <inheritdoc />
		public override void BlockUpdate(World world, BlockCoordinates position, BlockCoordinates updatedBlock)
		{
			var state = Check(world, position, updatedBlock, BlockState);
			if (state != BlockState)
				world.SetBlockState(position, state);
			
			//base.BlockUpdate(world, position, updatedBlock);
		}

		private BlockState Check(IBlockAccess world, BlockCoordinates position, BlockCoordinates updatedBlock, BlockState current)
		{
			var neighbor = world.GetBlockState(updatedBlock);
			
			var facePos = updatedBlock - position;
			var fp      = new Vector3(facePos.X, facePos.Y, facePos.Z);
			fp.Normalize();
			
			var face       = new Vector3(fp.X, fp.Y, fp.Z).GetBlockFace();
			var faceString = face.ToString().ToLower();
			
			current.TryGetValue(faceString, out var currentValue);
			
			if (CanAttach(face, neighbor.Block))
			{
				if (currentValue != "true")
				{
					return current.WithProperty(faceString, "true");
					//world.SetBlockState(position, state);
				}
			}
			else 
			{
				if (currentValue != "false")
				{
					return current.WithProperty(faceString, "false");
					//world.SetBlockState(position, state);
				}
			}

			return current;
		}

		/// <inheritdoc />
		public override bool CanAttach(BlockFace face, Block block)
		{
			if (block is Fence || block is FenceGate)
				return true;
			
			return base.CanAttach(face, block);
		}
	}
}