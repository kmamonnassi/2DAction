using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class EntityShadowCaster : MonoBehaviour, IUpdate
{
	[SerializeField] private SpriteRenderer shadowPrefab;
	[SerializeField] private Vector3 offset = new Vector3(1, -1, 1);
	private IUpdater updater;

	private SpriteRenderer rend;
	private SpriteRenderer shadow;

	private void Start()
	{
		updater = Locator.Resolve<IUpdater>(UpdaterID.ENTITY);
		rend = GetComponent<SpriteRenderer>();
		shadow = Instantiate(shadowPrefab, rend.transform.position + offset, rend.transform.rotation, null);
		shadow.sprite = rend.sprite;
		shadow.transform.localScale = rend.transform.localScale;
		shadow.sortingLayerID = rend.sortingLayerID;
		updater.AddUpdate(this);
	}

	private void OnEnable()
	{
		if (shadow == null) return;
		shadow.gameObject.SetActive(true);
	}

	private void OnDisable()
	{
		if (shadow == null) return;
		shadow.gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		if (shadow == null) return;
		updater.RemoveUpdate(this);
		Destroy(shadow.gameObject);
	}

	void IUpdate.OnUpdate()
	{
		shadow.sprite = rend.sprite;
		shadow.transform.position = rend.transform.position + offset;
		shadow.transform.rotation = rend.transform.rotation;
		shadow.transform.localScale = rend.transform.localScale;
	}
}
