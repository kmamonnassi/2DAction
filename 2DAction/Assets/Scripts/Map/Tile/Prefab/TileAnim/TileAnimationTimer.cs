using System;

public class TileAnimationTimer : ITileAnimationTimer, ILocatorInitalizer, IUpdate
{
	public event Action<int> OnAddFrame;

	private IUpdater updater;
	private int frame;

	public void Initalize()
	{
		updater = Locator.Resolve<IUpdater>(UpdaterID.MAP);
		updater.AddUpdate(this);
	}

	public void OnUpdate()
	{
		frame++;
		OnAddFrame?.Invoke(frame);
	}
}
