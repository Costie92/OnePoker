using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using UnityEngine.SceneManagement;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;


public class MultiManager: MonoBehaviour, RealTimeMultiplayerListener {

    GameObject player;
    public GameObject prefab;
    MessageData mData;
    public static MultiManager instance;
    public static int target;
    bool canStartPlaying = true;
    bool playingGame = false;
    public GameObject Invite;
    public GameObject SInvite;
    public GameObject AutoMatch;
    public GameObject Return;
    public GameObject Leave;
    public GameObject Loading;

    Invitation inv;
    public string sysLanguage;
    private void Awake()
    {
        sysLanguage = Application.systemLanguage.ToString();
        Screen.SetResolution(1920, 1080, false);
        target = 60;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = target;

        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    // Use this for initialization
    void Start () {

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //DontDestroyOnLoad(this);
        FindAdd();
        ActiveFalse();
        Authenicate();
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        if (arg1.name == "InviteScene") {
            FindAdd();
            ActiveFalse();
            Authenicate();
        }
        string currentName = arg0.name;

        if (currentName == null)
        {
            // Scene1 has been removed
            currentName = "Replaced";
        }

        Debug.Log("Scenes: " + currentName + ", " + arg1.name);
    }

    // Update is called once per frame
    void Update () {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (SceneManager.GetActiveScene().name == "InviteScene")
            {
                if (Input.GetKey(KeyCode.Escape))
                {
                    Application.Quit();
                }
            }
        }

        /*
                bool reliability = false;
                string data = player.transform.position.x + ":" + player.transform.position.y + ":" + player.transform.position.z;
                byte[] bytedata = System.Text.ASCIIEncoding.Default.GetBytes(data);
                PlayGamesPlatform.Instance.RealTime.SendMessageToAll(reliability, bytedata);
           */

    }
    void FindAdd()
    {
        Invite = GameObject.Find("Invite");
        SInvite = GameObject.Find("ShowInvite");
        AutoMatch = GameObject.Find("AutoMatch");
        Return = GameObject.Find("Return");
        Leave = GameObject.Find("Leave");
        Loading = GameObject.Find("Loading");

        Invite.GetComponent<Button>().onClick.AddListener(() =>
        { InviteFriend(); });
        SInvite.GetComponent<Button>().onClick.AddListener(() =>
        { ShowInvite(); });
        AutoMatch.GetComponent<Button>().onClick.AddListener(() =>
        { CreateGame(); });
        Return.GetComponent<Button>().onClick.AddListener(() =>
        { ShowWait(); });
        Leave.GetComponent<Button>().onClick.AddListener(() =>
        { LeaveRoom(); });

        SetLan();
    }
    void ActiveFalse() {
        
        Invite.SetActive(false);
        SInvite.SetActive(false);
        AutoMatch.SetActive(false);
        Return.SetActive(false);
        Leave.SetActive(false);
        Loading.SetActive(true);
    }
    void SetLan() {
        switch (sysLanguage)
        {
            case "Korean":
                Invite.GetComponentInChildren<Text>().text = "초대";
                SInvite.GetComponentInChildren<Text>().text = "초대\n보기";
                AutoMatch.GetComponentInChildren<Text>().text = "자동\n매칭";
                Return.GetComponentInChildren<Text>().text = "돌아\n가기";
                Leave.GetComponentInChildren<Text>().text = "나가기";
                break;
            case "Japanese":
                Invite.GetComponentInChildren<Text>().text = "招待";
                SInvite.GetComponentInChildren<Text>().text = "招待\n確認";
                AutoMatch.GetComponentInChildren<Text>().text = "自動\nマッチング";
                AutoMatch.GetComponentInChildren<Text>().fontSize = 40;
                Return.GetComponentInChildren<Text>().text = "帰る";
                Leave.GetComponentInChildren<Text>().text = "出る";
                break;
            default:
                break;
        }
    }
    public void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            Debug.Log("Pause :" + paused);
        }
        else
        {
            
            Debug.Log("Focus :" + paused);
            if (SceneManager.GetActiveScene().name == "Main")
                LeaveRoom();
        }
    }

    /*
    void OnGUI()
    {

        if (playingGame)
        {
            // don't show the buttons during game play.
            return;
        }
        if (canStartPlaying)
        {
            if (GUI.Button(new Rect(10, Screen.height / 4f, Screen.height / 8f, Screen.width / 3f), "Play"))
            {
                canStartPlaying = false;

                ulong bitmask = 0L; // no special bitmask for players
                uint minOpponents = 1; // so the smallest is a 2 player game
                uint maxOpponents = 1; // so the largest is a 2 player game
                PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen(minOpponents, maxOpponents, 0, this);
                //PlayGamesPlatform.Instance.RealTime.CreateQuickGame(minOpponents,maxOpponents, 0, bitmask, this);

            }
        }
        else
        {
            if (GUI.Button(new Rect(10, Screen.height / 4f, Screen.height / 12f, 100), "Leave Room"))
            {
                PlayGamesPlatform.Instance.RealTime.LeaveRoom();
            }
            if (GUI.Button(new Rect(350, Screen.height / 4f, Screen.height / 12f, 100), "Show Waiting Room"))
            {
                PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
            }
        }
    }
    */

    void Authenicate()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);

        PlayGamesPlatform.DebugLogEnabled = true;

        PlayGamesPlatform.Activate();

        PlayGamesPlatform.Instance.Authenticate((bool success) =>
        {
            if (success)
            {
                Invite.SetActive(true);
                SInvite.SetActive(true);
                AutoMatch.SetActive(true);
                Loading.SetActive(false);
                Debug.Log("Success Authenicate");
                PlayGamesPlatform.Instance.RealTime.LeaveRoom();
            }
            else
            {
                Debug.Log("fail");
            }

        });

    }


    public void SignIn()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated() == false)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().WithInvitationDelegate(OnInvitationReceived).Build();
            PlayGamesPlatform.InitializeInstance(config);

            PlayGamesPlatform.DebugLogEnabled = true;

            PlayGamesPlatform.Activate();
            PlayGamesPlatform.Instance.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log("Success SignIn");
                }
                else
                {
                    Debug.Log("fail");
                }

            });
        }
    }

    public void SignOut()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated() == true)
        {
            PlayGamesPlatform.Instance.SignOut();
        }
    }


    public void CreateGame()
    {
        Invite.SetActive(false);
        SInvite.SetActive(false);
        AutoMatch.SetActive(false);
        Loading.SetActive(true);
        const int MinOpponents = 1, MaxOpponents = 1;
        const int GameVariant = 0;
        Debug.Log("CreateGame() called...");
        PlayGamesPlatform.Instance.RealTime.CreateQuickGame(MinOpponents, MaxOpponents, GameVariant, this);
    }

    public void InviteFriend()
    {
        Invite.SetActive(false);
        SInvite.SetActive(false);
        AutoMatch.SetActive(false);
        Loading.SetActive(true);
        const int MinOpponents = 1, MaxOpponents = 1;
        const int GameVariant = 0;
        Debug.Log("InviteFriend() called...");
        PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen(MinOpponents, MaxOpponents, GameVariant, this);
    }

    public void ShowInvite()
    {
        Invite.SetActive(false);
        SInvite.SetActive(false);
        AutoMatch.SetActive(false);
        Loading.SetActive(true);
        PlayGamesPlatform.Instance.RealTime.AcceptFromInbox(this);
    }

    public void OnInvitation(Invitation invitation, bool shouldAutoAccept)
    {
        // handle the invitation
        if (shouldAutoAccept)
        {
            PlayGamesPlatform.Instance.RealTime.AcceptInvitation(invitation.InvitationId, this);
        }
        else
        {
            // do some other stuff.
        }
    }
    public void DoGetAllInvitations() {
        
    }
    public void OnInvitationReceived(Invitation invitation, bool shouldAutoAccept)
    {
        StartCoroutine(InvitationCo(invitation, shouldAutoAccept));
    }
    Invitation mIncomingInvitation;
    IEnumerator InvitationCo(Invitation invitation, bool shouldAutoAccept)
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().rootCount == 1);

        Debug.Log("DebugM | Invitation has been received!!!");
        //StartCoroutine(LM.LoadingAnim());
        if (shouldAutoAccept)
        {
            Debug.Log("DebugM | Should auto accept: TRUE");
            PlayGamesPlatform.Instance.RealTime.AcceptInvitation(invitation.InvitationId, instance);

        }
        else
        {
            // The user has not yet indicated that they want to accept this invitation.
            // We should *not* automatically accept it. Rather we store it and 
            // display an in-game popup:
            Debug.Log("DebugM | Should auto accept: FALSE");

            //Lobby LM = FindObjectOfType<Lobby>();
            //LM.invPanel.SetActive(true);

            mIncomingInvitation = invitation;
        }
    }

    public void AcceptGoogleInv(GameObject panel)
    {
        if (mIncomingInvitation != null)
        {
            // show the popup
            //string who = (mIncomingInvitation.Inviter != null &&
            //    mIncomingInvitation.Inviter.DisplayName != null) ?
            //        mIncomingInvitation.Inviter.DisplayName : "Someone";
            Debug.Log("DebugM | Invitation has been accepted");
            PlayGamesPlatform.Instance.RealTime.AcceptInvitation(mIncomingInvitation.InvitationId, instance);
            panel.SetActive(false);
        }
    }

    public void achievement()
    {
        Social.ShowAchievementsUI();
    }

    #region RealTimeMultiplayerListener implementation

    private bool isRoomSetuped = false;

    public void OnRoomSetupProgress(float percent)
    {
        Debug.Log(percent);
        if (!isRoomSetuped)
        {

            isRoomSetuped = true;
            Debug.Log(percent);
            PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
            Return.SetActive(true);
            Leave.SetActive(true);
            Loading.SetActive(false);
        }
    }
    public void ShowWait() {
        PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
    }
    public void CreateQuickRoom()
    {
        PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen(1, 1, 1, instance);
    }
    public void LeaveRoom() {
        PlayGamesPlatform.Instance.RealTime.LeaveRoom();
    }
    public void OnRoomConnected(bool success)
    {
        if (success)
        {
            playingGame = success;
            SceneManager.LoadScene(1);
            isRoomSetuped = true;
            Debug.Log("ssssssssssssssss");
            //player = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            //player.name = PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId;
            bool reliability = true;
            byte[] bytesy = new byte[1];
            PlayGamesPlatform.Instance.RealTime.SendMessageToAll(reliability, bytesy);
        }
        else
        {
            PlayGamesPlatform.Instance.RealTime.LeaveRoom();
            Debug.Log("fffff");
            isRoomSetuped = false;
            //CreateGame();
        }
    }

    public void OnLeftRoom()
    {
        if (SceneManager.GetActiveScene().name == "InviteScene")
        {
            Return.SetActive(false);
            Leave.SetActive(false);
            Invite.SetActive(true);
            SInvite.SetActive(true);
            AutoMatch.SetActive(true);
            Loading.SetActive(false);
        }
        Debug.Log("Left the room");
        canStartPlaying = true;
        isRoomSetuped = false;
    }


    public void OnPeersConnected(string[] participantIds)
    {
        List<Participant> playerscount = PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
        if (playerscount != null && playerscount.Count > 1)//this condition should be decided by you.
        {
            //Start the game here
            SceneManager.LoadScene(1);
        }
    }

    public void OnPeersDisconnected(string[] participantIds)
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            UIManager.instance.OpLeft.SetActive(true);
            StartCoroutine(GameEnd());
            Debug.Log("Other Player OnPeersDisconnected");
        }
    }
    IEnumerator GameEnd() {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }
    public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
    {
        Debug.Log("data lenght : " + data.Length);
        print("Scene Name " + SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "Main")
        {
            mData = GameObject.Find("GameManager").GetComponent<MessageData>();
            byte b = data[0];
            switch (b)
            {
                // SetCardList
                case 0xCA:
                    Debug.Log("aa");
                    byte[] data2 = new byte[data.Length - 1];
                    for (int i = 0; i < data.Length - 1; i++)
                    {
                        data2[i] = data[i + 1];
                    }
                    string decoded = System.Text.ASCIIEncoding.Default.GetString(data2);
                    Debug.Log(decoded);
                    mData.SetCardIndex(decoded);
                    break;

                // Call Player2
                case 0xCB:
                    UIManager.instance.P2Called.SetActive(true);
                    if (UIManager.instance.P1Called.active)
                        mData.EndTurn();
                    break;
                // Bet Player2
                case 0xCC:
                    UIManager.instance.SetP2Bet(data[1]);
                    break;

                // Set Player2 Panel
                case 0xCD:
                    Decks.instance.SetP2Panel(data[1], data[2]);
                    break;
                // Player2 Set Card
                case 0xCE:
                    GameManager.instance.P2 = data[1];
                    UIManager.instance.SetP2Card();
                    break;
                // Player2 Surrender Game
                case 0xCF:
                    mData.Player2Surrender();
                    break;

                case 0xDD:
                    GameManager.instance.SetCardFromHost();
                    break;

                default:
                    Debug.Log("UnDefined Data : " + b);
                    break;

            }
        }
    }

    public void OnParticipantLeft(Participant participant)
    {
        Debug.Log("OnParticipantLeft");
    }
    #endregion
}
