using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TileAnimationPlayer : MonoBehaviour
{
	[SerializeField] private TileAnimationData animData;

	private SpriteRenderer rend;

	private bool isPlaying;

	private void OnEnable()
	{
		rend = GetComponent<SpriteRenderer>();
		Locator.Resolve<ITileAnimationTimer>().OnAddFrame += OnAddFrame;
		isPlaying = true;
	}

	public void OnAddFrame(int frame)
	{
		if (!isPlaying) return;
		rend.sprite = animData.Sprites[frame / animData.Interavl % animData.Sprites.Length];
	}

	public void Restart()
	{
		isPlaying = true;
	}

	public void Stop()
	{
		isPlaying = false;
	}

	private void OnDisable()
	{
		Locator.Resolve<ITileAnimationTimer>().OnAddFrame -= OnAddFrame;
	}
}
