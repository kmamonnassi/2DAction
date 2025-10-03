using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Entity : MonoBehaviour, IFixedUpdate
{
    [SerializeField] private Rigidbody2D rb = null;
	private IUpdater updater;

	private Vector2 moveVelocity;

	public event Action OnDestroyed;

	private void Start()
	{
		updater = Locator.Resolve<IUpdater>(UpdaterID.ENTITY);
		updater.AddFixedUpdate(this);
	}

	public void Move(Vector2 dir, float power)
    {
		moveVelocity += dir * power;
	}

	void IFixedUpdate.OnFixedUpdate()
	{
		if (moveVelocity.magnitude < 0)
		{
			return;
		}
		else if (moveVelocity.magnitude > EntityExtension.MAX_SPEED)
		{
			moveVelocity = Vector3.ClampMagnitude(moveVelocity, EntityExtension.MAX_SPEED);
		}
		rb.AddForce(moveVelocity, ForceMode2D.Impulse);
		moveVelocity = Vector2.zero;
	}

	private void OnDestroy()
	{
		updater.RemoveFixedUpdate(this);
		OnDestroyed?.Invoke();
		OnDestroyed -= OnDestroyed;
	}
}
