using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public interface IMap
{
	event Action<int> OnStartReadTileData;
	event Action<int> OnReadingTileData;
	event Action<int[,], int[,], Direction[,]> OnEndReadTileData;

	event Action<int> OnStartWriteGroundData;
	event Action<int> OnWritingGroundData;
	event Action OnEndWriteGroundData;

	public event Action<int> OnStartWriteWallData;
	public event Action<int> OnWritingWallData;
	public event Action OnEndWriteWallData;

	public event Action<int> OnStartWriteWallDirData;
	public event Action<int> OnWritingWallDirData;
	public event Action OnEndWriteWallDirData;

	void Setup(int[,] groundTiles, int[,] wallTiles, Direction[,] newWallDirections);

	/// <summary>
	/// TileType‚ªGround‚©Wall‚©”»•Ê‚ª‚Â‚©‚È‚¢Žž‚É—˜—p‚·‚é
	/// </summary>
	void SetTile(TileID id, Vector2Int position, Direction dir = Direction.Down);

	void SetGround(TileID id, Vector2Int position);
	
	void SetWall(TileID id, Vector2Int position, Direction dir = Direction.Down);
	void RemoveWall(Vector2Int position);
	
	TileID GetGroundID(Vector2Int position);
	TileID GetWallID(Vector2Int position);
	
	GroundData GetGroundData(Vector2Int position);
	WallData GetWallData(Vector2Int position);
	TileData GetTileData(TileType tileType, Vector2Int tilePos);
	
	Direction GetWallDirection(Vector2Int position);
	void SetWallDirection(Direction dir, Vector2Int position);

	void MoveWall(Vector2Int start, Vector2Int goal, float speed);
	bool CanMoveWall(Vector2Int start, Vector2Int goal);

	Sprite GetTileSprite(TileType tileType, Vector2Int position);
	Texture2D GetTileTexture(TileType tileType, Vector2Int position);
	
	void StartGenerateChunk(Vector3 position);
	void SetGeneratePosition(Vector3 position);

	UniTask ReadMapData();
	UniTask WriteMapData();
	UniTask WriteMapData(int[,] newGroundTiles, int[,] newWallTiles, Direction[,] newWallDirections);
}
