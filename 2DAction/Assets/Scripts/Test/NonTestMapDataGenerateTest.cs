using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

public class NonTestMapDataGenerateTest : MonoBehaviour
{
	[SerializeField] private ActType actType;
	[SerializeField] private Slider slider;
	[SerializeField] private Transform player;
	private BiomeDataContainer biomeDataContainer;
	private IMap map;

	private enum ActType
	{
		ReadAndGenerate,
		Write,
	}

	public async void Start()
	{
		biomeDataContainer = Locator.Resolve<BiomeDataContainer>();
		map = Locator.Resolve<IMap>();

		if (actType == ActType.ReadAndGenerate)
		{
			map.OnStartReadTileData += x =>
			{
				slider.minValue = 0;
				slider.maxValue = MapExtension.WORLD_HEIGHT;
			};

			map.OnReadingTileData += x =>
			{
				slider.value = x;
			};

			map.OnEndReadTileData += (groundDatas, wallTiles, wallDirections) =>
			{
				map.Setup(groundDatas, wallTiles, wallDirections);
				map.StartGenerateChunk(player.transform.position);
			};
			await map.ReadMapData();
		}
		else
		if(actType == ActType.Write)
		{
			slider.minValue = 0;
			slider.maxValue = MapExtension.WORLD_WIDTH * MapExtension.WORLD_HEIGHT;

			(int[,], Direction[,]) wall = await WriteWallData();
			int[,] ground = await WriteGroundData(x => slider.value = x);

			map.OnStartWriteGroundData += x =>
			{
				slider.minValue = 0;
				slider.maxValue = MapExtension.WORLD_HEIGHT;
			};
			map.OnWritingGroundData += x =>
			{
				slider.value = x;
			};

			await map.WriteMapData(ground, wall.Item1, wall.Item2);
		}
	}

	private void Update()
	{
		map.SetGeneratePosition(player.transform.position);
	}

	private UniTask<int[,]> WriteGroundData(Action<int> onGeneratingFirstMapData)
	{
		return new FirstMapGroundGenerator().GenerateGroundData(biomeDataContainer.Datas, 1000, onGeneratingFirstMapData);
	}

	private UniTask<(int[,], Direction[,])> WriteWallData()
	{
		return new FirstMapWallGenerator().GenerateWallData(TileID.GreenBrickWall, 3, 0.5f);
	}
}
