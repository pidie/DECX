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
// todo: move constructor logic to Awake method. Might have to make three different Card subtypes - Card_Action, Card_Creature, Card_Item, with abstract Card class

public class Card : MonoBehaviour
{
    [Header("Basic Info")]
    public string title;
    public string ID;
    
    [Header("ActionData Info")]
    public int energyCost;
    public int cooldownPeriod;
    public int range;
    
    [Header("CreatureData Info")]
    public int healthPoints;
    public int healthPointModifier;
    public int baseHealthPoints;
    public int damageAmount;
    public int damageAmountModifier;
    public int baseDamageAmount;
    
    [Header("Flavor Info")]
    public string description;

    [Header("Dev Info")]
    [CanBeNull] public CardData_Action actionData;
    [CanBeNull] public CardData_Creature creatureData;
    public bool initData;   // todo: remove this
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
        // basic info
        title = data.title;
        ID = data.ID;
        
        // action data
        energyCost = data.energyCost;
        cooldownPeriod = data.cooldownPeriod;
        range = data.range;
        
        // creature data
        healthPoints = null;
        healthPointsModifier = null;
        baseHealthPoints = null;
        damageAmount = null;
        damageAmountModifier = null;
        baseDamageAmount = null;
        
        // flavor info
        description = data.description;
        
        // dev info
        actionData = data;
        creatureData = null;
        isBeingHeld = false;
        placeOnTable = null;
        
        // display
        Title.text = title;
        EnergyCost.text = energyCost.ToString();
        baseHealthPoints.text = healthPoints.ToString();
        DamageAmount = damageAmount.ToString();
        Description = description;
        image.texture = data.imageTexture;

        // misc
        gameObject.name = title;
        energyCostDisplay.SetActive(true);
        healthPointsDisplay.SetActive(false);
        damageAmountDisplay.SetActive(false);
    }

    private Card(CardData_Creature data)
    {
        // basic info
        title = data.title;
        ID = data.ID;

        // action data
        energyCost = null;
        cooldownPeriod = null;
        range = null;
        
        // creature data
        baseHealthPoints = data.healthPoints;
        healthPointModifier = data.healthPointModifier;
        healthPoints = baseHealthPoints + healthPointModifier;
        baseDamageAmount = data.damageAmount;
        damageAmountModifier = data.damageAmountModifier;
        damageAmount = baseDamageAmount + damageAmountModifier;
        
        // flavor info
        description = data.description;
        
        // dev info
        actionData = null;
        creatureData = data;
        isBeingHeld = false;
        placeOnTable = null;

        // display
        Title.text = title;
        EnergyCost.text = energyCost.ToString();
        baseHealthPoints.text = healthPoints.ToString();
        DamageAmount = damageAmount.ToString();
        Description = description;
        image.texture = data.imageTexture;

        // misc
        gameObject.name = title;
        energyCostDisplay.SetActive(false);
        healthPointsDisplay.SetActive(true);
        damageAmountDisplay.SetActive(true);
    }
    
    private void Awake()
    {
        // determine the type of CardData that is stored
        // var cardData;
        // if cardData.typeof.CardData_Action:
        // initialize values accordingly
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
