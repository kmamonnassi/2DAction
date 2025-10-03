using System.Collections.Generic;


public class UseItemActionContainer
{
	private List<IUseItemAction> actions = new List<IUseItemAction>();

	public UseItemActionContainer()
	{
		
		AddAction<UseTileItemAction>();
	}

	public IUseItemAction GetAction(ItemID id)
	{
		if(id >= 0 && (int)id < 20000)
		{
			return GetAction<UseTileItemAction>();
		}
		return null;
	}

	private void AddAction<T>() where T : IUseItemAction, new()
	{
		actions.Add(new T());
	}

	private IUseItemAction GetAction<T>() where T : IUseItemAction, new()
	{
		return actions.Find(x => x is T);
	}
}