using UnityEngine;

public class PutWallTest : MonoBehaviour
{
    [SerializeField] private Vector2Int startPos;
    [SerializeField] private Vector2Int endPos;
    [SerializeField] private TileID tileID;
    [SerializeField] private ItemID[] addTiles;

    private IMap map;
    private IItemInventory inventory;

    private void Start()
    {
        map = Locator.Resolve<IMap>();
        inventory = Locator.Resolve<IItemInventory>();

        map.Setup(MapExtension.GetFirstGroundTiles(), MapExtension.GetFirstWallTiles(), MapExtension.GetFirstWallDirections());
        for(int y = startPos.y; y < endPos.y;y++)
        {
            for (int x = startPos.y;x < endPos.x;x++)
            {
                map.SetWall(tileID, new Vector2Int(x, y));
            }
		}
        for(int i = 0; i < addTiles.Length; i++)
        inventory.AddItem(addTiles[i], 9999);
    }
}
