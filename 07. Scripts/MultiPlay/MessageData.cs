using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageData : MonoBehaviour
{
    private static MessageData _instance = null;
    public static MessageData instance
    {
        get
        {
            if (_instance == null)
                _instance = new MessageData();
            return _instance;
        }
    }

    public List<int> mCardIndex = new List<int>();
    public int P2Bet = 1;

    public void SetCardIndex(string mMessage)
    {
        string[] parsingIndex = ParsingMessage(mMessage);
        for (int i = 0; i < parsingIndex.Length; i++)
        {
            if (parsingIndex[i] == "") continue;
            mCardIndex.Add(System.Int32.Parse(parsingIndex[i]));
        }
        Debug.Log("CardIndex Size = " + mCardIndex.Count);
        GameManager.instance.SetCardFromIndex();
    }
    public int GetCardIndex(int Index)
    {
        Debug.Log("GetCardIndex : from " + Index + " is : " + mCardIndex[Index]);
        if (mCardIndex.Count > Index)
        {
            return mCardIndex[Index];
        }
        else
            return -1;
    }
    public void EndTurn()
    {
        Debug.Log("EndofTurn");
        GameManager.instance.SetFlip();
    }
    public void StartGame() {
        StartTurn();
        StartCoroutine(OnStart());
    }
    public void StartTurn() {
        Debug.Log("Start Turn ");
        CardDist.instance.Create();
        UIManager.instance.SetP1Bet();
        UIManager.instance.OnStartGame();
        CardFlip.instance.ResetRotate();
        Debug.Log("Bet End ");
    }
    string[] ParsingMessage(string mMessage)
    {
        return mMessage.Split(',');
    }

    public void Player1Surrender() {

        CardFlip.instance.P1Surr();
        EndTurn();
    }
    public void Player2Surrender()
    {
        UIManager.instance.P2Surrender();
        CardFlip.instance.P2Surr();
        EndTurn();
    }
    IEnumerator OnStart() {
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Create One more When Start");
        CardDist.instance.Create();
    }
}