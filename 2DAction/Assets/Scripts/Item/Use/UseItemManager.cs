using UnityEngine;


public class UseItemManager : MonoBehaviour, IUpdate
{
	private IUpdater updater;
	private IHotbar hotbar;
	private IPlayerInfo playerInfo;
	private IMap map;
	private IItemInventory itemInventory;

	private UseItemActionContainer actionContainer;
	private UseItemData useItemData;
	private ItemID nowUsingItemID;
	private IUseItemAction nowUsingItemAction;

	private void Start()
	{
		updater = Locator.Resolve<IUpdater>(UpdaterID.UI);
		hotbar = Locator.Resolve<IHotbar>();
		playerInfo = Locator.Resolve<IPlayerInfo>();
		map = Locator.Resolve<IMap>();
		itemInventory = Locator.Resolve<IItemInventory>();

		actionContainer = new UseItemActionContainer();
		useItemData = new UseItemData(playerInfo, map, itemInventory);
		updater.AddUpdate(this);
	}

	public void OnUpdate()
	{
		if(hotbar.GetSelectedItem() == ItemID.Null)
		{
			return;
		}

		if(Input.GetMouseButtonDown(1))
		{
			nowUsingItemID = hotbar.GetSelectedItem();
			nowUsingItemAction = actionContainer.GetAction(nowUsingItemID);

			nowUsingItemAction.OnUseStart(InputEX.WorldMousePosition(), nowUsingItemID, useItemData);
		}
		else if(Input.GetMouseButton(1))
		{
			nowUsingItemAction.OnUsing(InputEX.WorldMousePosition(), nowUsingItemID, useItemData);
		} 
		else if (Input.GetMouseButtonUp(1))
		{
			nowUsingItemAction.OnUseEnd(InputEX.WorldMousePosition(), nowUsingItemID, useItemData);
			nowUsingItemID = ItemID.Null;
			nowUsingItemAction = null;
		}
	}

	private void OnDestroy()
	{
		updater.RemoveUpdate(this);
	}
}
