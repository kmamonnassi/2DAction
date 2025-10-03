using UnityEngine;


public class DroppedItemManager : MonoBehaviour, IDroppedItemManager
{
	[SerializeField] private DroppedItem prefab;
	private IItemDataContainer itemDataContainer;

	public DroppedItem Create(ItemID id, int amount, Vector2 position, float invincibleDuration)
	{
		itemDataContainer = Locator.Resolve<IItemDataContainer>();
		ItemData data = itemDataContainer.GetData(id);
		if(data == null)
		{
			Debug.LogError(id + "のアイテムデータは存在しません");
			return null;
		}
		DroppedItem instance = Instantiate(prefab);
		instance.Setup(data, amount, invincibleDuration);
		instance.transform.position = new Vector3(position.x, position.y, instance.transform.position.z);
		return instance;
	}
}