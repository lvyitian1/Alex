namespace Alex.Blocks.Minecraft
{
	public class EndGateway : Block
	{
		public EndGateway() : base()
		{
			Solid = true;
			Transparent = false;
			IsReplacible = false;
			LightValue = 15;
			
			Hardness = -1;
			
			BlockMaterial = Material.Portal;
		}
	}
}
