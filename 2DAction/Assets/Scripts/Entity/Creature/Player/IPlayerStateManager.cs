public interface IPlayerStateManager
{
	void StartState(PlayerStateType state);
	void EndState(PlayerStateType state);
	bool ContainsState(params PlayerStateType[] states);
	bool HaveAllState(params PlayerStateType[] states);
}
