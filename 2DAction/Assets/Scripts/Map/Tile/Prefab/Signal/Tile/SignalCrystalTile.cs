using UnityEngine;

public class SignalCrystalTile : MonoBehaviour, IPrefabTile
{
	private ISignalManager signalManager;

	private Vector2Int position;

	public void OnPlaced(Vector2Int position, Direction dir)
	{
		this.position = position;
		signalManager = Locator.Resolve<ISignalManager>();

		signalManager.SetReceived(position, true);
		signalManager.SetReceived(position + Vector2Int.down, true);
		signalManager.SetReceived(position + Vector2Int.left, true);
		signalManager.SetReceived(position + Vector2Int.up, true);
		signalManager.SetReceived(position + Vector2Int.right, true);
	}

	public void OnRemoved()
	{
		signalManager.SetReceived(position, false);
		signalManager.SetReceived(position + Vector2Int.down, false);
		signalManager.SetReceived(position + Vector2Int.left, false);
		signalManager.SetReceived(position + Vector2Int.up, false);
		signalManager.SetReceived(position + Vector2Int.right, false);
	}

	public void OnSetDirection(Direction dir)
	{
	}
}