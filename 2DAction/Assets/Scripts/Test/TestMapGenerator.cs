using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace test
{
	public class TestMapGenerator : MonoBehaviour
	{
		[SerializeField] private Tilemap tilemap;
		[SerializeField] private TileBase testTile;
		[SerializeField] private List<TestBiomeData> datas = new List<TestBiomeData>();
		[SerializeField] private int mapWidth = 200;
		[SerializeField] private int mapHeight = 150;

		public void Start()
		{
			Generate();
		}

		[ContextMenu("Generate")]
		private void Generate()
		{
			StartCoroutine(GenerateCoroutine());
		}

		private IEnumerator GenerateCoroutine()
		{
			tilemap.transform.position = new Vector3(mapWidth / -2, mapHeight / -2, 0);

			List<TestBiomeData> _datas = new List<TestBiomeData>();
			foreach (TestBiomeData data in datas)
			{
				TestBiomeData _data = data.Copy();
				_data.ResetStartPos();
				_datas.Add(_data);
				SetTile(_data.StartPos, _data, datas);
			}
			int i = 0;
			int notSetTileCount = 0;
			Direction nowDir = Direction.Down;
			while (true)
			{
				bool isSetTile = false;
				foreach (TestBiomeData _data in _datas)
				{
					Vector2Int pos = _data.GetRandomCanPutPos(nowDir);
					if ((pos.x == -1 && pos.y == -1) || !CanPut(pos))
					{
						continue;
					}
					SetTile(pos, _data, _datas);
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
				nowDir = ChangeDirection(nowDir);
				i++;
				if (i % 100 == 0)
				{
					Debug.Log(i + "é¸");
					yield return null;
				}
			}
			Debug.Log("èIóπ");
		}

		private void SetTile(Vector2Int pos, TestBiomeData data, List<TestBiomeData> datas)
		{
			SetTile(pos.x, pos.y, data, datas);
		}

		private void SetTile(int x, int y, TestBiomeData data, List<TestBiomeData> datas)
		{
			if (!CanPut(x, y))
			{
				Debug.LogError("íuÇØÇ»Ç¢!" + x + "/" + y + "/" + data.color);
				return;
			}
			Vector3Int view_pos = new Vector3Int(x, mapHeight - y, 0);

			tilemap.SetTile(view_pos, testTile);
			tilemap.SetColor(view_pos, data.color);

			Vector3Int pos = new Vector3Int(x, y, 0);
			datas.ForEach(x => x.RemoveCanPutPos(pos));

			for (int i = 0; i < 4; i++)
			{
				Direction dir = (Direction)i;
				Vector3Int canPutPos = pos + dir.GetDirVec3Int();
				if (CanPut(canPutPos))
				{
					data.AddCanPutPos(canPutPos);
				}
			}
		}

		private bool CanPut(Vector2Int pos)
		{
			return CanPut(pos.x, pos.y);
		}

		private bool CanPut(Vector3Int pos)
		{
			return CanPut(pos.x, pos.y);
		}

		private bool CanPut(int x, int y)
		{
			Vector3Int pos = new Vector3Int(x, mapHeight - y, 0);
			if (x > mapWidth || x < 0) return false;
			if (y > mapHeight || y < 0) return false;
			return tilemap.GetTile(pos) == null;
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
	}

	[System.Serializable]
	public class TestBiomeData
	{
		public Color color;
		public Vector2Int startPosMin;
		public Vector2Int startPosMax;
		public Vector2Int StartPos { get; private set; }

		public List<Vector2Int> canPutPos { get; private set; } = new List<Vector2Int>();

		public TestBiomeData(Color color, Vector2Int startPosMin, Vector2Int startPosMax)
		{
			this.color = color;
			this.startPosMin = startPosMin;
			this.startPosMax = startPosMax;
			canPutPos = new List<Vector2Int>();
		}

		public void ResetStartPos()
		{
			int x = Random.Range(startPosMin.x, startPosMax.x);
			int y = Random.Range(startPosMin.y, startPosMax.y);
			StartPos = new Vector2Int(x, y);
		}

		public void RemoveCanPutPos(Vector2Int pos)
		{
			if (canPutPos == null)
			{
				canPutPos = new List<Vector2Int>();
				return;
			}
			if (canPutPos.Contains(pos))
			{
				canPutPos.Remove(pos);
			}
		}

		public void RemoveCanPutPos(Vector3Int pos)
		{
			RemoveCanPutPos((Vector2Int)pos);
		}

		public void AddCanPutPos(Vector2Int pos)
		{
			if (canPutPos == null)
			{
				canPutPos = new List<Vector2Int>();
				return;
			}
			if (canPutPos.Contains(pos)) return;
			canPutPos.Add(pos);
		}

		public void AddCanPutPos(Vector3Int pos)
		{
			AddCanPutPos((Vector2Int)pos);
		}

		public Vector2Int GetRandomCanPutPos(Direction dir)
		{
			if (canPutPos == null)
			{
				canPutPos = new List<Vector2Int>();
			}
			List<Vector2Int> positions = new List<Vector2Int>();
			switch (dir)
			{
				case Direction.Up:
					positions = canPutPos.Where(x => x.y < StartPos.y).ToList();
					break;
				case Direction.Left:
					positions = canPutPos.Where(x => x.x > StartPos.x).ToList();
					break;
				case Direction.Down:
					positions = canPutPos.Where(x => x.y > StartPos.y)?.ToList();
					break;
				case Direction.Right:
					positions = canPutPos.Where(x => x.x < StartPos.x).ToList();
					break;
			}
			if (positions.Count == 0) return new Vector2Int(-1, -1);
			return positions[Random.Range(0, positions.Count)];
		}

		public TestBiomeData Copy()
		{
			return JsonUtility.FromJson<TestBiomeData>(JsonUtility.ToJson(this));
		}
	}
}