using DG.Tweening;
using UnityEngine;

public static class EntityExtension
{
    public const float MAX_SPEED = 50000;

    public static Tween MoveEntity(this Entity entity, Vector2 dir, float power, float duration)
    {
        Tween t = DOVirtual.Float(0, 1, duration, MAX_SPEED => { entity.Move(dir, power); });
        t.SetUpdate(DG.Tweening.UpdateType.Fixed);
        return t;
	}
}
