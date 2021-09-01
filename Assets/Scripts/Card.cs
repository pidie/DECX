using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("Basic Info")]
    public string title;
    public string ID;
    public int energyCost;
    public int healthPoints;
    public int healthPointModifier;
    public int damageAmount;
    public int damageAmountModifier;

    [Header("Flavor Info")]
    public string description;

    [Header("Dev Info")]
    public CardData_Action actionData;
    [CanBeNull] public CardData_Creature creatureData;
    private bool initData;
    public bool isBeingHeld;

    public TMP_Text Title;
    public TMP_Text EnergyCost;
    public TMP_Text HealthPoints;
    public TMP_Text DamageAmount;
    public TMP_Text Description;

    public GameObject energyCostDisplay;
    public GameObject healthPointsDisplay;
    public GameObject damageAmountDisplay;

    private void Awake()
    {
        initData = false; isBeingHeld = false;
    }

    private void Update()
    {
        if (!initData)
        {
            if (actionData != null)
            {
                this.title = actionData.title;
                this.ID = actionData.ID;
                this.energyCost = actionData.energyCost;
                this.description = actionData.description;

                this.gameObject.name = title;
                healthPointsDisplay.SetActive(false);
                damageAmountDisplay.SetActive(false);

                creatureData = null;
            }
            else if (creatureData)
            {
                this.title = creatureData.title;
                this.ID = creatureData.ID;
                this.healthPoints = creatureData.healthPoints + healthPointModifier;
                this.damageAmount = creatureData.damageAmount + damageAmountModifier;
                this.description = creatureData.description;
                
                this.gameObject.name = title;
                healthPointsDisplay.SetActive(true);
                damageAmountDisplay.SetActive(true);
            }
            initData = true;
        }

        if (creatureData)
        {
            CheckVitals();
        }
        
        DrawCardToScreen();
    }

    private void OnMouseDown()
    {
        // click the card from the hand
        if (transform.parent.GetComponent<Hand>())
        {
            isBeingHeld = true;
        }
        
        // click a card on the table
        else if (transform.parent.GetChild(0).GetComponent<CardPosition>())
        {
            healthPoints -= 1;
        }
    }

    private void OnMouseUp()
    {
        Hand hand = GameObject.Find("Hand").GetComponent<Hand>();

        if (isBeingHeld == true)
        {
            isBeingHeld = false;
            if (hand.dropOff != null)
            {
                hand.dropOff.PlaceCardOnTableFromHand(this);
                hand.cardsInHand.Remove(this);
                Destroy(gameObject);
            }
        }
    }

    private void DrawCardToScreen()
    {
        Title.text = title;
        EnergyCost.text = energyCost.ToString();
        HealthPoints.text = healthPoints.ToString();
        DamageAmount.text = damageAmount.ToString();
        Description.text = ModifyTextForValue(description);
    }
    
    public void PlayCard()
    {
        if (actionData.summonCreature)
        {
            creatureData = actionData.creatureSummoned;
            healthPointModifier = actionData.modifyHealth;
            actionData = null;
        }
    }

    // rename this - maybe expand on it so it's its own class?
    private string ModifyTextForValue(string description)
    {
        string exception = "#!X:DMG";
        
        if (description.Contains(exception))
        {
            description = description.Replace(exception, "5 Fire");
        }
        return description;
    }

    private void CheckVitals()
    {
        if (healthPoints < 1)
        {
            Destroy(this.gameObject);
        }
    }
}
