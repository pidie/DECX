using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<CardData_Action> cardDatas;
    public GameObject cardPrefab;
    public Button drawCardButton;
    public TMP_Text CardsInStock;

    private void Awake()
    {
        Button btn = drawCardButton.GetComponent<Button>();
        btn.onClick.AddListener(CreateNewCard);

        cardDatas = ShuffleCards(cardDatas);
    }

    private void Update()
    {
        CardsInStock.text = cardDatas.Count.ToString();
    }

    public void CreateNewCard()
    {
        Hand hand = GameObject.Find("Hand").GetComponent<Hand>();

        if (hand.cardsInHand.Count >= hand.maxCardsInHand)
        {
            Debug.Log($"Cannot have more than {hand.maxCardsInHand} cards in Hand.");
            return;
        }
        else if (cardDatas.Count < 1)
        {
            Debug.LogWarning("deck is empty");
            return;
        }
        else
        {
            
        }
        
        Card c = CardManager.InstantiateCard.CreateNewCard(hand.transform, cardDatas[0]);
        
        cardDatas.Remove(cardDatas[0]);
        hand.cardsInHand.Add(c);
    }
    
    private List<CardData_Action> ShuffleCards(List<CardData_Action> cards)
    {
        cards = cards.OrderBy(a => Guid.NewGuid()).ToList();
        return cards;
    }
}
