using UnityEngine;

public interface IUseItemAction
{
	void OnUseStart(Vector2 position, ItemID id, UseItemData data);
	void OnUsing(Vector2 position, ItemID id, UseItemData data);
	void OnUseEnd(Vector2 position, ItemID id, UseItemData data);
}