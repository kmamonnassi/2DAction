using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeDataContainer", menuName = "Container/BiomeData")]
public class BiomeDataContainer : ScriptableObject
{
    [SerializeField] private BiomeData[] datas;

    public IReadOnlyList<BiomeData> Datas => datas;

    public BiomeData GetData(BiomeID id) => System.Array.Find(datas, x => x.ID == id);
}
