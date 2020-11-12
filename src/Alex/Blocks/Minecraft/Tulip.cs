namespace Alex.Blocks.Minecraft
{
    public class Tulip : FlowerBase
    {
        public Tulip()
        {
            Solid = false;
            Transparent = true;

            IsFullBlock = false;
            IsFullCube = false;

            BlockMaterial = Material.Plants;
        }
    }
}