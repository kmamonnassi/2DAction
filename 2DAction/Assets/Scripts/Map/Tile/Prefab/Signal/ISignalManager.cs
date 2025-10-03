using System;
using UnityEngine;

public interface ISignalManager
{
	event Action<Vector2Int> OnReceived;
	event Action<Vector2Int> OnCancelReceived;

	void SetReceived(int x, int y, bool b);
	void SetReceived(Vector2Int pos, bool b);
	void SetReceived(Vector2 pos, bool b);
	bool IsReceived(int x, int y);
	bool IsReceived(Vector2Int pos);
	bool IsReceived(Vector2 pos);
}