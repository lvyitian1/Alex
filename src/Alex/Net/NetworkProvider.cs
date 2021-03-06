using Alex.API;
using Alex.API.Network;
using Alex.API.Utils;
using Alex.Entities;
using Alex.Items;
using Microsoft.Xna.Framework;
using MiNET;
using MiNET.Net;
using Player = Alex.Entities.Player;


namespace Alex.Net
{
	public enum ItemUseOnEntityAction
	{
		Interact,
		Attack,
		ItemInteract,
	}
	
	public abstract class NetworkProvider
	{
		public abstract bool IsConnected { get; }
		public abstract ConnectionInfo GetConnectionInfo();

		public abstract void PlayerOnGroundChanged(Player player, bool onGround);
		
		public abstract void EntityFell(long entityId, float distance, bool inVoid);
		
		public abstract void EntityAction(int entityId, EntityAction action);

		public abstract void PlayerAnimate(PlayerAnimations animation);

		public abstract void BlockPlaced(BlockCoordinates position,
			API.Blocks.BlockFace face,
			int hand,
			int slot,
			Vector3 cursorPosition,
			Entity p);
		public abstract void PlayerDigging(DiggingStatus status, BlockCoordinates position, API.Blocks.BlockFace face, Vector3 cursorPosition);

		public abstract void EntityInteraction(Entity player, Entity target,
			ItemUseOnEntityAction action, int hand, int slot);

		public abstract void WorldInteraction(Entity entity,
			BlockCoordinates position,
			API.Blocks.BlockFace face,
			int hand,
			int slot,
			Vector3 cursorPosition);
		public abstract void UseItem(Item item, int hand, ItemUseAction action, BlockCoordinates position, API.Blocks.BlockFace face, Vector3 cursorPosition);
		public abstract void HeldItemChanged(Item item, short slot);
		public abstract void Close();

		public abstract void SendChatMessage(ChatObject message);
	}
}