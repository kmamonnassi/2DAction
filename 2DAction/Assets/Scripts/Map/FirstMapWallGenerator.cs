using Cysharp.Threading.Tasks;
using UnityEngine;

public class FirstMapWallGenerator
{
	public async UniTask<(int[,], Direction[,])> GenerateWallData(TileID wallID, int blurCount, float generateRate)
	{
		int width = MapExtension.WORLD_WIDTH;
		int height = MapExtension.WORLD_HEIGHT;

		float[,] noise = new float[width, height];

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				noise[x, y] = Random.value;
			}
		}
		await UniTask.Delay(1);

		float[,] noiseTmp = noise;
		for (int j = 0; j < blurCount; j++)
		{
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					if (x + 1 >= width || x - 1 < 0) continue;
					if (y + 1 >= height || y - 1 < 0) continue;
					noise[x, y] = noiseTmp[x, y] + noiseTmp[x + 1, y] + noiseTmp[x, y + 1] + noiseTmp[x + 1, y + 1] + noiseTmp[x - 1, y] + noiseTmp[x, y - 1] + noiseTmp[x - 1, y - 1] + noiseTmp[x + 1, y - 1] + noiseTmp[x - 1, y + 1];
					noise[x, y] /= 9.0f;
				}
			}
		}
		await UniTask.Delay(1);

		int[,] walls = new int[width, height];
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				if(noise[x, y] > generateRate)
				{
					walls[x, y] = (int)wallID;
				}
				else
				{
					walls[x, y] = (int)TileID.NullWall;
				}
			}
		}
		Direction[,] directions = new Direction[width, height];
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				directions[x, y] = Direction.Up;
			}
		}

		return (walls, directions);
	}
}
