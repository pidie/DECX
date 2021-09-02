using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardManager
{
	static class InstantiateCard
	{
		
	/// Contains methods related to moving and rendering Cards.
	///
	/// InitializeCard (Card card) : initializes the Card variables from its CardData
	/// DrawCardToScreen (Card card) : writes the Card variables to the TMP objects on the Card
	/// PlaceCardOnTableFromHand (Card card, CardPosition cardPosition) : Moves a Card from the Hand to a CardPosition
	
		public static void InitializeCard(Card card)
		{
			if (card.actionData != null)
			{
				card.title = card.actionData.title;
				card.ID = card.actionData.ID;
				card.energyCost = card.actionData.energyCost;
				card.description = card.actionData.description;
				card.image.texture = card.actionData.image;

				card.gameObject.name = card.title;
				card.healthPointsDisplay.SetActive(false);
				card.damageAmountDisplay.SetActive(false);

				card.creatureData = null;
			}
			else if (card.creatureData)
			{
				card.title = card.creatureData.title;
				card.ID = card.creatureData.ID;
				card.healthPoints = card.creatureData.healthPoints + card.healthPointModifier;
				card.damageAmount = card.creatureData.damageAmount + card.damageAmountModifier;
				card.description = card.creatureData.description;
				card.image.texture = card.creatureData.image;
                
				card.gameObject.name = card.title;
				card.healthPointsDisplay.SetActive(true);
				card.damageAmountDisplay.SetActive(true);
			}
		}
		
		public static void DrawCardToScreen(Card card)
		{
			card.Title.text = card.title;
			card.EnergyCost.text = card.energyCost.ToString();
			card.HealthPoints.text = card.healthPoints.ToString();
			card.DamageAmount.text = card.damageAmount.ToString();
			card.Description.text = card.ModifyTextForValue(card.description);
		}
		
		public static void PlaceCardOnTableFromHand(Card card, CardPosition cardPosition)
        {
        	Card newCard = MonoBehaviour.Instantiate(card, cardPosition.transform.position, Quaternion.Euler(90, 0, 180),
        		cardPosition.transform.parent.transform);
        	cardPosition.isOccupied = true;
        	newCard.PlayCard();
        	cardPosition.LightOff();
        }
	}

	static class ActivateCard
	{
		
	}
}