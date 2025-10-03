using UnityEngine;

public class PlayerHPBar : MonoBehaviour
{
	[SerializeField] private PlayerHPSlot[] slots;
	[SerializeField] private int slotMaxHP = 25;

	private IPlayerInfo playerInfo;

	private void Start()
	{
		playerInfo = Locator.Resolve<IPlayerInfo>();

		playerInfo.Creature.OnSetMaxHP += maxHP =>
		{
			SetMaxHP(maxHP);
		};

		playerInfo.Creature.OnSetHP += hp =>
		{
			SetHP(hp);
		};
		SetMaxHP(playerInfo.Creature.MaxHP);
		SetHP(playerInfo.Creature.HP);
	}

	public void SetMaxHP(int maxHP)
	{
		for(int i = 0; i < slots.Length;i++)
		{
			bool isActiveSlot = i * slotMaxHP < maxHP || maxHP - i * slotMaxHP > 0;
			slots[i].gameObject.SetActive(isActiveSlot);
		}
		SetHP(playerInfo.Creature.HP);
	}

	public void SetHP(int hp)
	{
		for (int i = 0; i < slots.Length; i++)
		{
			if(!slots[i].gameObject.activeInHierarchy)
			{
				return;
			}
			float alpha = (hp - i * (float)slotMaxHP) / slotMaxHP;
			slots[i].SetAlpha(Mathf.Clamp01(alpha));
		}
	}
}