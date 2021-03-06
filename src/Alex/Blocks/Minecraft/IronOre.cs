using Alex.API.Utils;

namespace Alex.Blocks.Minecraft
{
	public class IronOre : Block
	{
		public IronOre() : base()
		{
			Solid = true;
			Transparent = false;
			IsReplacible = false;

			BlockMaterial = Material.Ore.Clone().SetRequiredTool(ItemType.PickAxe, ItemMaterial.Stone);
		}
	}
}
