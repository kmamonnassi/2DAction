using UnityEngine;

public class MainInstaller : MonoBehaviour
{
	[SerializeField] private BiomeDataContainer biomeData;
	[SerializeField] private TileDataContainer tileData;
	[SerializeField] private EntityDataContainer entityData;
	[SerializeField] private Updater entityUpdater;
	[SerializeField] private Updater mapUpdater;
	[SerializeField] private Updater uiUpdater;
	[SerializeField] private VignetteManager vignetteManager;
	[SerializeField] private PlayerStateManager stateManager;
	[SerializeField] private PlayerInfoManager playerInfoManager;
	[SerializeField] private ItemDataContainer itemDataContainer;
	[SerializeField] private DroppedItemManager droppedItemManager;
	[SerializeField] private Map map;
	[SerializeField] private WallDamageManager wallDamageManager;

	private void Awake()
	{
		Locator.Register<BiomeDataContainer>(biomeData);
		Locator.Register<TileDataContainer>(tileData);
		Locator.Register<EntityDataContainer>(entityData);
		Locator.Register<IVignetteManager>(vignetteManager);
		Locator.Register<IUpdater>(entityUpdater, UpdaterID.ENTITY);
		Locator.Register<IUpdater>(mapUpdater, UpdaterID.MAP);
		Locator.Register<IUpdater>(uiUpdater, UpdaterID.UI);
		Locator.Register<IPlayerStateManager>(stateManager);
		Locator.Register<IPlayerInfo>(playerInfoManager);
		Locator.Register<IItemDataContainer>(itemDataContainer);
		Locator.Register<IDroppedItemManager>(droppedItemManager);
		Locator.Register<IMap>(map);
		Locator.Register<IWallDamageManager>(wallDamageManager);

		Locator.Register<IItemInventory>(new ItemInventory());
		Locator.Register<IHotbar>(new Hotbar());
		Locator.Register<IBeltConveyorMover>(new BeltConveyorMover());
		Locator.Register<ITileAnimationTimer>(new TileAnimationTimer());
		Locator.Register<DropItemByPlayer>(new DropItemByPlayer());
		Locator.Register<ISignalManager>(new SignalManager());
		Locator.Register<IGimmickActivater>(new GimmickActivater());

		Locator.Initalize();
	}
}