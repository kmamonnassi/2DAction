using UnityEngine;


public class EntityMoveTowardsPlayer : MonoBehaviour, IFixedUpdate
{
	[SerializeField] private Entity entity;
	[SerializeField] private float moveSpeed = 50;
	[SerializeField] private float noticeDistance = 160;
	[SerializeField] private float lostDistance = 320;

	private IPlayerInfo playerInfo;
	private IUpdater updater;

	private bool isNotice = false;

	private void Start()
	{
		playerInfo = Locator.Resolve<IPlayerInfo>();
		updater = Locator.Resolve<IUpdater>(UpdaterID.ENTITY);
		updater.AddFixedUpdate(this);
	}

	void IFixedUpdate.OnFixedUpdate()
	{
		if(isNotice)
		{
			Vector2 dir = ((Vector2)playerInfo.Position - (Vector2)transform.position).normalized;
			entity.Move(dir, moveSpeed);
			transform.eulerAngles = new Vector3(0, 0, dir.GetAim());

			if (Vector2.Distance(playerInfo.Position, transform.position) < lostDistance)
			{
				isNotice = false;
			}
		}
		else
		{
			if(Vector2.Distance(playerInfo.Position, transform.position) < noticeDistance)
			{
				isNotice = true;
			}
		}
	}

	private void OnDestroy()
	{
		updater.RemoveFixedUpdate(this);
	}
}
