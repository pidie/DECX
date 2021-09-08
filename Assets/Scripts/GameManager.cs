using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DECX.UIManager;

public class GameManager : MonoBehaviour
{
    public List<CardData_Action> cardDatas;
    public List<CardData_Creature_Companion> companions;
    public int numOfCompanions;
    public int maxNumOfCompanions;
    public GameObject cardPrefab;
    public Button drawCardButton;
    public TMP_Text CardsInStock;
    private Hand hand;

    private void Awake()
    {
        drawCardButton.onClick.AddListener(CreateNewCard);
        numOfCompanions = companions.Count;
        hand = GameObject.Find("Hand").GetComponent<Hand>();

        cardDatas = ShuffleCards(cardDatas);
    }

    private void Start()
    {
        SpawnCompanions();
    }

    private void Update()
    {
        if (hand.companionsPlaced)
        {
            drawCardButton.interactable = true;
            CardsInStock.text = $"Cards Left: {cardDatas.Count.ToString()}";
        }
        else
        {
            CardsInStock.text = "Place your Companions";
        }
    }

    public void CreateNewCard()
    {
        if (hand.cardsInHand.Count >= hand.maxCardsInHand)
        {
            UIErrorMessage.DisplayErrorMessage(DECX.GameError.PlayerTooManyCardsInHand);
            return;
        }
        else if (cardDatas.Count < 1)
        {
            UIErrorMessage.DisplayErrorMessage(DECX.GameError.PlayerDeckIsEmpty);
            return;
        }
        
        Card c = CardManager.InstantiateCard.CreateNewCard(hand.transform, cardDatas[0]);
        
        cardDatas.Remove(cardDatas[0]);
        hand.cardsInHand.Add(c);
    }

    private void SpawnCompanions()
    {
        drawCardButton.interactable = false;
        TMP_Text messageBox = GameObject.Find("MessageBox").GetComponent<TMP_Text>();

        List<Card> cards = new List<Card>();

        foreach (CardData_Creature_Companion companion in companions)
        {
            Card card = CardManager.InstantiateCard.CreateNewCard(hand.transform, companion);
            hand.cardsInHand.Add(card);
        }
        companions.Clear();
    }
    
    private List<CardData_Action> ShuffleCards(List<CardData_Action> cards)
    {
        cards = cards.OrderBy(a => Guid.NewGuid()).ToList();
        return cards;
    }
}
