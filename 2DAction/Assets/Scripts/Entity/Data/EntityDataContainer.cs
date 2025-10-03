using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityDataContainer", menuName = "Container/EntityData")]
public class EntityDataContainer : ScriptableObject
{
	[SerializeField] private List<EntityData> datas;

	public EntityData GetEntityData(EntityID id)
	{
		return datas.Find(x => x.ID == id);
	}
}
