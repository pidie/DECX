using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
using CardManager;

// todo: figure out why clicks don't always register when on table
// todo: move some of the functionality to CardCreator
// todo: sometimes the current card from Hand is being detected in CardPosition when trying to place it

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
    [CanBeNull] private CardPosition placeOnTable;

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

    private void Awake()
    {
        initData = false; isBeingHeld = false;
    }

    private void Update()
    {
        if (!initData)
        {
            InstantiateCard.InitializeCard(this);
            initData = true;
        }

        if (creatureData)
        {
            CheckVitals();
        }
        
        InstantiateCard.DrawCardToScreen(this);
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
        if (placeOnTable == null)
        {
            placeOnTable = hand.dropOff;
        }

        if (isBeingHeld == true)
        {
            isBeingHeld = false;
            if (placeOnTable == null)
            {
                // do nothing
            }
            else if (placeOnTable != null && !placeOnTable.isOccupied)
            {
                InstantiateCard.PlaceCardOnTableFromHand(this, placeOnTable);
                hand.cardsInHand.Remove(this);
                Destroy(gameObject);
            }
            else if (placeOnTable.isOccupied)
            {
                Debug.LogWarning($"A card ({title}) is already here.");
            }
        }
    }
    
    public void PlayCard()
    {
        if (actionData.summonCreature)
        {
            creatureData = actionData.creatureSummoned;
            healthPointModifier = actionData.modifyHealth;
            damageAmountModifier = actionData.modifyDamage;
            actionData = null;
        }
    }

    // rename this - maybe expand on it so it's its own class?
    public string ModifyTextForValue(string description)
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
            placeOnTable.isOccupied = false;
            Destroy(this.gameObject);
        }
    }
}
