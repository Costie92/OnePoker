using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SceneManagement;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using System;

public class Decks : MonoBehaviour
{
    private static Decks _instance = null;
    public static Decks instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("Decks is NULL");
            return _instance;
        }
    }
    public Sprite CardBack;
    public SpriteRenderer P1Card1;
    public SpriteRenderer P1Card2;
    public SpriteRenderer P2Card1;
    public SpriteRenderer P2Card2;
    public List<int> P1Deck;
    public List<int> P2Deck;
    public Sprite LightOn;
    public Sprite LightOff;
    public Image[] P1Panel;
    public Image[] P2Panel;
    public Vector3 card1tr;
    public Vector3 card2tr;
    // Use this for initialization
    void Start()
    {
        GameObject Player1Panel = GameObject.Find("P1Panels");
        GameObject Player2Panel = GameObject.Find("P2Panels");
        P1Panel = new Image[4];
        P2Panel = new Image[4];
        for (int i = 0; i < 4; i++) {
            P1Panel[i] = Player1Panel.transform.GetChild(i).GetComponent<Image>();
            P2Panel[i] = Player2Panel.transform.GetChild(i).GetComponent<Image>();
        }
        P1Card1 = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        P1Card2 = this.transform.GetChild(1).GetComponent<SpriteRenderer>();
        P2Card1 = this.transform.GetChild(2).GetComponent<SpriteRenderer>();
        P2Card2 = this.transform.GetChild(3).GetComponent<SpriteRenderer>();
        card1tr = P1Card1.transform.position;
        card2tr = P1Card2.transform.position;
        _instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        if (P1Deck.Count > 1)
        {
            P1Card2.sprite = Cards.instance.cardsprite[P1Deck[1]];
        }
        else if (P1Deck.Count == 1)
        {
            P1Card1.sprite = Cards.instance.cardsprite[P1Deck[0]];
            P1Card2.sprite = null;
        }
        

    }
    public void SetDeck(int CardIndex) {
        P1Deck.Add(CardIndex);
        if (P1Deck.Count > 1) {
            SetP1Panel();
        }
    }
    public void SetP1Panel() {
        bool deck1 = ((P1Deck[0] % 13) > 6 || ((P1Deck[0] %13) == 0));
        bool deck2 = ((P1Deck[1] % 13) > 6 || ((P1Deck[1] % 13) == 0));
        if (deck1 && deck2)
        {
            P1Panel[0].sprite = P1Panel[1].sprite = LightOn;
            P1Panel[2].sprite = P1Panel[3].sprite = LightOff;
        }
        else if (deck1 || deck2)
        {
            P1Panel[0].sprite = P1Panel[2].sprite = LightOn;
            P1Panel[1].sprite = P1Panel[3].sprite = LightOff;
        }
        else
        {
            P1Panel[2].sprite = P1Panel[3].sprite = LightOn;
            P1Panel[0].sprite = P1Panel[1].sprite = LightOff;
        }
        if (SceneManager.GetActiveScene().name == "Main")
        {
            byte[] bytedata = { Convert.ToByte(deck1), Convert.ToByte(deck2) };
            List<byte> newArray = new List<byte>();
            newArray.AddRange(bytedata);
            newArray.Insert(0, 0xCD);
            PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, newArray.ToArray());
        }
    }
    public void SetP2Panel(byte data1, byte data2) {
        bool p2pdata1 = Convert.ToBoolean(data1);
        bool p2pdata2 = Convert.ToBoolean(data2);
        if(p2pdata1 && p2pdata2)
        {
            P2Panel[0].sprite = P2Panel[1].sprite = LightOn;
            P2Panel[2].sprite = P2Panel[3].sprite = LightOff;
        }
        else if (p2pdata1 || p2pdata2)
        {
            P2Panel[0].sprite = P2Panel[2].sprite = LightOn;
            P2Panel[1].sprite = P2Panel[3].sprite = LightOff;
        }
        else
        {
            P2Panel[2].sprite = P2Panel[3].sprite = LightOn;
            P2Panel[0].sprite = P2Panel[1].sprite = LightOff;
        }
    }

    public void SetBack() {
        if (P2Card1.sprite == null)
        {
            P2Card1.sprite = CardBack;
        }
        else
            P2Card2.sprite = CardBack;
    }
    public void P1Card1Selected() {
        Vector3 Selected = card1tr + new Vector3(0, 0.5f, 0);
        P1Card2.transform.position = card2tr;
        P1Card1.transform.position = Selected;
    }
    public void P1Card2Selected() {
        Vector3 Selected = card2tr + new Vector3(0, 0.5f, 0);
        P1Card1.transform.position = card1tr;
        P1Card2.transform.position = Selected;
    }
    public void SetSelectCard() {
        P1Card1.transform.position = card1tr;
        P1Card2.transform.position = card2tr;
    }
    public void InsertCardPrefabs() {

        //TrumpCards = new GameObject[52];
        //int deckindex = 0;

        /*
        string[] arr1 = Directory.GetFiles(Directory.GetCurrentDirectory() + "/Assets/Prefabs/Hearts", "*.prefab", SearchOption.AllDirectories);
        string[] arr2 = Directory.GetFiles(Directory.GetCurrentDirectory() + "/Assets/Prefabs/Diamonds", "*.prefab", SearchOption.AllDirectories);
        string[] arr3 = Directory.GetFiles(Directory.GetCurrentDirectory() + "/Assets/Prefabs/Clubs", "*.prefab", SearchOption.AllDirectories);
        string[] arr4 = Directory.GetFiles(Directory.GetCurrentDirectory() + "/Assets/Prefabs/Spades", "*.prefab", SearchOption.AllDirectories);
        
        foreach (string FullPath in arr1)
        {
            if (!FullPath.Contains(".meta"))
            {

                int index1 = FullPath.IndexOf("Assets/Prefabs/");
                string path = FullPath.Substring(index1);
                path = path.Replace("\\", "/");
#if UNITY_EDITOR
                TrumpCards[deckindex++] = AssetDatabase.LoadAssetAtPath<GameObject>(path);
#endif
            }
        }
        foreach (string FullPath in arr2)
        {
            if (!FullPath.Contains(".meta"))
            {

                int index1 = FullPath.IndexOf("Assets/Prefabs/");
                string path = FullPath.Substring(index1);
                path = path.Replace("\\", "/");
#if UNITY_EDITOR
                TrumpCards[deckindex++] = AssetDatabase.LoadAssetAtPath<GameObject>(path);
#endif
            }
        }
        foreach (string FullPath in arr3)
        {
            if (!FullPath.Contains(".meta"))
            {

                int index2 = FullPath.IndexOf("Assets/Prefabs/");
                string path = FullPath.Substring(index2);
                path = path.Replace("\\", "/");
#if UNITY_EDITOR
                TrumpCards[deckindex++] = AssetDatabase.LoadAssetAtPath<GameObject>(path);
#endif
            }
        }
        foreach (string FullPath in arr4)
        {
            if (!FullPath.Contains(".meta"))
            {

                int index3 = FullPath.IndexOf("Assets/Prefabs/");
                string path = FullPath.Substring(index3);
                path = path.Replace("\\", "/");
#if UNITY_EDITOR
                TrumpCards[deckindex++] = AssetDatabase.LoadAssetAtPath<GameObject>(path);
#endif
            }
        }
        */


    }
}