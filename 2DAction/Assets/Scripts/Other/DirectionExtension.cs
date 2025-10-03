using UnityEngine;

public static class DirectionExtension
{
	public static Direction RotationToDirection(float rotation)
	{
		switch (rotation)
		{
			case 0:
				return Direction.Up;
			case 90:
				return Direction.Left;
			case 180:
				return Direction.Down;
			case 270:
				return Direction.Right;
		}
		return 0;
	}

	public static float ToRotationZ(this Direction dir)
	{
		switch (dir)
		{
			case Direction.Up:
				return 0;
			case Direction.Left:
				return 90;
			case Direction.Down:
				return 180;
			case Direction.Right:
				return 270;
		}
		return 0;
	}

	public static Vector3Int GetDirVec3Int(this Direction dir)
	{
		switch (dir)
		{
			case Direction.Up:
				return Vector3Int.up;
			case Direction.Left:
				return Vector3Int.left;
			case Direction.Down:
				return Vector3Int.down;
			case Direction.Right:
				return Vector3Int.right;
		}
		return Vector3Int.zero;
	}
	public static Vector2Int GetDirVec2Int(this Direction dir)
	{
		switch (dir)
		{
			case Direction.Up:
				return Vector2Int.up;
			case Direction.Left:
				return Vector2Int.left;
			case Direction.Down:
				return Vector2Int.down;
			case Direction.Right:
				return Vector2Int.right;
		}
		return Vector2Int.zero;
	}
}