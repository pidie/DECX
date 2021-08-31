using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Basic Info")]
    public string title;
    public string ID;
    public int energyCost;

    [Header("Flavor Info")]
    public string description;

    [Header("Dev Info")]
    public CardData cardData;
    private bool initData;
    public bool isBeingHeld;

    public TMP_Text Title;
    public TMP_Text EnergyCost;
    public TMP_Text Description;

    private void Awake()
    {
        initData = false; isBeingHeld = false;
    }

    private void Update()
    {
        if (!initData)
        {
            // initializes the card
            if (cardData)
            {
                this.title = cardData.title;
                this.ID = cardData.ID;
                this.energyCost = cardData.energyCost;
                this.description = cardData.description;

                this.gameObject.name = title;
            }
            initData = true;
        }
        
        DrawCardToScreen();
    }

    private void OnMouseDown()
    {
        isBeingHeld = true;
    }

    private void OnMouseUp()
    {
        Hand hand = GameObject.Find("Hand").GetComponent<Hand>();
        
        isBeingHeld = false;
        if (hand.dropOff != null)
        {
            hand.dropOff.PlaceCardOnTableFromHand(this);
            Destroy(gameObject);
        }
    }

    private void DrawCardToScreen()
    {
        Title.text = title;
        EnergyCost.text = energyCost.ToString();
        Description.text = ModifyTextForValue(description);
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
}
