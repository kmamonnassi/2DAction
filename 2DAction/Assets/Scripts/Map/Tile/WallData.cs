using UnityEngine;

[System.Serializable]
public class WallData : TileData
{
    public override TileType TileType => TileType.Wall;
    
    [SerializeField] private float hardness = 1;
    [SerializeField] private int requireDrillLevel = 1;
    [SerializeField] private bool antiExplosive = false;
    [SerializeField] private bool isCollider = true;
    [SerializeField] private bool changeableDir = false;

    public float Hardness => hardness;
    public int RequireDrillLevel => requireDrillLevel;
    public bool AntiExplosive => antiExplosive;
    public bool IsCollider => isCollider;
    public bool ChangableDir => changeableDir;
}
