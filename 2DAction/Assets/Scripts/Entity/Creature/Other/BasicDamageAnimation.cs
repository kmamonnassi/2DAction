using UnityEngine;

[RequireComponent(typeof(Creature))]
public class BasicDamageAnimation : MonoBehaviour
{
	[SerializeField] private OnceParticleSystemPlayer damageEffectPrefab;

	private void Start()
	{
		Creature creature = GetComponent<Creature>();
		creature.OnHitAttack += data =>
		{
			OnceParticleSystemPlayer instance = Instantiate(damageEffectPrefab, null);
			instance.transform.position = new Vector3(transform.position.x, transform.position.y, instance.transform.position.z);
			instance.transform.eulerAngles = new Vector3(0, 0, data.AttackDirection.GetAim());
			instance.gameObject.SetActive(true);
			instance.Play();
		};
	}
}