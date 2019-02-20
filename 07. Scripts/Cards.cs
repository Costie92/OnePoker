using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class Cards : MonoBehaviour
{
    
    [System.Serializable]
    public class Card
    {
        public CardSuit CardSuit { get; private set; }
        public CardValue CardValue { get; private set; }
        public int CardIndex { get; private set; }
        public Sprite CardSprite { get; private set; }
        public Card(CardSuit cardSuit, CardValue cardValue, int cardIndex, Sprite cardsprite)
        {
            CardSuit = cardSuit;
            CardValue = cardValue;
            CardIndex = cardIndex;
            CardSprite = cardsprite;
        }
    };

    private static Cards _instance = null;
    public static Cards instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("Cards is NULL");
            return _instance;
        }
    }
    public List<Card> PlayCard;
    public string[] cardstring;
    public int[] cardIndex;
    public Sprite[] cardsprite;
    // Use this for initialization
    public List<Card> DeckCreate()
    {
        cardsprite = Resources.LoadAll<Sprite>("Trump Cards");
        List<Card> ThreeDecks = new List<Card>();
        for (int i = 0; i < 3; i++)
        {
            for (int cardsuit = 0; cardsuit < (int)CardSuit.eMax; cardsuit++)
            {
                for (int cardvalue = 1; cardvalue < (int)CardValue.eMax; cardvalue++)
                {
                    int index = (int)cardsuit * 13 + cardvalue;
                    Card cards = new Card((CardSuit)cardsuit, (CardValue)cardvalue, index, cardsprite[index - 1]);
                    ThreeDecks.Add(cards);
                    //52번작동
                }
            }
        }
        return ThreeDecks;
    }
    public List<Card> DeckShuffle(List<Card> Deck)
    {
        List<Card> ShuffledDeck = new List<Card>();
        int randomindex = 0;
        int deckcount = (int)(Deck.Count * 0.5);
        while (Deck.Count > deckcount)
        {
            randomindex = UnityEngine.Random.Range(0, Deck.Count);
            ShuffledDeck.Add(Deck[randomindex]);
            Deck.RemoveAt(randomindex);
        }
        return ShuffledDeck;
    }

    void Awake()
    {
        _instance = this;
        
        List<Card> NormalDeck = DeckCreate();
        PlayCard = DeckShuffle(NormalDeck);
        cardstring = new string[PlayCard.Count];
        cardIndex = new int[PlayCard.Count];
        for (int i = 0; i < PlayCard.Count; i++)
        {
            cardIndex[i] = PlayCard[i].CardIndex - 1;
            cardstring[i] = "Card_"+PlayCard[i].CardSuit + "_" + PlayCard[i].CardValue;
        }
        
    }
    public void Change(List<int> mIndex) {
        PlayCard.Clear();
        for (int i = 0; i < mIndex.Count; i++) {
            Card cards = new Card((CardSuit)(mIndex[i] / 13) + 1, (CardValue)(mIndex[i] % 13), mIndex[i] + 1, cardsprite[mIndex[i]]);
            PlayCard.Add(cards);
        }
    }
    // Update is called once per frame
}