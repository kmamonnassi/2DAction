using UnityEngine;

[System.Serializable]
public class AttackData
{
	public HitAttackType[] HitTarget;
	public int Damage;
	public float KnockbackPower;
	public float InvincibleTime;
	[HideInInspector] public Vector2 AttackDirection;

	public AttackData(HitAttackType[] hitTarget, int damage, float knockbackPower, Vector2 attackDirection, float invincibleTime = 0.05f)
	{
		HitTarget = hitTarget;
		Damage = damage;
		KnockbackPower = knockbackPower;
		AttackDirection = attackDirection;
		InvincibleTime = invincibleTime;
	}
}