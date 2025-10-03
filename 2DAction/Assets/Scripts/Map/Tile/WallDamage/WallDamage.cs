using System;
using UnityEngine;

public class WallDamage
{
	public Vector2Int Position { get; private set; }
	public float Duration { get; private set; }
	public float BreakingRate { get; private set; }

	public event Action OnReset;
	public event Action OnBreak;

	public WallDamage(Vector2Int position)
	{
		Position = position;
		Duration = MapExtension.WALL_BREAKING_DURATION;
	}

	public void AddBreakingRate(float addBreakingRate)
	{
		Duration = MapExtension.WALL_BREAKING_DURATION;
		BreakingRate += addBreakingRate;
		if (BreakingRate >= 1)
		{
			OnBreak?.Invoke();
		}
	}

	public void OnUpdate()
	{
		Duration -= Time.deltaTime;
		if(Duration <= 0)
		{
			OnReset?.Invoke();
		}
	}
}