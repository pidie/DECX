using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using DECX.CardManager;
using DECX.UIManager;

// todo: figure out why clicks don't always register when on table
// todo: move some of the functionality to CardManager
// todo: RedAlertStandDown when clicking a card in a CardPosition that has placement restrictions
// todo: set up constructors and create new cards instead of recycling old cards with extra data

public class Card : MonoBehaviour
{
    [Header("Basic Info")]
    public string title;
    public string ID;
    public int energyCost;
    public int healthPoints;
    public int healthPointModifier;
    public int baseHealthPoints;
    public int damageAmount;
    public int damageAmountModifier;
    public int baseDamageAmount;
    
    [Header("Flavor Info")]
    public string description;

    [Header("Dev Info")]
    public CardData_Action actionData;
    [CanBeNull] public CardData_Creature creatureData;
    public bool initData;
    public bool isBeingHeld;
    [CanBeNull] public CardPosition placeOnTable;

    [Header("Display")]
    public TMP_Text Title;
    public TMP_Text EnergyCost;
    public TMP_Text HealthPoints;
    public TMP_Text DamageAmount;
    public TMP_Text Description;
    public RawImage image;

    public GameObject energyCostDisplay;
    public GameObject healthPointsDisplay;
    public GameObject damageAmountDisplay;

    private Card(CardData_Action data)
    {
        title = data.title;
        ID = data.ID;
        energyCost = data.energyCost;
        description = data.description;
        image.texture = data.imageTexture;

        gameObject.name = title;
        energyCostDisplay.SetActive(true);
        healthPointsDisplay.SetActive(false);
        damageAmountDisplay.SetActive(false);
    }

    private Card(CardData_Creature data)
    {
        title = data.title;
        ID = data.ID;
        baseHealthPoints = data.healthPoints;
        healthPointModifier = 0;
        baseDamageAmount = data.damageAmount;
        damageAmountModifier = 0;
        description = data.description;
        image.texture = data.imageTexture;
        
        healthPoints = healthPoints + healthPointModifier;
        damageAmount = baseDamageAmount + damageAmountModifier;
        
        gameObject.name = title;
        energyCostDisplay.SetActive(false);
        healthPointsDisplay.SetActive(true);
        damageAmountDisplay.SetActive(true);
    }
    
    private void Awake()
    {
        initData = false; isBeingHeld = false;
    }

    private void Update()
    {
        InstantiateCard.InitializeCard(this);
        CreatureCard.CheckVitals(this);
        InstantiateCard.DrawCardToScreen(this);
    }

    private void OnMouseDown()
    {
        ActivateCard.PaintValidCardPositions(this);
        
        // click the card from the hand
        if (transform.parent.GetComponent<Hand>())
        {
            isBeingHeld = true;
        }
        
        // click a card on the table
        else if (transform.parent.GetChild(0).GetComponent<CardPosition>())
        {
            if (transform.parent.GetChild(0).GetComponent<CardPosition>().cardInPosition == this.transform)
            {
                Debug.Log("hiiiiii");
                ActivateCard.RedAlertStandDown();
            }
            healthPoints -= 1;
        }
    }

    private void OnMouseUp()
    {
        ActivateCard.PaintValidCardPositions(this);
        Hand hand = GameObject.Find("Hand").GetComponent<Hand>();
        if (placeOnTable == null)
        {
            placeOnTable = hand.dropOff;
        }

        if (isBeingHeld == true)
        {
            isBeingHeld = false;
            
            if (placeOnTable == null) { }
            else if (placeOnTable != hand.dropOff)
            {
                Debug.Log("bleh");
            }
            else if (placeOnTable.isOccupied)
            {
                UIErrorMessage.DisplayErrorMessage(GameError.CardPositionNotEmpty);
            }
            else if (placeOnTable != hand.dropOff || (placeOnTable != null && !placeOnTable.isOccupied))
            {
                PlayerManager player = GameObject.Find("Player").GetComponent<PlayerManager>();

                if (player.currentEnergyPoints < energyCost)
                {
                    UIErrorMessage.DisplayErrorMessage(GameError.PlayerNotEnoughEnergy);
                } 
                else
                {
                    player.currentEnergyPoints -= energyCost;
                    ActivateCard.PlaceCard(this, placeOnTable);
                    hand.cardsInHand.Remove(this);
                    Destroy(gameObject);
                }
            }
            
            ActivateCard.RedAlertStandDown();
            placeOnTable = null;
        }
    }
}
