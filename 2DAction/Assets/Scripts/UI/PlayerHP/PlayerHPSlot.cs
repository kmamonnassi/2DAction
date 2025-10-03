using UnityEngine;
using UnityEngine.UI;

public class PlayerHPSlot : MonoBehaviour
{
	[SerializeField] private Image heart;

	public void SetAlpha(float value)
	{
		heart.color = new Color(1, 1, 1, value);
	}
}
