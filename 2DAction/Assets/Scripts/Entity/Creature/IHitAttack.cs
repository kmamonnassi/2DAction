public interface IHitAttack
{
	HitAttackType HitAttackType { get; }
	void HitAttack(AttackData attackData);
}