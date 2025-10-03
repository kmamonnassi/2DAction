using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGateTile : MonoBehaviour, IPrefabTile, IGimmickActivatable
{
	[SerializeField] private EntityID id = EntityID.Gokiburi;
	[SerializeField] private float interval = 2f;
	[SerializeField] private int halfSize = 4;
	[SerializeField] private int maxSummon = 5;
	[SerializeField] private int canSummonTileDistance = 20;

	private ISignalManager signalManager;
	private IGimmickActivater gimmickActivater;
	private EntityDataContainer entityDataContainer;
	private IMap map;
	private IPlayerInfo playerInfo;

	private Vector2Int position;
	private List<Entity> entities = new List<Entity>();

	public void OnPlaced(Vector2Int position, Direction dir)
	{
		signalManager = Locator.Resolve<ISignalManager>();
		gimmickActivater = Locator.Resolve<IGimmickActivater>();
		entityDataContainer = Locator.Resolve<EntityDataContainer>();
		map = Locator.Resolve<IMap>();
		playerInfo = Locator.Resolve<IPlayerInfo>();

		this.position = position;

		signalManager.OnReceived += OnSignalReceived;
		signalManager.OnCancelReceived += OnCancelSignalReceived;

		if (!signalManager.IsReceived(position))
		{
			OnCancelSignalReceived(position);
		}
	}

	public void OnRemoved()
	{
		signalManager.OnReceived -= OnSignalReceived;
		signalManager.OnCancelReceived -= OnCancelSignalReceived;
		gimmickActivater.RemoveActivatable(interval, this);
	}

	public void OnSetDirection(Direction dir)
	{
	}

	private void OnSignalReceived(Vector2Int pos)
	{
		if (position != pos) return;
		gimmickActivater.RemoveActivatable(interval, this);
	}

	private void OnCancelSignalReceived(Vector2Int pos)
	{
		if (position != pos) return;
		gimmickActivater.AddActivatable(interval, this);
	}

	public void OnActive()
	{
		//プレイヤーが近くにいないと召喚できない
		if (Vector2.Distance(playerInfo.Position, transform.position) > canSummonTileDistance * MapExtension.TILE_SIZE)
		{
			return;
		}

		//召喚上限
		if (entities.Count >= maxSummon)
		{
			return;
		}

		Vector2Int minPos = position - new Vector2Int(halfSize, halfSize);
		Vector2Int maxPos = position + new Vector2Int(halfSize, halfSize);
		
		//10回召喚しようとしてできなければスキップ
		for(int i = 0; i < 10;i++)
		{
			int x = Random.Range(minPos.x, maxPos.x);
			int y = Random.Range(minPos.y, maxPos.y);
			Vector2Int summonPos = new Vector2Int(x, y);
			if (MapExtension.IsInsideMap(summonPos))
			{
				WallData wallData = map.GetWallData(summonPos);
				if (wallData.ID == TileID.NullWall || !wallData.IsCollider)
				{
					Summon(summonPos);
					break;
				}
			}
		}
	}

	private void Summon(Vector2Int position)
	{
		Vector2 summonPos = position * MapExtension.TILE_SIZE;
		Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
		Entity entity = Instantiate(entityDataContainer.GetEntityData(id).EntityPrefab, summonPos, rotation);
		entities.Add(entity);
		entity.OnDestroyed += () => entities.Remove(entity);
	}
}
