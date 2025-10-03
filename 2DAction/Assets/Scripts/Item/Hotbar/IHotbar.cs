using System;

public interface IHotbar
{
	int SelectItemIdx { get; }

	event Action<ItemID, int> OnSetHotbarItem;
	event Action<int> OnRemoveHotbarItem;
	event Action<int> OnSelectItem;

	void SetItem(ItemID id, int idx);
	void AddItem(ItemID id);
	void RemoveItem(int idx);
	ItemID GetHotbarItem(int idx);
	int GetItemAmount(int idx);
	bool CanAddItem();
	void UpdateSlot(int idx);
	void UpdateSlot(ItemID id);
	void UpdateAllSlot();
	bool ContainsItem(ItemID id);
	void SelectItem(int idx);
	void NextItemSelect();
	void PrevItemSelect();
	ItemID GetSelectedItem();
}