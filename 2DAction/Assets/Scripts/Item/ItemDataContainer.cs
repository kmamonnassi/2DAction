using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataContainer", menuName = "Container/ItemData")]
public class ItemDataContainer : ScriptableObject, IItemDataContainer
{
	[SerializeField] private List<ItemData> datas;

	public ItemData GetData(ItemID id)
	{
		return datas.Find(x => x.ID == id);
	}
}