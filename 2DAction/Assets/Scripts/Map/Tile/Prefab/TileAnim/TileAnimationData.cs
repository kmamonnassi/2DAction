using UnityEngine;

[System.Serializable]
public class TileAnimationData
{
	[SerializeField] private Sprite[] sprites;
	[SerializeField] private int interavl = 10;

	public Sprite[] Sprites => sprites;
	public int Interavl => interavl;
}
