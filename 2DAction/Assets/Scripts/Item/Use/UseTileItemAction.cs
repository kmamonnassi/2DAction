using UnityEngine;

public class UseTileItemAction : IUseItemAction
{
	private float usingTime;

	private const float FIRST_USE_INTERVAL = 0.25f;
	private const float USE_INTERVAL = 0.1f;
	private const float CAN_USE_DISTANCE = 3;

	private bool useFirst = false;

	public void OnUseStart(Vector2 position, ItemID id, UseItemData data)
	{
		PutTile(position, id, data);
	}

	public void OnUsing(Vector2 position, ItemID id, UseItemData data)
	{
		usingTime += Time.deltaTime;
		if(!useFirst)
		{
			if (usingTime > FIRST_USE_INTERVAL)
			{
				usingTime = 0;
				useFirst = true;
				PutTile(position, id, data);
			}
		}
		else
		{
			if (usingTime > USE_INTERVAL)
			{
				usingTime = 0;
				PutTile(position, id, data);
			}
		}
	}

	public void OnUseEnd(Vector2 position, ItemID id, UseItemData data)
	{
		usingTime = 0;
		useFirst = false;
	}

	private void PutTile(Vector2 position, ItemID id, UseItemData data)
	{
		TileID tileID = (TileID)id;

		if (Vector2.Distance(data.PlayerInfo.Position, position) / MapExtension.TILE_SIZE > CAN_USE_DISTANCE)
		{
			return;
		}
		
		Vector2Int tilePos = new Vector2Int(Mathf.FloorToInt(position.x / MapExtension.TILE_SIZE), Mathf.FloorToInt(position.y / MapExtension.TILE_SIZE));

		if (tilePos.x < 0 || tilePos.y < 0 || tilePos.x >= MapExtension.WORLD_WIDTH || tilePos.y >= MapExtension.WORLD_HEIGHT)
		{
			return;
		}

		if (data.Map.GetTileData(tileID.GetTileType(), tilePos).ID.IsNull())
		{
			Direction dir;
			Vector2 vec = ((Vector2)data.PlayerInfo.Position - position);
			if (Mathf.Abs(vec.x) > Mathf.Abs(vec.y))
			{
				if (vec.x > 0)
				{
					dir = Direction.Right;
				}
				else
				{
					dir = Direction.Left;
				}
			}
			else
			{
				if (vec.y > 0)
				{
					dir = Direction.Up;
				}
				else
				{
					dir = Direction.Down;
				}
			}

			data.Map.SetTile((TileID)id, tilePos, dir);
			data.ItemInventory.ReduceItem(id, 1);
		}
	}
}
