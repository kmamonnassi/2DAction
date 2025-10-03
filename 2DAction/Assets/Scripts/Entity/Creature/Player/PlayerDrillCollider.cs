using System;
using UnityEngine;

public class PlayerDrillCollider : MonoBehaviour
{
	[SerializeField] private CircleCollider2D col;

	public event Action<Collider2D> OnEnter;
	public event Action<Collider2D> OnExit;
	public float ColliderSize => col.radius;

	private void OnTriggerStay2D(Collider2D collision)
	{
		OnEnter?.Invoke(collision);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		OnExit?.Invoke(collision);
	}
}
