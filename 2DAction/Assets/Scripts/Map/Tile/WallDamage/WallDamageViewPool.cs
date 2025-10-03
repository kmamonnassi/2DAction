using System.Collections.Generic;
using UnityEngine;

public class WallDamageViewPool : MonoBehaviour
{
	[SerializeField] private WallDamageView prefab;

	private List<WallDamageView> views = new List<WallDamageView>();

	public WallDamageView GetDamageView(Vector2Int position)
	{
		WallDamageView view = GetNowPositionView(position);
		if(view == null)
		{
			view = GetInactiveView();
		}
		if (view == null)
		{
			view = Instantiate(prefab, transform);
			views.Add(view);
		}

		return view;
	}

	public void HideDamageView(Vector2Int position)
	{
		WallDamageView view = GetNowPositionView(position);
		if (view != null)
		{
			view.Inactive();
		}
	}

	public void HideDamageCrack(Vector2Int position)
	{
		WallDamageView view = GetNowPositionView(position);
		if (view != null)
		{
			view.HideCrack();
		}
	}

	private WallDamageView GetNowPositionView(Vector2Int position)
	{
		foreach (WallDamageView view in views)
		{
			if (view.gameObject.activeInHierarchy && view.Position == position)
			{
				return view;
			}
		}
		return null;
	}

	private WallDamageView GetInactiveView()
	{
		foreach(WallDamageView view in views)
		{
			if(!view.gameObject.activeInHierarchy)
			{
				return view;
			}
		}
		return null;
	}
}