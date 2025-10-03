using System;


public class Hotbar : IHotbar, ILocatorInitalizer
{
	private IItemInventory inventory;

	public event Action<ItemID, int> OnSetHotbarItem;
	public event Action<int> OnRemoveHotbarItem;
	public event Action<int> OnSelectItem;

	private ItemID[] slotItemID;

	public int SelectItemIdx { get; private set; }

	void ILocatorInitalizer.Initalize()
	{
		inventory = Locator.Resolve<IItemInventory>();
		slotItemID = new ItemID[ItemExtension.HOTBAR_SLOT_COUNT];
		for(int i = 0; i < ItemExtension.HOTBAR_SLOT_COUNT;i++)
		{
			slotItemID[i] = ItemID.Null;
		}
	}

	public void SetItem(ItemID id, int idx)
	{
		if(slotItemID[idx] == id)
		{
			OnSetHotbarItem?.Invoke(id, idx);
			return;
		}
		for(int i = 0; i < slotItemID.Length; i++)
		{
			if(slotItemID[i] == id)
			{
				RemoveItem(i);
				break;
			}
		}
		slotItemID[idx] = id;
		OnSetHotbarItem?.Invoke(id, idx);
	}

	public void AddItem(ItemID id)
	{
		for(int i = 0; i < ItemExtension.HOTBAR_SLOT_COUNT;i++)
		{
			if(slotItemID[i] == ItemID.Null)
			{
				SetItem(id, i);
				return;
			}
		}
	}

	public void RemoveItem(int idx)
	{
		slotItemID[idx] = ItemID.Null;
		OnRemoveHotbarItem?.Invoke(idx);
	}

	public ItemID GetHotbarItem(int idx)
	{
		return slotItemID[idx];
	}

	public int GetItemAmount(int idx)
	{
		return inventory.GetItemAmount(slotItemID[idx]);
	}

	public bool CanAddItem()
	{
		return ContainsItem(ItemID.Null);
	}

	public void UpdateSlot(int idx)
	{
		if(!inventory.ContainsItem(slotItemID[idx], 1))
		{
			SetItem(ItemID.Null, idx);
		}
		SetItem(slotItemID[idx], idx);
	}

	public void UpdateSlot(ItemID id)
	{
		for(int i = 0; i < ItemExtension.HOTBAR_SLOT_COUNT; i++)
		{
			if(slotItemID[i] == id)
			{
				UpdateSlot(i);
				return;
			}
		}
	}

	public void UpdateAllSlot()
	{
		for(int i = 0; i < ItemExtension.HOTBAR_SLOT_COUNT;i++)
		{
			UpdateSlot(i);
		}
	}

	public bool ContainsItem(ItemID id)
	{
		for (int i = 0; i < ItemExtension.HOTBAR_SLOT_COUNT; i++)
		{
			if (slotItemID[i] == id)
			{
				return true;
			}
		}
		return false;
	}

	public void SelectItem(int idx)
	{
		if (SelectItemIdx == idx) return;
		SelectItemIdx = idx;
		OnSelectItem?.Invoke(idx);
	}

	public void NextItemSelect()
	{
		if(SelectItemIdx + 1 >= ItemExtension.HOTBAR_SLOT_COUNT)
		{
			SelectItem(0);
		}
		else
		{
			SelectItem(SelectItemIdx + 1);
		}
	}

	public void PrevItemSelect()
	{
		if (SelectItemIdx - 1 < 0)
		{
			SelectItem(ItemExtension.HOTBAR_SLOT_COUNT - 1);
		}
		else
		{
			SelectItem(SelectItemIdx - 1);
		}
	}

	public ItemID GetSelectedItem()
	{
		return slotItemID[SelectItemIdx];
	}
}
