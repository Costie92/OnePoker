using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager _instance = null;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("GameManager is NULL");
            return _instance;
        }
    }

    public Cards cards;
    public Decks decks;
    public MessageData mData;
    public CardFlip cflip;
    public int P1;
    public int P2;
    int compareP1;
    int compareP2;
    int winner;
    Participant myself;
    List<Participant> participants;
    public bool isHost;
    bool reliability = false;
    int cardindex = 0;
    int aa = 0;
    // Use this for initialization
    private void Awake()
    {
        isHost = false;
        _instance = this;
    }
    void Start () {
        cards = this.GetComponent<Cards>();
        decks = GameObject.Find("PlayerDecks").GetComponent<Decks>();
        mData = this.GetComponent<MessageData>();
        cflip = GameObject.Find("axis").GetComponent<CardFlip>();
        if (SceneManager.GetActiveScene().name == "Main")
        {
            FindHost();
        }
    }

    // Update is called once per frame
    void Update () {

        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            isHost = true;
        }


        if (Input.GetMouseButtonDown(1))
        {
            if (isHost)
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    List<byte> newArray = new List<byte>();
                    newArray.Insert(0, 0xCB);
                    PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, newArray.ToArray());
                }
                if (Application.platform == RuntimePlatform.WindowsEditor) {
                    UIManager.instance.SetP2Bet(++aa);
                }
                mData.StartGame();
            }
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            SetFlip();
        }
    }

    public void FindHost() {

        myself = PlayGamesPlatform.Instance.RealTime.GetSelf();
        Debug.Log("My Participant ID is " + myself.ParticipantId);
        participants = PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
        isHost = (myself.ParticipantId == participants[0].ParticipantId);
        if (!isHost) {
            CardDist.instance.index++;
        }
        for (int i = 0; i < participants.Count; i++)
        {
            Debug.Log("All Participants ID " + i + " is " + participants[i].ParticipantId);
        }
        SetCardFromHost();
    }
    public void SetCardFromHost() {
        if (isHost)
        {
            reliability = true;
            string data = null;
            for (int i = 0; i < cards.cardIndex.Length; i++)
            {
                data += cards.cardIndex[i] + ",";
            }
            byte[] bytedata = System.Text.ASCIIEncoding.Default.GetBytes(data);
            List<byte> newArray = new List<byte>();
            newArray.AddRange(bytedata);
            newArray.Insert(0, 0xCA);
            Debug.Log("Send Message From Host");
            PlayGamesPlatform.Instance.RealTime.SendMessageToAll(reliability, newArray.ToArray());
            mData.SetCardIndex(data);
        }
        if (!isHost) {
            Debug.Log("WaitCard Dist");
            StartCoroutine(WaitSetCard());
        }
    }
    public void SetCardFromIndex() {
        if (!isHost)
        {
            Debug.Log("Set Card From Index");
            Debug.Log(mData.mCardIndex.Count);
            cards.Change(mData.mCardIndex);
        }
        if (decks.P1Deck.Count < 1)
        {
            StartCoroutine(GameStart());
        }
    }
    public void SetFlip() {
        cflip.SetSprite(cards.cardsprite[P1], cards.cardsprite[P2]);
        cflip.flip = true;
    }
    public void Compare() {
        compareP1 = P1 % 13;
        compareP2 = P2 % 13;
        if ((compareP1 == 0)) {
            if (compareP2 == 1)
            {
                LoseGame();
                return;
            }
            else if (compareP2 == 0)
            {
                DrawGame();
                return;
            }
            else {
                WinGame();
                return;
            }
        }
        if ((compareP2 == 0)) {
            if ((compareP1 == 1))
            {
                WinGame();
                return;
            }
            else
            {
                LoseGame();
                return;
            }
        }
        if ((compareP1) > (compareP2))
        {
            WinGame();
        }
        else if ((compareP1) == (compareP2))
        {
            DrawGame();
        }
        else {
            LoseGame();
        }
    }
    public void WinGame() {
        UIManager.instance.AfterWin();
        print("P1 Win");
        winner = 1;
        StartCoroutine(TurnStart());
    }
    public void LoseGame() {
        UIManager.instance.AfterLose();
        print("P2 Win");
        winner = -1;
        StartCoroutine(TurnStart());
    }
    public void DrawGame() {
        UIManager.instance.AfterDraw();
        print("Draw");
        winner = 0;
        StartCoroutine(TurnStart());
    }
    IEnumerator TurnStart() {
        compareP1 = P1 % 13;
        compareP2 = P2 % 13;
        UIManager.instance.RecordBoard((compareP1 + 1), (compareP2 + 1),winner);
        Debug.Log("Turn Start From Game Manager");
        yield return new WaitForSeconds(3f);
        if (UIManager.instance.p1life > 0)
        {
            if (UIManager.instance.p2life > 0)
            {
                mData.StartTurn();
            }
            else {
                StartCoroutine(P2GameOver());
            }
        }
        else {
            PlayGamesPlatform.Instance.RealTime.LeaveRoom();
            SceneManager.LoadScene(0);
        }
    }
    IEnumerator GameStart() {
        Debug.Log("Call Start Game From Game Manager");
        yield return new WaitForSeconds(1f);
        mData.StartGame();
    }
    IEnumerator P2GameOver() {
        Debug.Log("You Win");
        yield return new WaitForSeconds(2f);
        PlayGamesPlatform.Instance.RealTime.LeaveRoom();
        SceneManager.LoadScene(0);
    }

    IEnumerator WaitSetCard() {
        yield return new WaitForSeconds(4f);
        if (decks.P1Deck.Count < 1) {
            List<byte> newArray = new List<byte>();
            newArray.Insert(0, 0xDD);
            Debug.Log("Send Message To Host Card Dist");
            PlayGamesPlatform.Instance.RealTime.SendMessageToAll(reliability, newArray.ToArray());
        }
    }
}