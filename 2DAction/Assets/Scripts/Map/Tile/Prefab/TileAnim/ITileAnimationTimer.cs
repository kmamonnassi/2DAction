using System;

public interface ITileAnimationTimer
{
	event Action<int> OnAddFrame;
}