using UnityEngine;

public class PlayerInfoManager : MonoBehaviour, IPlayerInfo
{
	public Vector3 Position => transform.position;

	public Vector3 EulerAngles => transform.eulerAngles;

	public Vector3 LocalScale => transform.localScale;

	public Entity Entity
	{
		get
		{
			if (entity == null)
			{
				entity = GetComponent<Entity>();
			}
			return entity;
		}
	}

	public Creature Creature
	{
		get
		{
			if (entity == null)
			{
				creature = GetComponent<Creature>();
			}
			return creature;
		}
	}

	private Entity entity;
	private Creature creature;
}
