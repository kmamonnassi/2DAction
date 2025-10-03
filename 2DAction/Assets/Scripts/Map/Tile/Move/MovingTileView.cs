using DG.Tweening;
using System;
using UnityEngine;

public class MovingTileView : MonoBehaviour, IUpdate
{
	[SerializeField] private SpriteRenderer rend;
	[SerializeField] private float rotateSpeed = 90;

	private IUpdater updater;
	private IMap map;
	private Direction dir;
	private Direction tileDir;
	private float speed;
	private Vector2Int startTilePos;
	private Vector2Int nowTilePos;
	private Vector2Int goalTilePos;
	private Action<Vector2Int, Direction> onEndAct;

	public void SetTile(Sprite sprite)
	{
		rend.sprite = sprite;
	}

	public void Move(Vector2Int start, Vector2Int goal, float speed, Action<Vector2Int, Direction> onEndAct)
	{
		updater = Locator.Resolve<IUpdater>(UpdaterID.MAP);
		map = Locator.Resolve<IMap>();

		this.speed = speed;
		this.onEndAct = onEndAct;
		startTilePos = start;
		goalTilePos = goal;

		int dirX = goal.x - start.x;
		int dirY = goal.y - start.y;
		
		if(dirX > 0)
		{
			dir = Direction.Right;
		}
		else if (dirX < 0)
		{
			dir = Direction.Left;
		}
		else if (dirY > 0)
		{
			dir = Direction.Up;
		}
		else if (dirY < 0)
		{
			dir = Direction.Down;
		}
		tileDir = dir;

		transform.position = (Vector2)startTilePos * MapExtension.TILE_SIZE + new Vector2(MapExtension.TILE_SIZE, MapExtension.TILE_SIZE) / 2;
		transform.eulerAngles = new Vector3(0, 0, dir.ToRotationZ());
		updater.AddUpdateSafe(this);
	}

	public void OnUpdate()
	{
		float moveSpeed = Time.deltaTime * MapExtension.TILE_SIZE;

		transform.position += moveSpeed * (Vector3)dir.GetDirVec3Int() * speed;
		transform.eulerAngles += new Vector3(0, 0, rotateSpeed * Time.deltaTime * speed);

		Vector2Int _nowTilePos = Vector2Extension.WorldToTilePosition(transform.position - new Vector3(MapExtension.TILE_SIZE, MapExtension.TILE_SIZE) / 2);

		if(nowTilePos != _nowTilePos)
		{
			nowTilePos = _nowTilePos;
			tileDir = AddTileDir(tileDir);

			if (map.GetWallID(nowTilePos + dir.GetDirVec2Int()) != TileID.NullWall)
			{
				updater.RemoveUpdateSafe(this);
				onEndAct?.Invoke(nowTilePos, tileDir);
				Destroy(gameObject);
			}
			else
			if (nowTilePos == goalTilePos)
			{
				updater.RemoveUpdateSafe(this);
				onEndAct?.Invoke(nowTilePos, tileDir);
				Destroy(gameObject);
			}
		}
	}

	private Direction AddTileDir(Direction dir)
	{
		switch(dir)
		{
			case Direction.Up:
				return Direction.Right;
			case Direction.Right:
				return Direction.Down;
			case Direction.Down:
				return Direction.Left;
			case Direction.Left:
				return Direction.Up;
		}
		return Direction.Down;
	}
}