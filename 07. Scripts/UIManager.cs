using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using System;

public class UIManager : MonoBehaviour {

    private static UIManager _instance = null;
    public static UIManager instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("UIManager is NULL");
            return _instance;
        }
    }
    public bool P2Surr;
    public bool BetLife;
    public BettingLifes bettingLifes;
    public int p1bet;
    public int p1life;
    public int p2bet;
    public int p2life;

    public bool selectedcard;
    public SpriteRenderer P1SetCardBack;
    public SpriteRenderer P2SetCardBack;
    public Sprite CardBack;

    public int deckindex = 0;
    public GameObject P1Called;
    public GameObject P2Called;
    public bool SetAllPlayer;
    public GameObject OpLeft;
    public GameObject Setting;

    bool Board;
    bool CanMove;

    int BoardPage;
    public GameObject[] Pages;
    public GameObject BtnNext;
    public GameObject BtnPrev;

    public GameObject[] P1Board;
    public GameObject[] P2Board;

    public int GameRound;

    public Button callbutton;
    public GameObject Rule;
    public GameObject Tutorial;

    public Text Concede;
    public Text Quit;
    public Text Resume;

    private void Awake()
    {
        _instance = this;
    }
    // Use this for initialization
    void Start() {
        P2Surr = false;
        CanMove = true;
        Board = false;
        BetLife = false;
        selectedcard = false;
        BoardPage = 0;
        GameRound = 0;
        p1life = p2life = 10;
        p1bet = p2bet = 0;
        bettingLifes = this.GetComponent<BettingLifes>();
        Pages = new GameObject[3];
        P1Board = new GameObject[30];
        P2Board = new GameObject[30];
        for (int i = 0; i < 3; i++) {
            string temp = "Page" + (i+1);
            Pages[i] = GameObject.Find(temp);
            for (int j = 0; j < 10; j++)
            {
                P1Board[i * 10 + j] = Pages[i].transform.GetChild(1).GetChild(j + 1).gameObject;
                P2Board[i * 10 + j] = Pages[i].transform.GetChild(2).GetChild(j + 1).gameObject;
            }
            if (i > 0) {
                Pages[i].SetActive(false);
            }
        }
        callbutton = GameObject.Find("Call").GetComponent<Button>();
        Rule = GameObject.Find("Rule");
        BtnNext = GameObject.Find("Next");
        BtnPrev = GameObject.Find("Prev");
        BtnPrev.SetActive(false);
        Setting = GameObject.Find("Setting");
        OpLeft = GameObject.Find("OpLeft");
        P1Called = GameObject.Find("p1Called");
        P2Called = GameObject.Find("p2Called");
        P1SetCardBack = GameObject.Find("axis").transform.GetChild(0).GetComponent<SpriteRenderer>();
        P2SetCardBack = GameObject.Find("axis").transform.GetChild(1).GetComponent<SpriteRenderer>();
        OpLeft.SetActive(false);
        Setting.SetActive(false);
        Rule.SetActive(false);
        SetBtnLan();
        if (SceneManager.GetActiveScene().name == "Tutorial") {
            OnStartGame();
        }
    }
    void SetBtnLan()
    {
        string sysLang = Application.systemLanguage.ToString();


        switch (sysLang)
        {
            case "Korean":
                if (SceneManager.GetActiveScene().name == "Main")
                {
                    Concede.text = "게임항복";
                }
                Quit.text = "앱  종료";
                Resume.text = "게임재개";
                break;
            case "Japanese":
                if (SceneManager.GetActiveScene().name == "Main") {
                    Concede.text = "降伏";
                }
                Quit.text = "終了";
                Resume.text = "再開";
                break;
            default:
                break;
        }
    }
    // Update is called once per frame
    void Update() {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                OnClickSetup();
            }
        }
    }
    public void P2Surrender() {
        P2Surr = true;
    }
    public void Bet(Button button) {
        if (!P2Surr)
        {
            if (button.name == "Life")
            {
                BetLife = !BetLife;
                if (BetLife)
                {
                    bettingLifes.Selected();
                }
                else
                {
                    bettingLifes.UnSelected();
                }
            }
            if (button.name == "Bet")
            {
                if (BetLife && p1life > 0)
                {
                    SetP1Bet();
                    if (SceneManager.GetActiveScene().name == "Tutorial")
                    {
                        Tutorials.instance.Tuto4Call();
                        P2Called.SetActive(true);
                    }
                }
            }
        }
    }
    
    public void SelectCard(Button button)
    {
        if (P1SetCardBack.sprite == null)
        {
            selectedcard = true;
            if (button.name == "Deck1")
            {
                Decks.instance.P1Card1Selected();
                deckindex = 0;
            }
            else
            {
                Decks.instance.P1Card2Selected();
                deckindex = 1;
            }
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                Tutorials.instance.Tuto2Set();
            }
        }
    }
    
    public void SetCard() {
        if (selectedcard) {
            P1SetCardBack.sprite = CardBack;
            int compare = Decks.instance.P1Deck[deckindex];
            Decks.instance.SetSelectCard();
            Debug.Log(compare);
            GameManager.instance.P1 = compare;
            if (Application.platform == RuntimePlatform.Android)
            {
                List<byte> newArray = new List<byte>();
                newArray.Add(Convert.ToByte(compare));
                newArray.Insert(0, 0xCE);
                PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, newArray.ToArray());
            }
            Decks.instance.P1Deck.RemoveAt(deckindex);
            selectedcard = false;
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                SetP2Card();
                SetP2Bet(2);
                if (GameRound == 1)
                {
                    Tutorials.instance.Tuto3Bet();
                }
                else {
                    Tutorials.instance.Tuto5Surrender();
                }
            }
        }
    }
    public void SetP2Card()
    {
        P2SetCardBack.sprite = CardBack;
        Decks.instance.P2Card2.sprite = null;
    }
    public void Call(Button btn) {
        SetAllPlayer = (P1SetCardBack.sprite != null && P2SetCardBack.sprite != null);
        if ((p1bet == p2bet) && SetAllPlayer) {
            P1Called.SetActive(true);
            StartCoroutine(BtnInteractable(btn,GameRound));
            if (Application.platform == RuntimePlatform.Android)
            {
                List<byte> newArray = new List<byte>();
                newArray.Insert(0, 0xCB);
                PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, newArray.ToArray());
            }
        }
        if(P2Called.active)
            MessageData.instance.EndTurn();
    }
    IEnumerator BtnInteractable(Button btn,int round) {
        btn.interactable = false;
        yield return new WaitUntil(() => round < GameRound);
        btn.interactable = true;
    }
    public void OnStartGame() {
        P2Surr = false;
        GameRound++;
        P1SetCardBack.sprite = null;
        P2SetCardBack.sprite = null;
        SetDeactive();
        if (SceneManager.GetActiveScene().name == "Tutorial") {
            if (GameRound == 2)
            {
                SetP2Bet(1);
                Tutorials.instance.Tuto1Select();
            }
            if (GameRound == 3) {
                SetP2Bet(1);
                Tutorials.instance.Tuto6OpenBoard();
            }
        }
    }

    public void RecordBoard(int P1, int P2, int Win) {
        Debug.Log("P1 : " + P1 + ", P2 : " + P2);
        int index = GameRound - 1;
        switch (P1) {
            case 1:
                P1Board[index].GetComponentInChildren<Text>().text = "A";
                break;
            case 11:
                P1Board[index].GetComponentInChildren<Text>().text = "J";
                break;
            case 12:
                P1Board[index].GetComponentInChildren<Text>().text = "Q";
                break;
            case 13:
                P1Board[index].GetComponentInChildren<Text>().text = "K";
                break;
            default:
                P1Board[index].GetComponentInChildren<Text>().text = P1.ToString();
                break;
        }
        switch (P2) {
            case 1:
                P2Board[index].GetComponentInChildren<Text>().text = "A";
                break;
            case 11:
                P2Board[index].GetComponentInChildren<Text>().text = "J";
                break;
            case 12:
                P2Board[index].GetComponentInChildren<Text>().text = "Q";
                break;
            case 13:
                P2Board[index].GetComponentInChildren<Text>().text = "K";
                break;
            default:
                P2Board[index].GetComponentInChildren<Text>().text = P2.ToString();
                break;
        }
        switch (Win) {
            case 1:
                P1Board[index].GetComponent<Image>().color = Color.red;
                break;
            case -1:
                P2Board[index].GetComponent<Image>().color = Color.red;
                break;
            default:
                P1Board[index].GetComponent<Image>().color = Color.red;
                P2Board[index].GetComponent<Image>().color = Color.red;
                break;
        }
        
    }
    public void SetDeactive() {
        callbutton.interactable = true;
        P1Called.SetActive(false);
        P2Called.SetActive(false);
    }
    public void Surrender(Button btn) {
        SetAllPlayer = (P1SetCardBack.sprite != null && P2SetCardBack.sprite != null);
        if (SetAllPlayer) {
            StartCoroutine(BtnInteractable(btn, GameRound));
            if (Application.platform == RuntimePlatform.Android)
            {
                List<byte> newArray = new List<byte>();
                newArray.Insert(0, 0xCF);
                PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, newArray.ToArray());
            }
            MessageData.instance.Player1Surrender();
        }
    }
    public void OnClickRule() {
        Rule.SetActive(true);
    }
    public void OnCloseRule() {
        Rule.SetActive(false);
    }
    public void OnClickSetup() {
        Setting.SetActive(true);
    }

    public void OnClickQuit() {
        PlayGamesPlatform.Instance.RealTime.LeaveRoom();
        Application.Quit();
    }

    public void OnClickResume() {
        Setting.SetActive(false);
    }

    public void OnClickConcede() {
        PlayGamesPlatform.Instance.RealTime.LeaveRoom();
        SceneManager.LoadScene(0);
    }
    public void OnClickLobby()
    {
        SceneManager.LoadScene(0);
    }

    public void OnClickNext() {
        BoardPage++;
        PageControl();
    }
    public void OnClickPrev()
    {
        BoardPage--;
        PageControl();
    }
    public void PageControl() {
        switch (BoardPage)
        {
            case 0:
                BtnPrev.SetActive(false);
                Pages[0].SetActive(true);
                Pages[1].SetActive(false);
                break;

            case 1:
                BtnNext.SetActive(true);
                BtnPrev.SetActive(true);
                Pages[0].SetActive(false);
                Pages[1].SetActive(true);
                Pages[2].SetActive(false);
                break;

            case 2:
                BtnNext.SetActive(false);
                Pages[1].SetActive(false);
                Pages[2].SetActive(true);
                break;

            default:
                break;
        }
    }

    public void OnClickBoard(GameObject obj) {

        if (CanMove)
        {
            Board = !Board;
            CanMove = false;
            if (!Board)
            {
                if (SceneManager.GetActiveScene().name == "Tutorial")
                {
                    Tutorials.instance.TutoSteps[6].transform.GetChild(2).gameObject.SetActive(false);
                }
                LeanTween.moveX(obj, obj.transform.position.x + 1200, 0.5f).setOnComplete(Complete);
            }
            else
            {
                if (SceneManager.GetActiveScene().name == "Tutorial")
                {
                    Tutorials.instance.TutoSteps[5].transform.GetChild(2).gameObject.SetActive(false);
                }
                LeanTween.moveX(obj, obj.transform.position.x - 1200, 0.5f).setOnComplete(Complete);
            }
        }
    }
    void Complete() {
        CanMove = true;

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            if (Board)
            {
                Tutorials.instance.Tuto7CloseBoard();
            }
            else
            {
                Tutorials.instance.Tuto8End();
            }
        }
    }

    public void SetP1Bet()
    {
        if (p1bet < p2bet + p2life && p1life > 0)
        {
            SetDeactive();
            p1bet++;
            p1life--;
            bettingLifes.SetP1Bet(p1bet);
            bettingLifes.SetP1Life(p1life);
            BetLife = false;

            if (Application.platform == RuntimePlatform.Android)
            {
                if (SceneManager.GetActiveScene().name == "Main")
                {
                    List<byte> newArray = new List<byte>();
                    newArray.Insert(0, 0xCC);
                    newArray.Add((byte)p1bet);
                    PlayGamesPlatform.Instance.RealTime.SendMessageToAll(true, newArray.ToArray());
                }
            }
        }
    }
    public void SetP2Bet(int bet)
    {
        SetDeactive();
        p2bet = bet;
        p2life--;
        bettingLifes.SetP2Bet(p2bet);
        bettingLifes.SetP2Life(p2life);
    }
    public void AfterWin() {
        p1life += p1bet + p2bet;
        p1bet = 0;
        p2bet = 0;
        bettingLifes.SetP1Bet(p1bet);
        bettingLifes.SetP2Bet(p2bet);
        bettingLifes.SetP1Life(p1life);
    }
    public void AfterLose() {
        p2life += p1bet + p2bet;
        p1bet = 0;
        p2bet = 0;
        bettingLifes.SetP1Bet(p1bet);
        bettingLifes.SetP2Bet(p2bet);
        bettingLifes.SetP2Life(p2life);
    }
    public void AfterDraw() {
        p1life += p1bet;
        p2life += p2bet;
        p1bet = 0;
        p2bet = 0;
        bettingLifes.SetP1Bet(p1bet);
        bettingLifes.SetP2Bet(p2bet);
        bettingLifes.SetP1Life(p1life);
        bettingLifes.SetP2Life(p2life);
        
    }
}
