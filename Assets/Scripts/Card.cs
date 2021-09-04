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
// todo: move some of the functionality to CardManager
// todo: if attempting to place a card on a non-empty CardPosition, all card positions are now locked

public class Card : MonoBehaviour
{
    [Header("Basic Info")]
    public string title;
    public string ID;
    public int energyCost;
    public int healthPoints;    public int healthPointModifier;
    public int damageAmount;    public int damageAmountModifier;

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
        CardManager.ActivateCard.PaintValidCardPositions(this);
        
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
        CardManager.ActivateCard.PaintValidCardPositions(this);
        Hand hand = GameObject.Find("Hand").GetComponent<Hand>();
        if (placeOnTable == null)
        {
            placeOnTable = hand.dropOff;
        }

        if (isBeingHeld == true)
        {
            isBeingHeld = false;
            if (placeOnTable == null) { }
            else if (placeOnTable != null && !placeOnTable.isOccupied)
            {
                PlayerManager player = GameObject.Find("Player").GetComponent<PlayerManager>();

                if (player.currentEnergyPoints < energyCost)
                {
                    Debug.LogWarning("You do not have enough energy to play this card.");
                }
                else
                {
                    player.currentEnergyPoints -= energyCost;
                    InstantiateCard.PlaceCardOnTableFromHand(this, placeOnTable);
                    hand.cardsInHand.Remove(this);
                    Destroy(gameObject);
                }
            }
            else if (placeOnTable.isOccupied)
            {
                Debug.Log(placeOnTable.name);
                Debug.LogWarning($"A card ({placeOnTable.cardInPosition.title}) is already here.");
            }
        }
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
