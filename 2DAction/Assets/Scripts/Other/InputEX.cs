using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputEX
{
	public static Vector2 WorldMousePosition()
	{
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
}
