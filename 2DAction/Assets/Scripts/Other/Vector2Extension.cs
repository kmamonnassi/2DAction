using UnityEngine;

public static class Vector2Extension 
{
    public static float GetAim(this Vector2 dir)
    {
        float rad = Mathf.Atan2(dir.y, dir.x);
        return rad * Mathf.Rad2Deg - 90;
    }

    public static Vector2Int WorldToTilePosition(this Vector2 position)
    {
        Vector3 tilePos = position / MapExtension.TILE_SIZE;
        int tilePosX = Mathf.FloorToInt(tilePos.x);
        int tilePosY = Mathf.FloorToInt(tilePos.y);
        return new Vector2Int(tilePosX, tilePosY);
    }

    public static Vector2Int WorldToTilePosition(this Vector3 position)
    {
        return WorldToTilePosition((Vector2)position);
    }
}
