using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Entity))]
public class Creature : MonoBehaviour, IHitAttack
{
	[SerializeField, Tooltip("Ž©“®‚ÅÅ‘å‘Ì—Í‚ªÝ’è‚³‚ê‚é‚©")] private bool startToSetup = true;
	[SerializeField, Tooltip("‰ŠúÅ‘åHP")] private int initalizeMaxHP = 100;
	[SerializeField] private CreatureType creatureType = CreatureType.Enemy;
	[SerializeField] private SpriteRenderer[] rends;

	public int MaxHP { get; private set; } 
	public int HP { get; private set; }
	public bool IsDead { get; private set; }

	public HitAttackType HitAttackType => (HitAttackType)creatureType;

	public event Action<int> OnSetHP;
	public event Action<int> OnSetMaxHP;
	public event Action<int> OnDamage;
	public event Action OnDead;
	public event Action<AttackData> OnHitAttack;

	private Entity entity;
	private List<AttackData> invincibleAttackData = new List<AttackData>();
	private Tween damageEffectTween;

	private void Start()
	{
		if(startToSetup)
		{
			Setup();
		}
	}

	public void Setup()
	{
		SetMaxHP(this.initalizeMaxHP);
		SetHP(this.initalizeMaxHP);
		entity = GetComponent<Entity>();
	}
	
	public void Setup(int initalizeMaxHP)
	{
		SetMaxHP(initalizeMaxHP);
		SetHP(initalizeMaxHP);
	}

	public void SetMaxHP(int maxHP)
	{
		if (IsDead) return;
		MaxHP = maxHP;
		if(HP > MaxHP)
		{
			SetHP(MaxHP);
		}
		OnSetMaxHP?.Invoke(maxHP);
	}

	public void SetHP(int hp)
	{
		if (IsDead) return;
		if(hp > MaxHP)
		{
			HP = MaxHP;
		}
		else
		{
			HP = hp;
		}
		OnSetHP?.Invoke(HP);

		if(HP <= 0)
		{
			Dead();
		}
	}

	public void Revival(int hp)
	{
		IsDead = false;
		SetHP(hp);
	}

	public void Damage(int damage)
	{
		if (IsDead) return;
		SetHP(HP - damage);
		DamageEffect();
		OnDamage?.Invoke(damage);
	}

	private void DamageEffect()
	{
		damageEffectTween?.Kill();
		damageEffectTween = DOVirtual.Color(Color.red, Color.white, 0.1f, x =>
		{
			foreach (SpriteRenderer rend in rends)
			{
				rend.color = x;
			}
		});
	}

	public void Dead()
	{
		IsDead = true;
		OnDead?.Invoke();
	}

	public void HitAttack(AttackData attackData)
	{
		if (invincibleAttackData.Contains(attackData)) return;

		Damage(attackData.Damage);
		StartInvincible(attackData);
		Knockback(attackData.AttackDirection, attackData.KnockbackPower);
		OnHitAttack?.Invoke(attackData);
	}

	public void StartInvincible(AttackData attackData)
	{
		if(!invincibleAttackData.Contains(attackData) && gameObject.activeInHierarchy)
		{ 
			StartCoroutine(InvincibleCoroutine(attackData));
		}
	}

	private IEnumerator InvincibleCoroutine(AttackData attackData)
	{
		invincibleAttackData.Add(attackData);
		yield return new WaitForSeconds(attackData.InvincibleTime);
		invincibleAttackData.Remove(attackData);
	}

	public void Knockback(Vector2 dir, float power)
	{
		if (!gameObject.activeInHierarchy) return;
		StartCoroutine(KnockbackCoroutine(dir, power));
		entity.Move(dir, power);
	}

	private IEnumerator KnockbackCoroutine(Vector2 dir, float power)
	{
		float duration = 0.05f;
		while(duration > 0)
		{
			duration -= Time.deltaTime;
			entity.Move(dir, power);
			yield return null;
		}
	}

	private void OnDestroy()
	{
		damageEffectTween?.Kill();
	}
}
