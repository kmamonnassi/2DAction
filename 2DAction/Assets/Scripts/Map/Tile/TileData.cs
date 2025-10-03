using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public abstract class TileData
{
	[SerializeField] private TileID id;
	[SerializeField] private TileObjectType objectType;
	[SerializeField] private RuleTile tile;

	public TileID ID => id;
	public TileObjectType ObjectType => objectType;
	public RuleTile Tile => tile;

	public abstract TileType TileType { get; }
}
