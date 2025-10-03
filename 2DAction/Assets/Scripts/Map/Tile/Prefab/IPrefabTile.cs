using UnityEngine;

public interface IPrefabTile
{
	public void OnPlaced(Vector2Int position, Direction dir);
	public void OnSetDirection(Direction dir);
	public void OnRemoved();
}