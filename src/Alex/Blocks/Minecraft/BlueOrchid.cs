namespace Alex.Blocks.Minecraft
{
	public class BlueOrchid : FlowerBase
	{
		public BlueOrchid()
		{
			Solid = false;
			Transparent = true;

			IsFullBlock = false;
			IsFullCube = false;

			BlockMaterial = Material.Plants;
		}
	}
}