using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Hand : MonoBehaviour
{
	public List<Card> cardsInHand;
	[CanBeNull] public CardPosition dropOff { get; private set; }

	[Range(-10f, 10f)] [SerializeField]	private float rayOffsetX;
	[Range(-10f, 10f)] [SerializeField] private float rayOffsetY;
	[Range(-10f, 10f)] [SerializeField] private float rayOffsetZ;

	public float cardWidth;
	[Range(0f, 5f)] public float widthBetweenCards;

	private void Awake()
	{
		widthBetweenCards = 1.2f;
		cardWidth = 7f;
		dropOff = null;
		
		rayOffsetX = 0; rayOffsetY = 0; rayOffsetZ = 0;
	}

	private void Update()
	{
		ArrangeCardsInHand();
		HoldingCard();
	}

	public int NumOfCards()
	{
		return cardsInHand.Count;
	}

	private void ArrangeCardsInHand()
	{
		if (NumOfCards() > 0)
		{
			int n = NumOfCards() - 1;
			float xPosition = NumOfCards() > 1 ? ((cardWidth + widthBetweenCards) / 2) * n : 0f;
			Camera mainCamera = Camera.main;

			foreach (Transform child in transform)
			{
				child.transform.position = transform.position + new Vector3(xPosition, 0, 0);
				xPosition -= (cardWidth + widthBetweenCards);
                
				float cameraRotAdjust = mainCamera.transform.localEulerAngles.x;
				child.transform.rotation = Quaternion.Euler(cameraRotAdjust, 180, 0);
			}
		}
	}

	private void HoldingCard()
	{
		foreach (Card card in cardsInHand)
		{
			if (card.isBeingHeld)
			{
				Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
					Camera.main.WorldToScreenPoint(card.transform.position).z - 0.5f);
				card.transform.position = Camera.main.ScreenToWorldPoint(pos);

				float rayLength = 80f;
				Vector3 rayOffset = new Vector3(rayOffsetX, rayOffsetY, rayOffsetZ);
				
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition + rayOffset);
				Debug.DrawRay(ray.origin, ray.direction * rayLength);

				RaycastHit[] hits;
				hits = Physics.RaycastAll(ray, rayLength);
				bool noCardPosition = true;

				foreach (RaycastHit hit in hits)
				{
					if (hit.transform.GetComponent<CardPosition>() != null)
					{
						if (dropOff == null)
						{
							dropOff = hit.transform.GetComponent<CardPosition>();
							dropOff.LightOn();
						}

						noCardPosition = false;
					}
				}
				if (noCardPosition)
				{
					if (dropOff != null)
					{
						dropOff.LightOff();
					}
					dropOff = null;
				}
			}
		}
	}
}