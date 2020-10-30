﻿namespace Alex.Blocks.Minecraft
{
	public class FlowingWater : Block
	{
		public FlowingWater(byte meta = 0) : base()
		{
			Solid = false;
			Transparent = true;

			LightOpacity = 2;
			BlockMaterial = Material.Water;
			//BlockModel = new LiquidBlockModel()
			//{
			//	IsFlowing = true,
			//	Level = meta
			//};
		}
	}
}