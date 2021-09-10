using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using DECX.EventManager;

public class Hand : MonoBehaviour
{
	public List<Card> cardsInHand;
	[Range(0, 12)]	public int maxCardsInHand;
	
	[CanBeNull] public CardPosition dropOff { get; private set; }

	public float cardWidth;
	[Range(0f, 5f)] public float widthBetweenCards;

	public bool handHasCompanions = false;
	public bool companionsPlaced = false;

	private void Awake()
	{
		maxCardsInHand = 4;
		dropOff = null;
		widthBetweenCards = 1.2f;
		cardWidth = 7f;
	}

	private void Update()
	{
		ArrangeCardsInHand();
		dropOff = HandEvents.HoldingCard(cardsInHand, dropOff);
		if (cardsInHand.Count < 1) { }
		else if (cardsInHand[0].creatureData != null && cardsInHand[0].creatureData.GetType() == typeof(
			CardData_Creature_Companion))
		{
			handHasCompanions = true;
		}

		if (handHasCompanions && cardsInHand.Count < 1)
		{
			companionsPlaced = true;
			handHasCompanions = false;
		}
	}

	private void ArrangeCardsInHand()
	{
		if (cardsInHand.Count > 0)
		{
			int n = cardsInHand.Count - 1;
			float xPosition = cardsInHand.Count > 1 ? ((cardWidth + widthBetweenCards) / 2) * n : 0f;
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
}