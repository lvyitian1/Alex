namespace Alex.Blocks.Minecraft
{
	public class PistonHead : Block
	{
		public PistonHead() : base()
		{
			Solid = true;
			Transparent = true;
			IsReplacible = false;
			
			BlockMaterial = Material.Piston;
			Hardness = 0.5f;
		}
	}
}
