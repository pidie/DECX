using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CardPosition : MonoBehaviour
{
	public bool isOccupied;
	private Color color;
	private Material material;

	private void Awake()
	{
		color = this.transform.GetComponent<MeshRenderer>().material.color;
		material = this.transform.GetComponent<MeshRenderer>().material;
	}

	public void LightOn()
	{
		color.a = 1;
		material.SetColor("_Color", color);
	}

	public void LightOff()
	{
		color.a = 0;
		material.SetColor("_Color", color);
	}
}
