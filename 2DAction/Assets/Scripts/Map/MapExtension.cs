using UnityEngine;

public static class MapExtension
{
    public const int TILE_SIZE = 16;

    public const int WORLD_WIDTH = 1000;
    public const int WORLD_HEIGHT = 750;

    public const int CHUNK_SIZE = 32;

    public const float WALL_BREAKING_DURATION = 30;

    public static TileType GetTileType(this TileID id)
	{
        int _id = (int)id;
        if(_id >= 0)
        {
            if (_id < 10000)
            {
                return TileType.Ground;
            }
            else if (_id >= 10000 && _id < 20000)
            {
                return TileType.Wall;
            }
        }
        else
        {
            if (_id == -1)
            {
                return TileType.Ground;
            }
            else if (_id == -10001)
            {
                return TileType.Wall;
            }

        }
        return TileType.Ground;
    }

    public static int[,] GetFirstGroundTiles()
    {
        int[,] groundTiles = new int[MapExtension.WORLD_WIDTH, MapExtension.WORLD_HEIGHT];
        for (int y = 0; y < MapExtension.WORLD_HEIGHT; y++)
        {
            for (int x = 0; x < MapExtension.WORLD_WIDTH; x++)
            {
                groundTiles[x, y] = -1;
            }
        }
        return groundTiles;
    }

    public static int[,] GetFirstWallTiles()
    {
        int[,] wallTiles = new int[MapExtension.WORLD_WIDTH, MapExtension.WORLD_HEIGHT];
        for (int y = 0; y < MapExtension.WORLD_HEIGHT; y++)
        {
            for (int x = 0; x < MapExtension.WORLD_WIDTH; x++)
            {
                wallTiles[x, y] = -10001;
            }
        }
        return wallTiles;
    }

    public static Direction[,] GetFirstWallDirections()
    {
        Direction[,] wallTiles = new Direction[MapExtension.WORLD_WIDTH, MapExtension.WORLD_HEIGHT];
        for (int y = 0; y < MapExtension.WORLD_HEIGHT; y++)
        {
            for (int x = 0; x < MapExtension.WORLD_WIDTH; x++)
            {
                wallTiles[x, y] = Direction.Down;
            }
        }
        return wallTiles;
    }

    public static bool IsNull(this TileID id)
    {
        return id == TileID.NullWall || id == TileID.NullGround;
	}

    public static bool IsInsideMap(Vector2Int position)
    {
        if (WORLD_WIDTH > position.x && WORLD_HEIGHT > position.y && position.x >= 0 && position.y >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}