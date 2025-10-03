using System.Collections.Generic;
using UnityEngine;


public class ItemInventory : IItemInventory, ILocatorInitalizer
{
	private IItemDataContainer itemDataContainer;
	private IDroppedItemManager droppedItemManager;
	private IPlayerInfo playerInfo;
	private IHotbar hotbar;

	private Dictionary<ItemID, int> inventory = new Dictionary<ItemID, int>();

	void ILocatorInitalizer.Initalize()
	{
		itemDataContainer = Locator.Resolve<IItemDataContainer>();
		droppedItemManager = Locator.Resolve<IDroppedItemManager>();
		playerInfo = Locator.Resolve<IPlayerInfo>();
		hotbar = Locator.Resolve<IHotbar>();
	}

	public void AddItem(ItemID id, int amount)
	{
		if (inventory.ContainsKey(id))
		{
			inventory[id] += amount;
		}
		else
		{
			inventory.Add(id, amount);
		}

		//所持数が最大を超えた場合、超えた分のアイテムを地面にドロップする
		int maxStack = itemDataContainer.GetData(id).MaxStack;
		if (maxStack < inventory[id])
		{
			droppedItemManager.Create(id, inventory[id] - maxStack, playerInfo.Position, 0);
			inventory[id] = maxStack;
		}

		//ホットバーを更新
		if(hotbar.ContainsItem(id))
		{
			hotbar.UpdateSlot(id);
		}
		else
		if (hotbar.CanAddItem())
		{
			hotbar.AddItem(id);
		}
	}

	public void ReduceItem(ItemID id, int amount)
	{
		if (inventory[id] - amount >= 0)
		{
			inventory[id] -= amount;
		}
		else
		{
			Debug.LogError("アイテムID" + id + "を" + amount + "消費すると所持数がマイナスになります");
		}

		//ホットバーを更新
		if (hotbar.ContainsItem(id))
		{
			hotbar.UpdateSlot(id);
		}
	}

	public bool ContainsItem(ItemID id, int amount)
	{
		return inventory.ContainsKey(id) && inventory[id] >= amount;
	}

	public int GetItemAmount(ItemID id)
	{
		if(!inventory.ContainsKey(id))
		{
			return 0;
		}
		return inventory[id];
	}

	public IDictionary<ItemID, int> GetAllItems()
	{
		return inventory;
	}

	public int GetMaxStack(ItemID id)
	{
		return itemDataContainer.GetData(id).MaxStack;
	}
}
