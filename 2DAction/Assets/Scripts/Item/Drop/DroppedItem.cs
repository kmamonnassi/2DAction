using UnityEngine;

public class DroppedItem : MonoBehaviour, IUpdate
{
	[SerializeField] private SpriteRenderer icon;
	[SerializeField] private SpriteRenderer shadow;

	private IItemInventory itemInventory;
	private IUpdater updater;

	private ItemID id;
	private int amount;
	private float invincibleDuration;

	public void Setup(ItemData data, int amount, float invincibleDuration)
	{
		itemInventory = Locator.Resolve<IItemInventory>();
		updater = Locator.Resolve<IUpdater>(UpdaterID.ENTITY);
		icon.sprite = data.Icon;
		shadow.sprite = data.Icon;
		this.id = data.ID;
		this.amount = amount;
		this.invincibleDuration = invincibleDuration;
		if(invincibleDuration > 0)
		{
			updater.AddUpdateSafe(this);
		}

		gameObject.SetActive(true);
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (invincibleDuration > 0) return;

		if(col.gameObject.CompareTag(TagNames.PLAYER))
		{
			Pickup();
		}
	}

	public void OnUpdate()
	{
		invincibleDuration -= Time.deltaTime;
		if (invincibleDuration <= 0)
		{
			updater.RemoveUpdateSafe(this);
		}
	}

	private void Pickup()
	{
		itemInventory.AddItem(id, amount);
		Destroy(gameObject);
	}
}