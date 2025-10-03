using System;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlotView : MonoBehaviour
{
	[SerializeField] private Image itemIcon;
	[SerializeField] private Text amountText;
	[SerializeField] private GameObject select;

	public void SetItem(Sprite item, int amount)
	{
		itemIcon.sprite = item;
		amountText.text = amount.ToString();

		itemIcon.gameObject.SetActive(true);
		amountText.gameObject.SetActive(true);
	}

	public void RemoveItem()
	{
		itemIcon.gameObject.SetActive(false);
		amountText.gameObject.SetActive(false);
	}

	public void Select()
	{
		select.SetActive(true);
	}

	public void Deselect()
	{
		select.SetActive(false);
	}
}
