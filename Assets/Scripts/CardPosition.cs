using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPosition : MonoBehaviour
{
	public bool isOccupied;
	private Color color;

	private void Awake()
	{
		color = transform.GetComponent<MeshRenderer>().material.color;
	}

	public bool GetIsOccupied()
	{
		if (isOccupied)
		{
			return true;
		}
		return false;
	}

	public void LightOn()
	{
		color.a = 1;
	}

	public void LightOff()
	{
		color.a = 0;
	}
}
