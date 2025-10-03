using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

public class FirstMapGroundGenerator
{
	private int[,] ground;
	private int[,] wall;
	private Dictionary<BiomeID, List<Vector2Int>> canPutGroundPositions;
	Dictionary<BiomeID, Vector2Int> firstBiomePos = new Dictionary<BiomeID, Vector2Int>();

	public async UniTask<int[,]> GenerateGroundData(IReadOnlyList<BiomeData> datas, int waitPutCountProgress, Action<int> onGenerating)
	{
		ground = new int[MapExtension.WORLD_WIDTH, MapExtension.WORLD_HEIGHT];
		for (int y = 0; y < MapExtension.WORLD_HEIGHT; y++)
		{
			for (int x = 0; x < MapExtension.WORLD_WIDTH; x++)
			{
				ground[x, y] = -1;
			}
		}
		canPutGroundPositions = new Dictionary<BiomeID, List<Vector2Int>>();
		firstBiomePos = new Dictionary<BiomeID, Vector2Int>();

		foreach (var data in datas)
		{
			var pos = new Vector2Int(Random.Range(data.StartPosMin.x, data.StartPosMax.x), Random.Range(data.StartPosMin.y, data.StartPosMax.y));
			firstBiomePos.Add(data.ID, pos);
			canPutGroundPositions.Add(data.ID, new List<Vector2Int>() { pos });
			SetGround(pos, data);
		}

		int notSetTileCount = 0;
		int putCount = 0;
		int nowWaitPutCountProgress = waitPutCountProgress;
		Direction nowDir = Direction.Down;

		while (true)
		{
			bool isSetTile = false;
			foreach (BiomeData data in datas)
			{
				Vector2Int pos = GetRandomCanPutGroundPos(data.ID, nowDir);
				if (!CanPutGround(pos.x, pos.y))
				{
					continue;
				}
				SetGround(pos, data);
				putCount++;
				isSetTile = true;
			}
			if (!isSetTile)
			{
				notSetTileCount++;
				if (notSetTileCount >= 4)
				{
					break;
				}
			}
			else
			{
				notSetTileCount = 0;
			}

			if (putCount > nowWaitPutCountProgress)
			{
				onGenerating?.Invoke(putCount);
				nowWaitPutCountProgress += waitPutCountProgress;
				await UniTask.Delay(1);
			}

			nowDir = ChangeDirection(nowDir);
		}
		return ground;
	}

	private void SetGround(Vector2Int pos, BiomeData data)
	{
		if (!CanPutGround(pos.x, pos.y))
		{
			Debug.LogError("’u‚¯‚È‚¢!" + pos + "/" + data.ID);
			return;
		}

		ground[pos.x, pos.y] = (int)data.BaseGroundID;
		RemoveCanPutGroundPositionAll(pos);

		for(int i = 0; i < 4;i++)
		{
			Direction dir = (Direction)i;
			Vector2Int canPutPos = pos + dir.GetDirVec2Int();
			AddCanPutGroundPosition(data.ID, canPutPos);
		}
	}

	private bool CanPutGround(int x, int y)
	{
		if (x >= MapExtension.WORLD_WIDTH || x < 0) return false;
		if (y >= MapExtension.WORLD_HEIGHT || y < 0) return false;
		return ground[x, y] == -1;
	}

	private void AddCanPutGroundPosition(BiomeID id, Vector2Int pos)
	{
		if(!canPutGroundPositions[id].Contains(pos) && CanPutGround(pos.x, pos.y))
		{
			canPutGroundPositions[id].Add(pos);
		}
	}

	private void RemoveCanPutGroundPositionAll(Vector2Int pos)
	{
		foreach(var value in canPutGroundPositions.Values)
		{
			value.Remove(pos);
		}
	}

	private Direction ChangeDirection(Direction dir)
	{
		switch (dir)
		{
			case Direction.Up:
				return Direction.Left;
			case Direction.Left:
				return Direction.Down;
			case Direction.Down:
				return Direction.Right;
			case Direction.Right:
				return Direction.Up;
		}
		return Direction.Up;
	}

	public Vector2Int GetRandomCanPutGroundPos(BiomeID id, Direction dir)
	{
		List<Vector2Int> positions = new List<Vector2Int>();
		switch (dir)
		{
			case Direction.Up:
				positions = canPutGroundPositions[id].Where(x => x.y < firstBiomePos[id].y).ToList();
				break;
			case Direction.Left:
				positions = canPutGroundPositions[id].Where(x => x.x > firstBiomePos[id].x).ToList();
				break;
			case Direction.Down:
				positions = canPutGroundPositions[id].Where(x => x.y > firstBiomePos[id].y)?.ToList();
				break;
			case Direction.Right:
				positions = canPutGroundPositions[id].Where(x => x.x < firstBiomePos[id].x).ToList();
				break;
		}
		if (positions.Count == 0) return new Vector2Int(-1, -1);
		return positions[Random.Range(0, positions.Count)];
	}
}