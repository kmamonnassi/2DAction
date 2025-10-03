using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
	[SerializeField] private Animator animator;
	[SerializeField] private List<CanvasGroup> inactiveUIList;
	[SerializeField] private float showGameOverTime = 0.5f;
	[SerializeField] private float hideGameOverTime = 0.5f;
	[SerializeField] private GameObject playerDeadEffect;
    private IPlayerInfo playerInfo;
	private IUpdater entityUpdater;
	private IUpdater mapUpdater;
	private IVignetteManager vignetteManager;

	private Tween tween;

	private void Start()
	{
		playerInfo = Locator.Resolve<IPlayerInfo>();
		entityUpdater = Locator.Resolve<IUpdater>(UpdaterID.ENTITY);
		mapUpdater = Locator.Resolve<IUpdater>(UpdaterID.MAP);
		vignetteManager = Locator.Resolve<IVignetteManager>();

		playerInfo.Creature.OnDead += () =>
		{
			ShowGamOver();
		};
		playerInfo.Creature.OnHitAttack += data =>
		{
			playerDeadEffect.transform.eulerAngles = new Vector3(0, 0, data.AttackDirection.GetAim());
		};
	}

	private void ShowGamOver()
	{
		 animator.SetBool("ShowGameOver", true);

		inactiveUIList.ForEach(x =>
		{
			x.interactable = false;
		});
		tween?.Kill();
		tween = DOVirtual.Float(1, 0, showGameOverTime, x =>
		{
			inactiveUIList.ForEach(y =>
			{
				y.alpha = x;
			});
		});
		vignetteManager.SetVignetteData(VignetteID.GameOver, showGameOverTime);

		entityUpdater.Pause();
		mapUpdater.Pause();

		playerDeadEffect.transform.position = playerInfo.Position;
		playerInfo.Entity.gameObject.SetActive(false);
		playerDeadEffect.gameObject.SetActive(true);

		DOVirtual.DelayedCall(showGameOverTime / 2, () =>
		{
			StartCoroutine(WaitHideGameOver());
		});
	}

	private void HideGameOver()
	{
		animator.SetBool("ShowGameOver", false);

		tween?.Kill();
		tween = DOVirtual.Float(0, 1, hideGameOverTime, x =>
		{
			inactiveUIList.ForEach(y =>
			{
				y.alpha = x;
			});
		});
		tween.onComplete += () => 
		{
			inactiveUIList.ForEach(x =>
			{
				x.interactable = true;
			});
		};
		vignetteManager.SetVignetteData(VignetteID.Default, hideGameOverTime);
		playerDeadEffect.gameObject.SetActive(false);
		playerInfo.Entity.gameObject.SetActive(true);
		playerInfo.Entity.gameObject.transform.position = new Vector3(0, 0, playerInfo.Entity.gameObject.transform.position.z);
		playerInfo.Creature.Revival(playerInfo.Creature.MaxHP);
		entityUpdater.Restart();
		mapUpdater.Restart();
	}

	public IEnumerator WaitHideGameOver()
	{
		yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
		yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
		HideGameOver();
	}

	private void OnDestroy()
	{
		tween?.Kill();
	}
}
