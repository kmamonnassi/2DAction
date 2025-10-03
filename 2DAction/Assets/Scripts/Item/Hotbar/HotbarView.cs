using UnityEngine;


public class HotbarView : MonoBehaviour, IUpdate
{
	[SerializeField] private HotbarSlotView[] slots;

	private IUpdater updater;
	private IHotbar hotbar;
	private IItemDataContainer itemDataContainer;

	private int selectIdx;

	private void Start()
	{
		updater = Locator.Resolve<IUpdater>(UpdaterID.UI);
		hotbar = Locator.Resolve<IHotbar>();
		itemDataContainer = Locator.Resolve<IItemDataContainer>();
		hotbar.OnSetHotbarItem += SetItem;
		hotbar.OnRemoveHotbarItem += RemoveItem;
		hotbar.OnSelectItem += SelectItem;
		updater.AddUpdate(this);

		for(int i = 0; i < ItemExtension.HOTBAR_SLOT_COUNT; i++)
		{
			SetItem(hotbar.GetHotbarItem(i), i);
		}
	}

	private void SetItem(ItemID id, int idx)
	{
		ItemData itemData = itemDataContainer.GetData(id);
		if(itemData == null)
		{
			RemoveItem(idx);
		}
		else
		{
			slots[idx].SetItem(itemData.Icon, hotbar.GetItemAmount(idx));
		}
	}

	private void RemoveItem(int idx)
	{
		slots[idx].RemoveItem();
	}

	private void SelectItem(int idx)
	{
		if (idx == selectIdx) return;
		slots[selectIdx].Deselect();
		slots[idx].Select();
		selectIdx = idx;
	}

	public void OnUpdate()
	{
		if (Input.mouseScrollDelta.y > 0)
		{
			hotbar.PrevItemSelect();
		}
		else if (Input.mouseScrollDelta.y < 0)
		{
			hotbar.NextItemSelect();
		}
	}

	private void OnDestroy()
	{
		updater.RemoveUpdate(this);
	}
}