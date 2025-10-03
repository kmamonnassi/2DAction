public interface IUpdater 
{
	void AddUpdate(IUpdate update);
	void AddUpdateSafe(IUpdate update);
	void RemoveUpdate(IUpdate update);
	void RemoveUpdateSafe(IUpdate update);
	void AddFixedUpdate(IFixedUpdate update);
	void AddFixedUpdateSafe(IFixedUpdate update);
	void RemoveFixedUpdate(IFixedUpdate update);
	void RemoveFixedUpdateSafe(IFixedUpdate update);
	void Pause();
	void Restart();
}