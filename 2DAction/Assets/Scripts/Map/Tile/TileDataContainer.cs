using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileDataContainer", menuName = "Container/TileData")]
public class TileDataContainer : ScriptableObject
{
    [SerializeField] private GroundData[] groundDatas;
    [SerializeField] private WallData[] wallDatas;

    public IReadOnlyList<GroundData> GroundDatas => groundDatas;
    public IReadOnlyList<WallData> WallDatas => wallDatas;

    public GroundData GetGroundData(TileID id)
    {
        return Array.Find(groundDatas, x => x.ID == id);
    }

    public WallData GetWallData(TileID id)
    {
        return Array.Find(wallDatas, x => x.ID == id);
    }

    /// <summary>
    /// TileType‚ªGround‚©Wall‚©”»•Ê‚ª‚Â‚©‚È‚¢Žž‚É—˜—p‚·‚é
    /// </summary>
    public TileData GetTileData(TileID id)
    {
        switch(id.GetTileType())
        {
            case TileType.Ground:
                return GetGroundData(id);
            case TileType.Wall:
                return GetWallData(id);
        }
        return null;
	}
}
