using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class BalistaTile : MonoBehaviour, IPrefabTile, IGimmickActivatable
{
	[SerializeField] private TrapArrow arrowPrefab;
	[SerializeField] private Animator animator;
	[SerializeField] private float arrowSpeed = 32;
	[SerializeField] private float shotInterval = 1.5f;
	[SerializeField] private float shotSpace = 1.25f;
	[SerializeField] private float arrowDuration = 5.0f;
	[SerializeField] private float shotAnimWaitTime = 0.1f;

	private ISignalManager signalManager;
	private IGimmickActivater gimmickActivater;

	private Direction dir;
	private Vector2Int pos;
	private Tween waitShot;

	public void OnPlaced(Vector2Int pos, Direction dir)
	{
		this.dir = dir;
		this.pos = pos;
		signalManager = Locator.Resolve<ISignalManager>();
		gimmickActivater = Locator.Resolve<IGimmickActivater>();
		signalManager.OnReceived += OnSignalReceive;
		signalManager.OnCancelReceived += OnCancelSignalReceive;

		if(signalManager.IsReceived(pos))
		{
			OnSignalReceive(pos);
		}
	}

	public void OnRemoved()
	{
		signalManager.OnReceived -= OnSignalReceive;
		signalManager.OnCancelReceived -= OnCancelSignalReceive;
		gimmickActivater.RemoveActivatable(shotInterval, this);
		StopShotAnimation();
	}

	public void OnSetDirection(Direction dir)
	{
		this.dir = dir;
	}

	private void OnSignalReceive(Vector2Int pos)
	{
		if (this.pos != pos) return;

		gimmickActivater.AddActivatable(shotInterval, this);
	}

	private void OnCancelSignalReceive(Vector2Int pos)
	{
		if (this.pos != pos) return;

		gimmickActivater.RemoveActivatable(shotInterval, this);
		StopShotAnimation();
	}

	private void Shot()
	{
		Vector2 pos = (Vector2)transform.position + shotSpace * MapExtension.TILE_SIZE * (Vector2)dir.GetDirVec2Int();
		TrapArrow arrow = Instantiate(arrowPrefab, pos, Quaternion.identity);
		arrow.gameObject.SetActive(true);
		arrow.Initalize(dir, arrowSpeed, arrowDuration);
	}

	private void ShotAnimation()
	{
		animator.Play("Shot");
		waitShot = DOVirtual.DelayedCall(shotAnimWaitTime, Shot);
	}

	private void StopShotAnimation()
	{
		animator.Play("Idle");
		waitShot?.Kill();
	}

	public void OnActive()
	{
		ShotAnimation();
	}
}