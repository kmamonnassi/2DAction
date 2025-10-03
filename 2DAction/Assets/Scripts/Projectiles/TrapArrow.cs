using System.Collections;
using UnityEngine;

public class TrapArrow : MonoBehaviour
{
	[SerializeField] private Rigidbody2D rb;
	[SerializeField] private AttackCollider col;

	private void Start()
	{
		col.OnHit += OnHit;
	}

	private void OnHit(IHitAttack hit)
	{
		Destroy(gameObject);
	}

	public void Initalize(Direction dir, float speed, float duration)
	{
		rb.velocity = (Vector2)dir.GetDirVec2Int() * speed;
		transform.eulerAngles = new Vector3(0, 0, dir.ToRotationZ());
		StartCoroutine(WaitDestroy(duration));
	}

	private IEnumerator WaitDestroy(float duration)
	{
		yield return new WaitForSeconds(duration);
		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if(col.CompareTag(TagNames.WALL))
		{
			Destroy(gameObject);
		}
	}

	private void OnDestroy()
	{
		col.OnHit -= OnHit;
	}
}