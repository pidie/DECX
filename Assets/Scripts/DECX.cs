using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DECX
{
	namespace CardManager
	{
		public static class InstantiateCard
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
				if (!card.initData)
				{
					if (card.actionData != null)
					{
						card.title = card.actionData.title;
						card.ID = card.actionData.ID;
						card.energyCost = card.actionData.energyCost;
						card.description = card.actionData.description;
						card.image.texture = card.actionData.imageTexture;

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
						card.image.texture = card.creatureData.imageTexture;

						card.gameObject.name = card.title;
						card.healthPointsDisplay.SetActive(true);
						card.damageAmountDisplay.SetActive(true);
					}

					card.initData = true;
				}
			}

			// todo: move this to EventManager
			public static void DrawCardToScreen(Card card)
			{
				card.Title.text = card.title;
				card.EnergyCost.text = card.energyCost.ToString();
				card.HealthPoints.text = card.healthPoints.ToString();
				card.DamageAmount.text = card.damageAmount.ToString();
				card.Description.text = DynamicText.Rewrite(card.description, card);
			}
		}

		enum dtTag
		{
			X_DMG,
			X_HP_,
			X_HPM
		}
		public static class DynamicText
		{
			/// using regex-esque codification, this allows predefined strings to be dynamically altered in game.
			private static string DecodeTag(dtTag tag, Card card)
			{
				switch (tag)
				{
					case dtTag.X_DMG:
						return card.damageAmount.ToString();
					case dtTag.X_HP_:
						return card.healthPoints.ToString();
					case dtTag.X_HPM:
						return card.healthPointModifier.ToString();
					default:
						return null;
				}
			}

			public static string Rewrite(string text, Card card)
			{
				string marker = "#!";
				if (text.Contains(marker))
				{
					string tagString = text.Substring(text.IndexOf(marker) + 2, 5);
					foreach (dtTag t in Enum.GetValues(typeof(dtTag)))
					{
						if (t.ToString() == tagString)
						{
							text.Replace(tagString, DecodeTag(t, card));
							break;
						}
					}
				}
				return text;
			}
		}

		// todo: disolve this class - it has no purpose. Most of these methods are events anyway
		public static class ActivateCard
		{
			public static void PlaceCard(Card card, CardPosition cardPosition)
			{
				Card newCard = MonoBehaviour.Instantiate(card, cardPosition.transform.position,
					Quaternion.Euler(90, 0, 180),
					cardPosition.transform.parent.transform);
				cardPosition.isOccupied = true;
				ActivateCard.PlayCard(newCard);
				cardPosition.Lights(false);
				cardPosition.cardInPosition = card;
			}

			public static void PlayCard(Card card)
			{
				if (card.actionData != null && card.actionData.summonCreature)
				{
					card.creatureData = card.actionData.creatureSummoned;
					card.healthPointModifier = card.actionData.modifyHealth;
					card.damageAmountModifier = card.actionData.modifyDamage;
					card.actionData = null;
				}
				else
				{

				}
			}

			public static void PaintValidCardPositions(Card card)
			{
				foreach (CardPosition cardPosition in GameObject.FindObjectsOfType(typeof(CardPosition)))
				{
					if (!CanBePlaced(card, cardPosition))
					{
						cardPosition.RedAlert(true);
					}
					else
					{
						cardPosition.RedAlert(false);
					}
				}
			}

			// todo: tidy and expand on conditions herein
			public static bool CanBePlaced(Card card, CardPosition cardPosition)
			{
				if (card.creatureData != null)
				{
					if (card.creatureData.isLockedBack && card.creatureData.isLockedFront)
					{
						return false;
					}
					else if ((card.creatureData.isLockedFront && cardPosition.IsFrontLine()) ||
					         (card.creatureData.isLockedBack && !cardPosition.IsFrontLine()) ||
					         (!card.creatureData.isLockedBack && !card.creatureData.isLockedFront))
					{
						return true;
					}

				}
				else if (card.actionData)
				{
					return true;
				}

				return false;
			}

			public static void RedAlertStandDown()
			{
				foreach (CardPosition cp in GameObject.FindObjectsOfType(typeof(CardPosition)))
				{
					cp.RedAlert(false);
				}
			}
		}

		public static class ActionCard
		{
			
		}
		
		public static class CreatureCard
		{
			public static void CheckVitals(Card card)
			{
				if (card.creatureData)
				{
					if (card.healthPoints < 1)
					{
						card.placeOnTable.isOccupied = false;
						MonoBehaviour.Destroy(card.gameObject);
					}
				}
			}
		}
	}
	
    namespace UIManager
    {
	    public static class HUD
	    {
		    // todo: make the button click magic happen here, and the instantiation happen in InstantiateCard
		    public static void DrawCard(Button button, Hand hand)
		    {
			    
		    }

		    public static void FadeErrorMessages(TMP_Text errorMessage)
		    {
			    ///	if the message is not blank, drop alpha to 0 over 2 seconds.
			    if (errorMessage.text != "")
			    {
				    float initialAlpha = errorMessage.alpha;
				    while (errorMessage.alpha > 0)
				    {
					    // errorMessage.alpha = Mathf.Lerp(1, 0, errorMessage.alpha);
					    // errorMessage.color = new Color(color.r, color.g, color.b, Mathf.Lerp(1, 0, alpha));
					    errorMessage.alpha -= 2.0f * Time.deltaTime;
				    }

				    errorMessage.text = "";
				    errorMessage.alpha = initialAlpha;
			    }
		    }
	    }
	    
	    enum GameError
	    {
		    CardPositionNotEmpty,
		    PlayerDeckIsEmpty,
		    PlayerNotEnoughEnergy,
		    PlayerTooManyCardsInHand
	    }
    	static class UIErrorMessage
    	{
    		public static void DisplayErrorMessage(GameError message, float time = 2.0f)
    		{
    			string msg = CompileErrorMessage(message);
    			TMP_Text errorMessageBox = GameObject.Find("ErrorMessage").GetComponent<TMP_Text>();
    			float alpha = errorMessageBox.alpha;
    			errorMessageBox.text = msg;
    		}
    
    		private static string CompileErrorMessage(GameError message)
    		{
    			switch (message)
    			{
    				case GameError.CardPositionNotEmpty:
    					return "Cannot play this card here";
    				case GameError.PlayerNotEnoughEnergy:
    					return "You do not have enough energy to play this card";
                    case GameError.PlayerTooManyCardsInHand:
	                    return "Cannot have more than !#X:CARDS_IN_HAND cards in your hand at once";
                    case GameError.PlayerDeckIsEmpty:
	                    return "No cards left in deck";
    				default:
    					return "default_error_message";
    			}
    		}
    	}
    }

    namespace EventManager
    {
	    public static class HandEvents
	    {
		    public static CardPosition HoldingCard(List<Card> cards, CardPosition cardPosition)
		    {
			    foreach (Card card in cards)
			    {
				    if (card.isBeingHeld)
				    {
					    Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
						    Camera.main.WorldToScreenPoint(card.transform.position).z - 0.5f);
					    card.transform.position = Camera.main.ScreenToWorldPoint(pos);

					    float rayLength = 80f;

					    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					    Debug.DrawRay(ray.origin, ray.direction * rayLength);

					    RaycastHit[] hits;
					    hits = Physics.RaycastAll(ray, rayLength);
					    bool noCardPosition = true;

					    foreach (RaycastHit hit in hits)
					    {
						    CardPosition hitPosition = hit.transform.GetComponent<CardPosition>();
						    if (hitPosition != null)
						    {
							    if (cardPosition == null && CardManager.ActivateCard.CanBePlaced(card, hitPosition))
							    {
								    cardPosition = hitPosition;
								    cardPosition.Lights(true);
							    }

							    noCardPosition = false;
						    }
					    }

					    if (noCardPosition)
					    {
						    if (cardPosition != null)
						    {
							    cardPosition.Lights(false);
						    }

						    cardPosition = null;
					    }
				    }
			    }

			    return cardPosition;
		    }
		    
		    [CanBeNull]
		    public static Card AddCardToHand(List<CardData_Action> deck, Hand hand)
		    {
			    if (hand.cardsInHand.Count >= hand.maxCardsInHand)
			    {
				    UIManager.UIErrorMessage.DisplayErrorMessage(UIManager.GameError.PlayerTooManyCardsInHand);
			    }
			    else if (deck.Count < 1)
			    {
				    UIManager.UIErrorMessage.DisplayErrorMessage(UIManager.GameError.PlayerDeckIsEmpty);
			    }
			    else
			    {
				    return CardManager.InstantiateCard.CreateNewCard(hand.transform, deck[0]);
			    }

			    return null;
		    }
		    
		    [CanBeNull]
		    public static Card AddCardToHand(List<CardData_Creature> deck, Hand hand)
		    {
			    if (hand.cardsInHand.Count >= hand.maxCardsInHand)
			    {
				    UIManager.UIErrorMessage.DisplayErrorMessage(UIManager.GameError.PlayerTooManyCardsInHand);
			    }
			    else if (deck.Count < 1)
			    {
				    UIManager.UIErrorMessage.DisplayErrorMessage(UIManager.GameError.PlayerDeckIsEmpty);
			    }
			    else
			    {
				    return CardManager.InstantiateCard.CreateNewCard(hand.transform, deck[0]);
			    }

			    return null;
		    }
	    }
    }
}