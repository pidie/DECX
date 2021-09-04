using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardManager
{
	static class InstantiateCard
	{
		public static Card CreateNewCard(Transform parent, CardData_Action cardData)
		{
			GameObject cardPrefab = Resources.Load<GameObject>("Prefabs/Card");
			GameObject newCard = MonoBehaviour.Instantiate(cardPrefab, parent);
			Card card = newCard.GetComponent<Card>();

			card.actionData = cardData;
			return card;
		}
		
		public static Card CreateNewCard(Transform parent, CardData_Creature cardData)
		{
			GameObject cardPrefab = Resources.Load<GameObject>("Prefabs/Card");
			GameObject newCard = MonoBehaviour.Instantiate(cardPrefab, parent);
			Card card = newCard.GetComponent<Card>();

			card.creatureData = cardData;
			return card;
		}
	
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
			card.Description.text = ActivateCard.ModifyTextForValue(card.description);
		}
		
		public static void PlaceCardOnTableFromHand(Card card, CardPosition cardPosition)
        {
        	Card newCard = MonoBehaviour.Instantiate(card, cardPosition.transform.position, Quaternion.Euler(90, 0, 180),
        		cardPosition.transform.parent.transform);
        	cardPosition.isOccupied = true;
        	ActivateCard.PlayCard(newCard);
        	cardPosition.LightOff();
        }
	}

	static class ActivateCard
	{
		public static string ModifyTextForValue(string description)
		{
			string exception = "#!X:DMG";
        
			if (description.Contains(exception))
			{
				description = description.Replace(exception, "5 Fire");
			}
			return description;
		}
		
		public static void PlayCard(Card card)
		{
			if (card.actionData.summonCreature)
			{
				card.creatureData = card.actionData.creatureSummoned;
				card.healthPointModifier = card.actionData.modifyHealth;
				card.damageAmountModifier = card.actionData.modifyDamage;
				card.actionData = null;
			}
		}
	}
}