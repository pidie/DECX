using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public List<CardData> cardDatas;
    public List<Card> cards;
    public GameObject cardPrefab;
    public Random rand;

    // public Hand hand;

    public Button drawCardButton;
    public TMP_Text CardsInStock;

    private void Awake()
    {
        Button btn = drawCardButton.GetComponent<Button>();
        btn.onClick.AddListener(CreateNewCard);

        cardDatas = ShuffleCards(cardDatas);

        rand = new Random();
    }

    private void Update()
    {
        CardsInStock.text = cardDatas.Count.ToString();
    }

    public void CreateNewCard()
    {
        Hand hand = GameObject.Find("Hand").GetComponent<Hand>();
        
        if (cardDatas.Count < 1)
        {
            Debug.LogWarning("deck is empty");
            return;
        }
        GameObject newCard = Instantiate(cardPrefab, gameObject.transform);
        Card c = newCard.GetComponent<Card>();
        c.cardData = cardDatas[0];
        newCard.transform.parent = hand.transform;
        
        cardDatas.Remove(cardDatas[0]);
        hand.cardsInHand.Add(c);
    }
    
    private List<CardData> ShuffleCards(List<CardData> cards)
    {
        cards = cards.OrderBy(a => Guid.NewGuid()).ToList();
        return cards;
    }
}
