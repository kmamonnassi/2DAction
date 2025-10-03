using System.Collections.Generic;
using UnityEngine;

public class BeltConveyorTile : MonoBehaviour, IPrefabTile
{
	[SerializeField] private TileAnimationPlayer tileAnimationPlayer;

	private IBeltConveyorMover beltConveyorMover;
	private ISignalManager signalManager;

	public Direction Direction { get; private set; }

	private List<Entity> targetEntities = new List<Entity>();
	private bool isSignalReceived;
	private Vector2Int position;

	public void OnPlaced(Vector2Int position, Direction dir)
	{
		beltConveyorMover = Locator.Resolve<IBeltConveyorMover>();
		signalManager = Locator.Resolve<ISignalManager>();
		Direction = dir;
		this.position = position;

		signalManager.OnReceived += OnSignalReceived;
		signalManager.OnCancelReceived += OnCancelSignalReceived;

		isSignalReceived = signalManager.IsReceived(position);
		if(isSignalReceived)
		{
			Debug.Log(isSignalReceived);
			tileAnimationPlayer.Stop();
		}
	}

	public void OnSetDirection(Direction dir)
	{
		Direction = dir;
	}

	public void OnRemoved()
	{
		signalManager.OnReceived -= OnSignalReceived;
		signalManager.OnCancelReceived -= OnCancelSignalReceived;
	}

	private void OnSignalReceived(Vector2Int pos)
	{
		if (position != pos) return;

		isSignalReceived = true;
		if (targetEntities == null) return;
		foreach(Entity entity in targetEntities)
		{
			beltConveyorMover.RemoveMoveEntity(entity, Direction);
		}
		tileAnimationPlayer.Stop();
	}

	private void OnCancelSignalReceived(Vector2Int pos)
	{
		if (position != pos) return;

		isSignalReceived = false;
		if (targetEntities == null) return;
		foreach (Entity entity in targetEntities)
		{
			beltConveyorMover.AddMoveEntity(entity, Direction);
		}
		tileAnimationPlayer.Restart();
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		Entity entity = col.GetComponent<Entity>();
		if(entity != null)
		{
			if(!isSignalReceived)
			{
				beltConveyorMover.AddMoveEntity(entity, Direction);
			}
			targetEntities.Add(entity);
		}
	}

	private void OnTriggerExit2D(Collider2D col)
	{
		Entity entity = col.GetComponent<Entity>();
		if (entity != null)
		{
			beltConveyorMover.RemoveMoveEntity(entity, Direction);
			targetEntities.Remove(entity);
		}
	}
}
