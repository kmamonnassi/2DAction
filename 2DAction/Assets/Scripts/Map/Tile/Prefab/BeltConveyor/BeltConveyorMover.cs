using System.Collections.Generic;
using UnityEngine;


public class BeltConveyorMover : IBeltConveyorMover, IFixedUpdate, ILocatorInitalizer
{
	private const float MOVE_SPEED = 40;

	private IMap map;
	private IUpdater updater;
	private Dictionary<Direction, List<Entity>> entities = new Dictionary<Direction, List<Entity>>();

	void ILocatorInitalizer.Initalize()
	{
		entities.Add(Direction.Up, new List<Entity>());
		entities.Add(Direction.Left, new List<Entity>());
		entities.Add(Direction.Down, new List<Entity>());
		entities.Add(Direction.Right, new List<Entity>());

		map = Locator.Resolve<IMap>();
		updater = Locator.Resolve<IUpdater>(UpdaterID.MAP);
		updater.AddFixedUpdate(this);
	}

	public void AddMoveEntity(Entity entity, Direction direction)
	{
		entities[direction].Add(entity);
	}

	public void RemoveMoveEntity(Entity entity, Direction direction)
	{
		entities[direction].Remove(entity);
	}

	public void OnFixedUpdate()
	{
		for(int i = 0; i < 4;i++)
		{
			List<Entity> movedEntity = new List<Entity>();
			foreach (Entity entity in entities[(Direction)i])
			{
				if (movedEntity.Contains(entity)) continue;

				Vector2Int position = entity.transform.position.WorldToTilePosition();
				TileID id = map.GetWallID(position);
				if (id == TileID.BeltConveyor)
				{
					Direction dir = map.GetWallDirection(position);
					entity.Move(dir.GetDirVec2Int(), MOVE_SPEED);
					movedEntity.Add(entity);
					entity.OnDestroyed += () => entities[dir].Remove(entity);
				}
			}
		}
	}
}
