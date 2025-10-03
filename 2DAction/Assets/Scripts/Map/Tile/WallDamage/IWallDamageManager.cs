using UnityEngine;

public interface IWallDamageManager
{
	void AddDamage(Vector2Int position, float addBreakingRate, Vector2 dir);
	void Dig(Vector2Int position, int level, float speed, Vector2 digDir);
}