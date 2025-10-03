using System;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
	[SerializeField] private AttackData attackData;

	public event Action<IHitAttack> OnHit;

	private void OnTriggerStay2D(Collider2D col)
	{
		HitCheck(col.gameObject);
	}

	private void OnCollisionStay2D(Collision2D col)
	{
		HitCheck(col.gameObject);
	}

	private void HitCheck(GameObject target)
	{
		IHitAttack hitAttack = target.GetComponent<IHitAttack>();
		if (hitAttack != null)
		{
			foreach(var type in attackData.HitTarget)
			{
				if(type == hitAttack.HitAttackType)
				{
					attackData.AttackDirection = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
					hitAttack.HitAttack(attackData);
					OnHit?.Invoke(hitAttack);
					return;
				}
			}
		}
	}
}
