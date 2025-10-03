using UnityEngine;

public interface IPlayerInfo
{
	Vector3 Position { get; }
	Vector3 EulerAngles { get; }
	Vector3 LocalScale { get; }
	Entity Entity { get; }
	Creature Creature { get; }
}
