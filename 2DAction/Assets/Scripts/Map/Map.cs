using Cysharp.Threading.Tasks;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Map : MonoBehaviour, IMap, IUpdate
{
    [SerializeField] private Tilemap groundTilemap;
	[SerializeField] private Tilemap wallTilemap;
	[SerializeField] private Tilemap wallNonColTilemap;
	[SerializeField] private Tilemap wallShadowTilemap;
	[SerializeField] private MovingTileView movingTilePrefab;
	private TileDataContainer tileDataContainer;
	private IUpdater updater;

	private int[,] groundTiles;
	private int[,] wallTiles;
	private Direction[,] wallDirections;
	private bool[,] isLoadChunk;

	private bool generatingChunk = false;
	private Vector3 generatePosition;

	private string groundPath = Application.streamingAssetsPath + "/groundData.csv";
	private string wallPath = Application.streamingAssetsPath + "/WallData.csv";
	private string wallDirPath = Application.streamingAssetsPath + "/WallDirData.csv";

	public event Action<int> OnStartReadTileData;
	public event Action<int> OnReadingTileData;
	public event Action<int[,], int[,], Direction[,]> OnEndReadTileData;

	public event Action<int> OnStartWriteGroundData;
	public event Action<int> OnWritingGroundData;
	public event Action OnEndWriteGroundData;

	public event Action<int> OnStartWriteWallData;
	public event Action<int> OnWritingWallData;
	public event Action OnEndWriteWallData;

	public event Action<int> OnStartWriteWallDirData;
	public event Action<int> OnWritingWallDirData;
	public event Action OnEndWriteWallDirData;

	public void Setup(int[,] groundTiles, int[,] wallTiles, Direction[,] wallDirections)
	{
		tileDataContainer = Locator.Resolve<TileDataContainer>();
		updater = Locator.Resolve<IUpdater>(UpdaterID.MAP);
		this.groundTiles = groundTiles;
		this.wallTiles = wallTiles;
		this.wallDirections = wallDirections;
		updater.AddUpdate(this);
	}

	public void SetGround(TileID id, Vector2Int position)
    {
        GroundData data = tileDataContainer.GetGroundData(id);
		groundTilemap.SetTile((Vector3Int)position, data.Tile);
		groundTilemap.SetTileFlags((Vector3Int)position, TileFlags.None);
		SetTileID(id, position);
	}

	public void SetWall(TileID id, Vector2Int position, Direction dir = Direction.Down)
	{
		if(id == TileID.NullWall)
		{
			RemoveWall(position);
			return;
		}
		WallData data = tileDataContainer.GetWallData(id);
		Tilemap tilemap = GetWallTilemap(data);
		tilemap.SetTile((Vector3Int)position, data.Tile);
		tilemap.SetTileFlags((Vector3Int)position, TileFlags.None);
		if (data.ObjectType == TileObjectType.Tile)
		{
			wallShadowTilemap.SetTile((Vector3Int)position, data.Tile);
			wallShadowTilemap.SetTileFlags((Vector3Int)position, TileFlags.None);
		}
		SetTileID(id, position);
		SetWallDirection(dir, position);
		GameObject tileObj = tilemap.GetInstantiatedObject((Vector3Int)position);
		tileObj?.GetComponent<IPrefabTile>()?.OnPlaced(position, GetWallDirection(position));
	}

	public void RemoveWall(Vector2Int position)
	{
		if (!IsInsideMap(position)) return;
		TileID id = (TileID)wallTiles[position.x, position.y];
		Tilemap tilemap = GetWallTilemap(tileDataContainer.GetWallData(id));
		tilemap.GetInstantiatedObject((Vector3Int)position)?.GetComponent<IPrefabTile>()?.OnRemoved();
		tilemap.SetTile((Vector3Int)position, null);
		wallShadowTilemap.SetTile((Vector3Int)position, null);
		SetTileID(TileID.NullWall, position);
	}

	public TileID GetGroundID(Vector2Int position)
	{
		if (!IsInsideMap(position)) return TileID.NullGround;
		return (TileID)groundTiles[position.x, position.y];
	}

	public TileID GetWallID(Vector2Int position)
	{
		if (!IsInsideMap(position)) return TileID.NullWall;
		return (TileID)wallTiles[position.x, position.y];
	}

	public GroundData GetGroundData(Vector2Int position)
	{
		return tileDataContainer.GetGroundData((TileID)groundTiles[position.x, position.y]);
	}

	public WallData GetWallData(Vector2Int position)
	{
		return tileDataContainer.GetWallData((TileID)wallTiles[position.x, position.y]);
	}

	public TileData GetTileData(TileType tileType, Vector2Int position)
	{
		if (tileType == TileType.Ground) return GetGroundData(position);
		if (tileType == TileType.Wall) return GetWallData(position);
		return null;
	}

	/// <summary>
	/// TileType‚ªGround‚©Wall‚©”»•Ê‚ª‚Â‚©‚È‚¢Žž‚É—˜—p‚·‚é
	/// </summary>
	public void SetTile(TileID id, Vector2Int position, Direction dir = Direction.Up)
	{
		if(id.GetTileType() == TileType.Ground)
		{
			SetGround(id, position);
		}
		else
		if(id.GetTileType() == TileType.Wall)
		{
			SetWall(id, position, dir);
		}
	}

	private void SetTileID(TileID id, Vector2Int position)
	{
		if (!IsInsideMap(position)) return;
		switch (id.GetTileType())
		{
			case TileType.Ground:
				groundTiles[position.x, position.y] = (int)id;
				break;
			case TileType.Wall:
				wallTiles[position.x, position.y] = (int)id;
				break;
		}
	}

	public Direction GetWallDirection(Vector2Int position)
	{
		if (!IsInsideMap(position)) return Direction.None;
		return wallDirections[position.x, position.y];
	}

	public void SetWallDirection(Direction dir, Vector2Int position)
	{
		if (!IsInsideMap(position)) return;
		WallData data = tileDataContainer.GetTileData((TileID)GetTiles(TileType.Wall)[position.x, position.y]) as WallData;
		if(data.ChangableDir)
		{
			Quaternion rotation = Quaternion.Euler(0, 0, dir.ToRotationZ());
			Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
			Tilemap tilemap = GetWallTilemap(data);
			tilemap.SetTransformMatrix((Vector3Int)position, matrix);
			if (data.ObjectType == TileObjectType.Tile)
			{
				wallShadowTilemap.SetTransformMatrix((Vector3Int)position, matrix);
			}
			else
			if (data.ObjectType == TileObjectType.Prefab)
			{
				GameObject tileObj = tilemap.GetInstantiatedObject((Vector3Int)position);
				tileObj.transform.rotation = rotation;
				tileObj.GetComponent<IPrefabTile>()?.OnSetDirection(dir);
			}
			wallDirections[position.x, position.y] = dir;
		}
	}

	public void MoveWall(Vector2Int start, Vector2Int goal, float speed)
	{
		WallData wallData = GetWallData(start);
		if (wallData.ID.IsNull())
		{
			return;
		}
		RemoveWall(start);
		
		MovingTileView instance = Instantiate(movingTilePrefab);
		instance.SetTile(wallData.Tile.m_DefaultSprite);
		instance.Move(start, goal, speed, (pos, dir) => SetWall(wallData.ID, pos, dir));
	}

	public bool CanMoveWall(Vector2Int start, Vector2Int goal)
	{
		Vector2 dist = start - goal;
		return Mathf.Abs(dist.x) > 1 || Mathf.Abs(dist.y) > 1;
	}

	private Tilemap GetTilemapByTileID(TileID id)
	{
		switch(id.GetTileType())
		{
			case TileType.Ground:
				return groundTilemap;
			case TileType.Wall:
				WallData data = (WallData)tileDataContainer.GetTileData(id);
				return GetWallTilemap(data);
		}
		return null;
	}

	private Tilemap GetWallTilemap(WallData data)
	{
		if (data == null)
		{
			return null;
		}
		if (data.IsCollider)
		{
			return wallTilemap;
		}
		else
		{
			return wallNonColTilemap;
		}
	}

	public Sprite GetTileSprite(TileType tileType, Vector2Int position)
	{
		if (!IsInsideMap(position)) return null;
		TileID id = (TileID)GetTiles(tileType)[position.x, position.y];
		return GetTilemapByTileID(id).GetSprite((Vector3Int)position);
	}

	public Texture2D GetTileTexture(TileType tileType, Vector2Int position)
	{
		if (!IsInsideMap(position)) return null;
		//TileData data = tileDataContainer.GetTileData((TileID)GetTiles(tileType)[position.x, position.y]);
		//return ((RuleTile)data.Tile).m_DefaultSprite.GetTextureSameSizeAsSprite();
		return GetTileSprite(tileType, position).GetTextureSameSizeAsSprite();
	}

	private int[,] GetTiles(TileType tileType)
	{
		if (tileType == TileType.Ground) return groundTiles;
		if (tileType == TileType.Wall) return wallTiles;
		return null;
	}

	private string GetTileSavePath(TileType tileType)
	{
		if (tileType == TileType.Ground) return groundPath;
		if (tileType == TileType.Wall) return wallPath;
		return null;
	}

	void IUpdate.OnUpdate()
	{
		if (generatingChunk)
		{
			GenerateChunk(generatePosition);
		}
	}

	public async UniTask ReadMapData()
	{
		string groundContents = File.ReadAllText(groundPath);
		string wallContents = File.ReadAllText(wallPath);
		string wallDirContents = File.ReadAllText(wallDirPath);

		groundTiles = new int[MapExtension.WORLD_WIDTH, MapExtension.WORLD_HEIGHT];
		wallTiles = new int[MapExtension.WORLD_WIDTH, MapExtension.WORLD_HEIGHT];
		wallDirections = new Direction[MapExtension.WORLD_WIDTH, MapExtension.WORLD_HEIGHT];
		OnStartReadTileData?.Invoke(MapExtension.WORLD_HEIGHT);

		string[] groundLines = groundContents.Split('\n');
		string[] wallLines = wallContents.Split('\n');
		string[] wallDirLines = wallContents.Split('\n');
		for (int y = 0; y < groundLines.Length; y++)
		{
			string[] groundLineTiles = groundLines[y].Split(',');
			string[] wallLineTiles = wallLines[y].Split(',');
			string[] wallDirLineTiles = wallLines[y].Split(',');
			for (int x = 0; x < groundLineTiles.Length; x++)
			{
				groundTiles[x, y] = int.Parse(groundLineTiles[x]);
				wallTiles[x, y] = int.Parse(wallLineTiles[x]);
				wallDirections[x, y] = (Direction)int.Parse(wallLineTiles[x]);
			}
			OnReadingTileData?.Invoke(y);
			await UniTask.Delay(1);
		}
		isLoadChunk = new bool[MapExtension.WORLD_WIDTH / MapExtension.CHUNK_SIZE, MapExtension.WORLD_HEIGHT / MapExtension.CHUNK_SIZE];

		OnEndReadTileData?.Invoke(groundTiles, wallTiles, wallDirections);
	}

	public async UniTask WriteMapData(int[,] newGroundTiles, int[,] newWallTiles, Direction[,] newWallDirections)
	{
		OnStartWriteGroundData?.Invoke(MapExtension.WORLD_HEIGHT);
		string groundContents = await CreateMapFileContents(newGroundTiles, 3, progress =>
		{
			if (progress != MapExtension.WORLD_HEIGHT)
			{
				OnWritingGroundData?.Invoke(progress);
			}
			else
			{
				OnEndWriteGroundData?.Invoke();
			}
		});

		File.WriteAllText(groundPath, groundContents);

		OnStartWriteWallData?.Invoke(MapExtension.WORLD_HEIGHT);
		string wallContents = await CreateMapFileContents(newWallTiles, 3, progress =>
		{
			if (progress != MapExtension.WORLD_HEIGHT)
			{
				OnWritingWallData?.Invoke(progress);
			}
			else
			{
				OnEndWriteWallData?.Invoke();
			}
		});

		File.WriteAllText(wallPath, wallContents);

		OnStartWriteWallDirData?.Invoke(MapExtension.WORLD_HEIGHT);
		string wallDirContents = await CreateMapFileContents(newWallDirections, 3, progress =>
		{
			if (progress != MapExtension.WORLD_HEIGHT)
			{
				OnWritingWallDirData?.Invoke(progress);
			}
			else
			{
				OnEndWriteWallDirData?.Invoke();
			}
		});

		File.WriteAllText(wallDirPath, wallContents);
	}

	public async UniTask WriteMapData()
	{
		await WriteMapData(groundTiles, wallTiles, wallDirections);
	}

	private async UniTask<string> CreateMapFileContents(int[,] mapData, int waitCountForProgress, Action<int> progress)
	{
		StringBuilder builder = new StringBuilder();
		int count = 0;
		for (int y = 0; y < MapExtension.WORLD_HEIGHT; y++)
		{
			for (int x = 0; x < MapExtension.WORLD_WIDTH; x++)
			{
				builder.Append(mapData[x, y]);
				builder.Append(',');
			}
			builder.Replace(',', '\n', builder.Length - 1, 1);

			count++;
			if (count % waitCountForProgress == 0)
			{
				progress?.Invoke(y);
				await UniTask.Delay(1);
			}
		}
		builder.Replace("\n", null, builder.Length - 1, 1);
		return builder.ToString();
	}

	private async UniTask<string> CreateMapFileContents(Direction[,] mapData, int waitCountForProgress, Action<int> progress)
	{
		StringBuilder builder = new StringBuilder();
		int count = 0;
		for (int y = 0; y < MapExtension.WORLD_HEIGHT; y++)
		{
			for (int x = 0; x < MapExtension.WORLD_WIDTH; x++)
			{
				builder.Append(mapData[x, y]);
				builder.Append(',');
			}
			builder.Replace(',', '\n', builder.Length - 1, 1);

			count++;
			if (count % waitCountForProgress == 0)
			{
				progress?.Invoke(y);
				await UniTask.Delay(1);
			}
		}
		builder.Replace("\n", null, builder.Length - 1, 1);
		return builder.ToString();
	}

	public void StartGenerateChunk(Vector3 position)
	{
		generatingChunk = true;
		SetGeneratePosition(position);
	}

	public void SetGeneratePosition(Vector3 position)
	{
		generatePosition = position;
	}

	private void GenerateChunk(Vector2 position)
	{
		Vector2Int tilePos = position.WorldToTilePosition();
		int chunkPosX = tilePos.x == 0 ? 0 : tilePos.x / MapExtension.CHUNK_SIZE;
		int chunkPosY = tilePos.y == 0 ? 0 : tilePos.y / MapExtension.CHUNK_SIZE;
		PutGroundsToChunk(chunkPosX, chunkPosY);
		PutGroundsToChunk(chunkPosX + 1, chunkPosY);
		PutGroundsToChunk(chunkPosX - 1, chunkPosY);
		PutGroundsToChunk(chunkPosX, chunkPosY + 1);
		PutGroundsToChunk(chunkPosX, chunkPosY - 1);
		PutGroundsToChunk(chunkPosX + 1, chunkPosY - 1);
		PutGroundsToChunk(chunkPosX - 1, chunkPosY + 1);
		PutGroundsToChunk(chunkPosX + 1, chunkPosY + 1);
		PutGroundsToChunk(chunkPosX - 1, chunkPosY - 1);
	}

	private void PutGroundsToChunk(int chunkX, int chunkY)
	{
		if (chunkX < 0 || chunkY < 0 || isLoadChunk[chunkX, chunkY])
		{
			return;
		}

		for (int _y = 0; _y < MapExtension.CHUNK_SIZE; _y++)
		{
			for (int _x = 0; _x < MapExtension.CHUNK_SIZE; _x++)
			{
				int x = chunkX * MapExtension.CHUNK_SIZE + _x;
				int y = chunkY * MapExtension.CHUNK_SIZE + _y;

				SetWall((TileID)wallTiles[x, y], new Vector2Int(x, y), wallDirections[x, y]);
				SetGround((TileID)groundTiles[x, y], new Vector2Int(x, y));
			}
		}

		isLoadChunk[chunkX, chunkY] = true;
	}

	private bool IsInsideMap(Vector2Int position)
	{
		if (MapExtension.WORLD_WIDTH > position.x && MapExtension.WORLD_HEIGHT > position.y && position.x >= 0 && position.y >= 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private void OnDestroy()
	{
		updater.RemoveUpdate(this);
	}
}
