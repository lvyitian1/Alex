using Alex.Worlds;

namespace Alex.Entities.Passive
{
	public class SkeletonHorse : AbstractHorse
	{
		public SkeletonHorse(World level) : base((EntityType)26, level)
		{
			Height = 1.6;
			Width = 1.396484;
		}
	}
}
