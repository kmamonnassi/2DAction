public class UseItemData
{
	public readonly IPlayerInfo PlayerInfo;
	public readonly IMap Map;
	public readonly IItemInventory ItemInventory;

	public UseItemData(IPlayerInfo playerInfo, IMap map, IItemInventory itemInventory)
	{
		PlayerInfo = playerInfo;
		Map = map;
		ItemInventory = itemInventory;
	}
}