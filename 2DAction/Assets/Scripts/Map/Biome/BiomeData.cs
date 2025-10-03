using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BiomeData 
{
	[SerializeField] private string name;
	[SerializeField] private BiomeID id;
	[SerializeField] private TileID baseGroundID;
	[SerializeField] private TileID baseWallID;
	[SerializeField] private Color themeColor;
	[SerializeField] private Vector2Int startPosMin;
	[SerializeField] private Vector2Int startPosMax;

	public string Name => name;
	public BiomeID ID => id;
	public TileID BaseGroundID => baseGroundID;
	public TileID BaseWallID => baseWallID;
	public Color ThemeColor => themeColor;
	public Vector2Int StartPosMin => startPosMin;
	public Vector2Int StartPosMax => startPosMax;
}
