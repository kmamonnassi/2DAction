using UnityEngine;

public interface IDroppedItemManager
{
	DroppedItem Create(ItemID id, int amount, Vector2 position, float invincibleDuration);
}