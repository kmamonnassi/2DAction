using System.Collections.Generic;

public interface IItemInventory
{
	void AddItem(ItemID id, int amount);
	void ReduceItem(ItemID id, int amount);
	int GetMaxStack(ItemID id);
	bool ContainsItem(ItemID id, int amount);
	int GetItemAmount(ItemID id);
	IDictionary<ItemID, int> GetAllItems();
}
