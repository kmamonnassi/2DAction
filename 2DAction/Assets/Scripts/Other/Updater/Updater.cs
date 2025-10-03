using System.Collections.Generic;
using UnityEngine;

public class Updater : MonoBehaviour, IUpdater
{
	private List<IUpdate> updates = new List<IUpdate>();
	private List<IFixedUpdate> fixedUpdates = new List<IFixedUpdate>();

	private List<IUpdate> addUpdates = new List<IUpdate>();
	private List<IFixedUpdate> addFixedUpdates = new List<IFixedUpdate>();

	private List<IUpdate> removeUpdates = new List<IUpdate>();
	private List<IFixedUpdate> removeFixedUpdates = new List<IFixedUpdate>();

	private bool updating = true;

	public void AddUpdate(IUpdate update)
	{
		if(!updates.Contains(update) && !addUpdates.Contains(update))
		{
			updates.Add(update);
		}
	}

	public void AddUpdateSafe(IUpdate update)
	{
		if (!updates.Contains(update) && !addUpdates.Contains(update))
		{
			addUpdates.Add(update);
		}
	}

	public void RemoveUpdate(IUpdate update)
	{
		updates.Remove(update);
	}

	public void RemoveUpdateSafe(IUpdate update)
	{
		removeUpdates.Add(update);
	}

	public void AddFixedUpdate(IFixedUpdate update)
	{
		if (!fixedUpdates.Contains(update) && !addFixedUpdates.Contains(update))
		{
			fixedUpdates.Add(update);
		}
	}

	public void AddFixedUpdateSafe(IFixedUpdate update)
	{
		if (!fixedUpdates.Contains(update) && !addFixedUpdates.Contains(update))
		{
			addFixedUpdates.Add(update);
		}
	}

	public void RemoveFixedUpdate(IFixedUpdate update)
	{
		fixedUpdates.Remove(update);
	}

	public void RemoveFixedUpdateSafe(IFixedUpdate update)
	{
		removeFixedUpdates.Add(update);
	}

	private void Update()
	{
		if (!updating) return;

		updates.AddRange(addUpdates);
		addUpdates.Clear();
		removeUpdates.ForEach(x => updates.Remove(x));
		removeUpdates.Clear();
		updates.ForEach(x => x.OnUpdate());
	}

	private void FixedUpdate()
	{
		if (!updating) return;
		fixedUpdates.AddRange(addFixedUpdates);
		addFixedUpdates.Clear();
		fixedUpdates.ForEach(x => x.OnFixedUpdate());
		removeFixedUpdates.ForEach(x => fixedUpdates.Remove(x));
		removeFixedUpdates.Clear();
	}

	public void Pause()
	{
		updating = false;
	}

	public void Restart()
	{
		updating = true;
	}
}
