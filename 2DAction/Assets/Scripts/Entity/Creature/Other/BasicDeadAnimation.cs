using UnityEngine;

[RequireComponent(typeof(Creature))]
public class BasicDeadAnimation : MonoBehaviour
{
    [SerializeField] private OnceParticleSystemPlayer deadEffect;

	private void Start()
	{
		Creature creature = GetComponent<Creature>();
		creature.OnHitAttack += data =>
		{
			deadEffect.transform.eulerAngles = new Vector3(0, 0, data.AttackDirection.GetAim());
		};
		creature.OnDead += () =>
		{
			deadEffect.Play();
			Destroy(gameObject);
		};
	}
}
