using DG.Tweening;
using UnityEngine;

public class PunchBoxTile : MonoBehaviour, IPrefabTile, IGimmickActivatable
{
	[SerializeField] private Animator animator;
	[SerializeField] private float interval = 1.5f;
	[SerializeField] private float animWaitTime = 0.05f;
	[SerializeField] private int punchPower = 9;

	private ISignalManager signalManager;
	private IGimmickActivater gimmickActivater;
	private IMap map;
	
	private Direction dir;
	private Vector2Int position;
	private Tween waitPunch;

	public void OnPlaced(Vector2Int position, Direction dir)
	{
		signalManager = Locator.Resolve<ISignalManager>();
		gimmickActivater = Locator.Resolve<IGimmickActivater>();
		map = Locator.Resolve<IMap>();

		this.position = position;
		this.dir = dir;

		signalManager.OnReceived += OnSignalReceived;
		signalManager.OnCancelReceived += OnCancelSignalReceived;

		if (signalManager.IsReceived(position))
		{
			OnSignalReceived(position);
		}
	}

	public void OnRemoved()
	{
		signalManager.OnReceived -= OnSignalReceived;
		signalManager.OnCancelReceived -= OnCancelSignalReceived;
		gimmickActivater.RemoveActivatable(interval, this);
	}

	public void OnSetDirection(Direction dir)
	{
		this.dir = dir;
	}

	private void OnSignalReceived(Vector2Int pos)
	{
		if (position != pos) return;
		gimmickActivater.AddActivatable(interval, this);
	}

	private void OnCancelSignalReceived(Vector2Int pos)
	{
		if (position != pos) return;
		gimmickActivater.RemoveActivatable(interval, this);
		StopPunchAnimation();
	}

	private void PunchAnimation()
	{
		animator.Play("Punch");
		waitPunch = DOVirtual.DelayedCall(animWaitTime, Punch);
	}

	private void StopPunchAnimation()
	{
		animator.Play("Idle");
		waitPunch?.Kill();
	}

	private void Punch()
	{
		Vector2Int punchStartPos = position + dir.GetDirVec2Int();
		Vector2Int punchGoalPos = punchStartPos + dir.GetDirVec2Int() * punchPower;

		map.MoveWall(punchStartPos, punchGoalPos, 10);
		map.MoveWall(punchStartPos + dir.GetDirVec2Int(), punchGoalPos + dir.GetDirVec2Int(), 10);
	}

	public void OnActive()
	{
		PunchAnimation();
	}
}
