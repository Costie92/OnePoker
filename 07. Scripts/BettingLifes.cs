using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BettingLifes : MonoBehaviour {

    public GameObject P1L;
    public GameObject P2L;
    public GameObject P1B;
    public GameObject P2B;
    public GameObject[] P1Lifes;
    public GameObject[] P2Lifes;
    public GameObject[] P1Bets;
    public GameObject[] P2Bets;
    public Material WhiteKing;
    public Material BlackKing;
    int Active = 0;
    // Use this for initialization
    void Start()
    {
        P1L = GameObject.Find("Player1Lifes");
        P1B = GameObject.Find("Player1Bets");
        P2L = GameObject.Find("Player2Lifes");
        P2B = GameObject.Find("Player2Bets");
        int lifeindex = P1L.transform.GetChildCount();
        int betindex = P1B.transform.GetChildCount();
        P1Lifes = new GameObject[lifeindex];
        P2Lifes = new GameObject[lifeindex];
        for (int i = 0; i < lifeindex; i++)
        {
            P1Lifes[i] = P1L.transform.GetChild(i).gameObject;
            P2Lifes[i] = P2L.transform.GetChild(i).gameObject;
        }
        P1Bets = new GameObject[betindex];
        P2Bets = new GameObject[betindex];
        for (int i = 0; i < betindex; i++)
        {
            P1Bets[i] = P1B.transform.GetChild(i).gameObject;
            P2Bets[i] = P2B.transform.GetChild(i).gameObject;
        }

        SetP1Life(10);
        SetP2Life(10);
        SetP1Bet(0);
        SetP2Bet(0);
    }
    public void SetP1Life(int count) {
        UnSelected();
        for (int i = 0; i < P1Lifes.Length; i++)
        {
            if (i < count)
                P1Lifes[i].SetActive(true);
            else
                P1Lifes[i].SetActive(false);
        }
    }
    public void SetP2Life(int count)
    {
        for (int i = 0; i < P2Lifes.Length; i++)
        {
            if (i < count)
                P2Lifes[i].SetActive(true);
            else
                P2Lifes[i].SetActive(false);
        }
    }
    public void SetP1Bet(int count)
    {
        for (int i = 0; i < P1Bets.Length; i++)
        {
            if (i < count)
                P1Bets[i].SetActive(true);
            else
                P1Bets[i].SetActive(false);
        }
    }
    public void SetP2Bet(int count)
    {
        for (int i = 0; i < P2Bets.Length; i++)
        {
            if (i < count)
                P2Bets[i].SetActive(true);
            else
                P2Bets[i].SetActive(false);
        }
    }
    public void Selected()
    {
        
        for (int i = 0; i < P1Lifes.Length; i++)
        {
            if (P1Lifes[i].active)
            {
                Active = i;
            }
        }
        P1Lifes[Active].GetComponent<MeshRenderer>().material = BlackKing;
    }
    public void UnSelected() {
        P1Lifes[Active].GetComponent<MeshRenderer>().material = WhiteKing;
    }
}
