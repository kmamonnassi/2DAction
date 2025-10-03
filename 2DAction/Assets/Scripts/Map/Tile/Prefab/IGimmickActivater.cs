public interface IGimmickActivater
{
	void AddActivatable(float interval, IGimmickActivatable activatable);
	void RemoveActivatable(float interval, IGimmickActivatable activatable);
}