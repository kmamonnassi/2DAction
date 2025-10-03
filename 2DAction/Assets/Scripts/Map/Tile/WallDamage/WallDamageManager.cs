using DG.Tweening;
using System.Collections.Generic;
using UnityEditor.Sprites;
using UnityEngine;


public class WallDamageManager : MonoBehaviour, IUpdate, IWallDamageManager
{
	[SerializeField] private WallDamageViewPool viewPool;
	[SerializeField] private float removeDamageViewDelay = 2;
	private IUpdater updater;
	private IMap map;
	private IDroppedItemManager droppedItemManager;

	private List<WallDamage> wallDamages = new List<WallDamage>();

	private void Start()
	{
		updater = Locator.Resolve<IUpdater>(UpdaterID.MAP);
		map = Locator.Resolve<IMap>();
		droppedItemManager = Locator.Resolve<IDroppedItemManager>();
		updater.AddUpdate(this);
	}

	void IUpdate.OnUpdate()
	{
		new List<WallDamage>(wallDamages).ForEach(x => x.OnUpdate());
	}

	private void OnDestroy()
	{
		updater.RemoveUpdate(this);
	}

	public void AddDamage(Vector2Int position, float addBreakingRate, Vector2 dir)
	{
		WallDamage wallDamage = wallDamages.Find(x => x.Position == position);
		if (wallDamage == null)
		{
			wallDamage = new WallDamage(position);

			wallDamage.OnBreak += () =>
			{
				wallDamages.Remove(wallDamage);
				
				Vector2Int dropItemPos = position * MapExtension.TILE_SIZE + new Vector2Int(MapExtension.TILE_SIZE / 2, MapExtension.TILE_SIZE / 2);
				ItemID dropItem = (ItemID)map.GetWallID(position);
				
				map.RemoveWall(position);
				viewPool.HideDamageCrack(position);
				DOVirtual.DelayedCall(removeDamageViewDelay, () => viewPool.HideDamageView(position));

				droppedItemManager.Create(dropItem, 1, dropItemPos, 0);
			};

			wallDamage.OnReset += () =>
			{
				wallDamages.Remove(wallDamage);
				viewPool.HideDamageCrack(position);
				DOVirtual.DelayedCall(removeDamageViewDelay, () => viewPool.HideDamageView(position));
			};

			wallDamages.Add(wallDamage);
		}
		WallDamageView view = viewPool.GetDamageView(position);
		view.Show(position, wallDamage.BreakingRate, dir);
		view.SetBreakEffectTex(map.GetTileTexture(TileType.Wall, position));
		view.Active();
		wallDamage.AddBreakingRate(addBreakingRate);
	}

	public void Dig(Vector2Int position, int level, float speed, Vector2 digDir)
	{
		WallData wallData = map.GetWallData(position);
		if (wallData.RequireDrillLevel <= level)
		{
			float addBreakingRate = speed * Time.deltaTime / wallData.Hardness;
			AddDamage(position, addBreakingRate, digDir);
		}
	}
}