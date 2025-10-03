using UnityEngine;

public class DropItemByPlayer : ILocatorInitalizer, IUpdate
{
	private const float DROP_ITEM_INVINCIBLE_DURATION = 0.5f;
	private const float DROP_ITEM_MOVE_POWER = 60.0f;
	private const float DROP_ITEM_MOVE_DURATION = 0.15f;

	private IPlayerInfo playerInfo;
	private IHotbar hotbar;
	private IItemInventory inventory;
	private IDroppedItemManager droppedItemManager;
	private IUpdater updater;

	void ILocatorInitalizer.Initalize()
	{
		playerInfo = Locator.Resolve<IPlayerInfo>();
		hotbar = Locator.Resolve<IHotbar>();
		inventory = Locator.Resolve<IItemInventory>();
		droppedItemManager = Locator.Resolve<IDroppedItemManager>();
		updater = Locator.Resolve<IUpdater>();
		updater.AddUpdate(this);
	}

	public void OnUpdate()
	{
		if (playerInfo.Creature.IsDead) return;

		if(Input.GetKeyDown(KeyCode.Q))
		{
			ItemID id = hotbar.GetSelectedItem();
			if (id == ItemID.Null) return;
			inventory.ReduceItem(id, 1);
			DroppedItem item = droppedItemManager.Create(id, 1, playerInfo.Position, DROP_ITEM_INVINCIBLE_DURATION);
			Entity entity = item.GetComponent<Entity>();
			Vector2 dir = (InputEX.WorldMousePosition() - (Vector2)playerInfo.Position).normalized;
			entity.MoveEntity(dir, DROP_ITEM_MOVE_POWER, DROP_ITEM_MOVE_DURATION);
		}
	}
}
