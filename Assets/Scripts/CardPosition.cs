using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CardPosition : MonoBehaviour
{
	public bool isOccupied;
	public bool isRed;
	public Card cardInPosition;
	private Color color;
	private Color savedColor;
	private Material material;

	private void Awake()
	{
		color = this.transform.GetComponent<MeshRenderer>().material.color;
		savedColor = color;
		material = this.transform.GetComponent<MeshRenderer>().material;
		isRed = false;
	}

	public bool IsFrontLine()
	{
		if (transform.name.Substring(0, 3) == "Fro")
		{
			return true;
		}
		
		return false;
	}

	public void RedAlert(bool active)
	{
		if (active)
		{
			color = Color.red;
			color.a = 0.4f;
			isRed = true;
		}
		else
		{
			color = savedColor;
			isRed = false;
		}
		material.SetColor("_Color", color);
	}

	public void Lights(bool on)
	{
		if (on)
		{
			color.a = 1;
		}
		else
		{
			color.a = 0;
		}
		material.SetColor("_Color", color);
	}
}
