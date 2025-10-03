using System.Collections.Generic;
using UnityEngine;

public class GimmickActivater : IGimmickActivater, IUpdate, ILocatorInitalizer
{
	private Dictionary<float, List<IGimmickActivatable>> activatables = new Dictionary<float, List<IGimmickActivatable>>();
	private float beforeTime = -1;

	public void Initalize()
	{
		IUpdater updater = Locator.Resolve<IUpdater>(UpdaterID.MAP);
		updater.AddUpdate(this);
	}

	public void OnUpdate()
	{
		float key = Mathf.Floor(Time.time * 100) / 100;
		if(key != beforeTime)
		{
			foreach (var pair in activatables)
			{
				if (key % pair.Key == 0)
				{
					foreach (IGimmickActivatable activatable in activatables[pair.Key])
					{
						activatable.OnActive();
					}
				}
			}
			beforeTime = key;
		}
	}

	public void AddActivatable(float intervavl, IGimmickActivatable activatable)
	{
		if(!activatables.ContainsKey(intervavl))
		{
			activatables.Add(intervavl, new List<IGimmickActivatable>());
		}
		if(!activatables[intervavl].Contains(activatable))
		{
			activatables[intervavl].Add(activatable);
		}
	}

	public void RemoveActivatable(float interval, IGimmickActivatable activatable)
	{
		if (!activatables.ContainsKey(interval)) return;
		activatables[interval].Remove(activatable);
	}
}
