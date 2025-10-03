using System;
using UnityEngine;

public class SignalManager : MonoBehaviour, ISignalManager
{
    private int[,] received = new int[MapExtension.WORLD_WIDTH, MapExtension.WORLD_HEIGHT];

	public event Action<Vector2Int> OnReceived;
	public event Action<Vector2Int> OnCancelReceived;

	public void SetReceived(int x, int y, bool b)
    {
        SetReceived(new Vector2Int(x, y), b);
    }

    public void SetReceived(Vector2Int pos, bool b)
    {
        if (pos.x < 0 || pos.y < 0) return;

        int before = received[pos.x, pos.y];
        received[pos.x, pos.y] += b ? 1 : -1;
        if (before == 0 && received[pos.x, pos.y] == 1)
        {
            OnReceived?.Invoke(pos);
        }
        else if(before == 1 && received[pos.x, pos.y] == 0)
        {
            OnCancelReceived?.Invoke(pos);
		}
    }

    public void SetReceived(Vector2 pos, bool b)
    {
        SetReceived(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y), b);
	}

    public bool IsReceived(int x, int y)
    {
        return received[x, y] > 0;
    }

    public bool IsReceived(Vector2Int pos)
    {
        return IsReceived(pos.x, pos.y);
    }

    public bool IsReceived(Vector2 pos)
    {
        return IsReceived(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y));
    }
}
