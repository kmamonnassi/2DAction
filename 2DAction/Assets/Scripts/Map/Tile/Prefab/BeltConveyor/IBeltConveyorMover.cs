public interface IBeltConveyorMover
{
	void AddMoveEntity(Entity entity, Direction dir);
	void RemoveMoveEntity(Entity entity, Direction dir);
}