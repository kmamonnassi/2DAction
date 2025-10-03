using UnityEngine;

[System.Serializable]
public class EntityData
{
    [SerializeField] private EntityID id;
    [SerializeField] private string name;
    [SerializeField, TextArea(0, 5)] private string discription;
    [SerializeField] private Entity entityPrefab;

	public EntityID ID => id;
	public string Name => name;
	public string Discription => discription;
	public Entity EntityPrefab => entityPrefab;
}
