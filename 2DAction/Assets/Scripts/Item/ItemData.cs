using UnityEngine;

[System.Serializable]
public class ItemData
{
    [SerializeField] private string name;
    [SerializeField] private ItemID id;
    [SerializeField] private ItemType type;
	[SerializeField] private Sprite icon;
    [SerializeField] private int maxStack = 99999;

	public string Name => name;
	public ItemID ID => id;
	public ItemType Type => type;
	public Sprite Icon => icon;
	public int MaxStack => maxStack;
}
