using System.Collections.Generic;
using UnityEngine;


public class PlayerUseDrill : MonoBehaviour, IUpdate
{
    [SerializeField] private PlayerDrillCollider drillCollider;
	[SerializeField] private Animator animator;
	[SerializeField] private Transform[] drillTips;
	private IUpdater updater;
	private IWallDamageManager wallDamage;
	private IPlayerStateManager stateManager;
	private IMap map;

	private void Start()
	{
		updater = Locator.Resolve<IUpdater>(UpdaterID.ENTITY);
		wallDamage = Locator.Resolve<IWallDamageManager>();
		stateManager = Locator.Resolve<IPlayerStateManager>();
		map = Locator.Resolve<IMap>();

		drillCollider.OnEnter += col => 
		{
			if(col.gameObject.CompareTag(TagNames.WALL) || col.gameObject.CompareTag(TagNames.WALL_NON_COLLIDER))
			{
				List<Vector2Int> posList = new List<Vector2Int>();
				for(int i = 0; i < drillTips.Length; i++)
				{
					int x = Mathf.FloorToInt(drillTips[i].transform.position.x / MapExtension.TILE_SIZE);
					int y = Mathf.FloorToInt(drillTips[i].transform.position.y / MapExtension.TILE_SIZE);
					if (x < 0 || y < 0 || x >= MapExtension.WORLD_WIDTH || y >= MapExtension.WORLD_HEIGHT) continue;
					Vector2Int pos = new Vector2Int(x, y);
					if(!posList.Contains(pos))
					{
						posList.Add(pos);
						if(!map.GetWallData(pos).ID.IsNull())
						{
							Vector2 digDir = (drillTips[i].transform.position - transform.position).normalized;
							wallDamage.Dig(pos, 99, 2, digDir);
						}
					}
				}
				
				//drillCollider.ColliderSize
			}
		};

		drillCollider.OnExit += col =>
		{

		};
		updater.AddUpdate(this);
	}

	void IUpdate.OnUpdate()
	{
		if(Input.GetMouseButtonDown(0))
		{
			UseDrill();
		}
		else if(Input.GetMouseButton(0))
		{
			RotateTowardCursor();
		}
		else if(Input.GetMouseButtonUp(0))
		{
			EndUseDrill();
		}
	}

	public void UseDrill()
	{
		stateManager.StartState(PlayerStateType.Drill);
		drillCollider.gameObject.SetActive(true);
		animator.SetBool("UseDrill", true);
		RotateTowardCursor();
	}

	private void RotateTowardCursor()
	{
		Vector2 pos = InputEX.WorldMousePosition();
		float angle = (pos - (Vector2)transform.position).GetAim();
		transform.eulerAngles = new Vector3(0, 0, angle);
	}

	public void EndUseDrill()
	{
		stateManager.EndState(PlayerStateType.Drill);
		drillCollider.gameObject.SetActive(false);
		animator.SetBool("UseDrill", false);
	}

	private void OnDestroy()
	{
		updater.RemoveUpdate(this);
	}
}
