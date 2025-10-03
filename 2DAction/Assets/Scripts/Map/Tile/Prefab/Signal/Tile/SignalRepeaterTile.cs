using UnityEngine;

public class SignalRepeaterTile : MonoBehaviour, IPrefabTile
{
	private ISignalManager signalManager;

	private Vector2Int position;
	private Direction dir;
	private bool repeated;

	private const int REPEAT_DISTANCE = 10;

	public void OnPlaced(Vector2Int position, Direction dir)
	{
		this.position = position;
		this.dir = dir;
		signalManager = Locator.Resolve<ISignalManager>();

		signalManager.OnReceived += CheckReceived;
		signalManager.OnCancelReceived += CheckReceived;

		CheckReceived(position);
	}

	public void OnRemoved()
	{
		if (repeated)
		{
			Repeat(dir, false);
			repeated = false;
		}

		signalManager.OnReceived -= CheckReceived;
		signalManager.OnCancelReceived -= CheckReceived;
	}

	public void OnSetDirection(Direction dir)
	{
		Direction beforeDir = this.dir;
		if(repeated && beforeDir != dir)
		{
			Repeat(beforeDir, false);
			Repeat(dir, true);
		}

		this.dir = dir;
	}

	private void CheckReceived(Vector2Int pos)
	{
		if (pos != position) return;

		if (signalManager.IsReceived(position))
		{
			if (!repeated)
			{
				Repeat(dir, true);
				repeated = true;
			}
		}
		else
		{
			if (repeated)
			{
				Repeat(dir, false);
				repeated = false;
			}
		}
	}

	private void Repeat(Direction dir, bool b)
	{
		for(int i = 1; i <= REPEAT_DISTANCE;i++)
		{
			signalManager.SetReceived(dir.GetDirVec2Int() * i + position, b);
		}
	}
}