using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorials : MonoBehaviour {

    private static Tutorials _instance = null;
    public static Tutorials instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("Tutorials is NULL");
            return _instance;
        }
    }

    public GameObject panel;
    public GameObject TutoCanvas;
    public GameObject[] TutoSteps;
    public Text Lobby;
    public int Page;
    string sysLan;
	// Use this for initialization
	void Start () {
        _instance = this;
        
        Page = 0;
        TutoSteps = new GameObject[8];
        for (int i = 0; i < 8; i++)
        {
            TutoSteps[i] = GameObject.Find("Steps").transform.GetChild(i).gameObject;
            if (i > 0) {
                TutoSteps[i].SetActive(false);
            }
        }
        
        TutoCanvas.SetActive(false);
        
        StartCoroutine(TutoStart());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator TutoStart() {
        yield return new WaitForSeconds(1.5f);
        CardDist.instance.Create();
        yield return new WaitForSeconds(1.5f);
        CardDist.instance.Create();
        yield return new WaitForSeconds(2f);
        
        UIManager.instance.SetP1Bet();
        UIManager.instance.SetP2Bet(1);
        Decks.instance.SetP2Panel(1, 1);
        panel.SetActive(false);
        sysLan = MultiManager.instance.sysLanguage;
        SetLan(sysLan);
        TutoCanvas.SetActive(true);
    }
    public void SetLan(string sys) {
        switch (sys) {
            case "Korean":
                TutoSteps[0].transform.GetChild(0).GetComponent<Text>().text = "내 덱에서 카드를 선택해주세요";
                TutoSteps[1].transform.GetChild(0).GetComponent<Text>().text = "선택한 카드를 제출해주세요";
                TutoSteps[2].transform.GetChild(0).GetComponent<Text>().text = "상대방의 칩만큼 배팅해주세요";
                TutoSteps[3].transform.GetChild(0).GetComponent<Text>().text = "양 플레이어가 콜하면 턴이 끝납니다";
                TutoSteps[4].transform.GetChild(0).GetComponent<Text>().text = "양 플레이어가 카드를 제출하면 항복할 수 있습니다";
                TutoSteps[5].transform.GetChild(0).GetComponent<Text>().text = "매 라운드 경기 결과는 점수판에 기록됩니다";
                TutoSteps[6].transform.GetChild(0).GetComponent<Text>().text = "한번 더 누르면 점수판이 닫힙니다";
                TutoSteps[7].transform.GetChild(0).GetComponent<Text>().text = "이제 로비로 돌아갑시다";
                Lobby.text = "로비";
                break;
            case "Japanese":
                TutoSteps[0].transform.GetChild(0).GetComponent<Text>().text = "私のデッキからカードを選んでください";
                TutoSteps[1].transform.GetChild(0).GetComponent<Text>().text = "選択したカードを提出してください";
                TutoSteps[2].transform.GetChild(0).GetComponent<Text>().text = "相手のチップほどバッティングしてください";
                TutoSteps[3].transform.GetChild(0).GetComponent<Text>().text = "両プレーヤーがコールするとターンが終わります";
                TutoSteps[4].transform.GetChild(0).GetComponent<Text>().text = "両プレーヤーがカードを提出すれば降伏できます";
                TutoSteps[5].transform.GetChild(0).GetComponent<Text>().text = "毎ラウンドの結果は点数版に記録されます";
                TutoSteps[6].transform.GetChild(0).GetComponent<Text>().text = "もう一度押すと点数版が下がります";
                TutoSteps[7].transform.GetChild(0).GetComponent<Text>().text = "もうロビーへ帰りましょう";
                Lobby.text = "ロビー";
                break;
            default:
                break;
        }
    }
    public void Tuto1Select() {
        
        TutoSteps[0].SetActive(true);
        TutoSteps[3].SetActive(false);
    }
    public void Tuto2Set()
    {
        TutoSteps[0].SetActive(false);
        TutoSteps[1].SetActive(true);
    }
    public void Tuto3Bet()
    {
        TutoSteps[1].SetActive(false);
        TutoSteps[2].SetActive(true);
    }
    public void Tuto4Call()
    {
        TutoSteps[2].SetActive(false);
        TutoSteps[3].SetActive(true);
    }
    public void Tuto5Surrender() {
        TutoSteps[1].SetActive(false);
        TutoSteps[4].SetActive(true);
    }
    public void Tuto6OpenBoard() {
        TutoSteps[4].SetActive(false);
        TutoSteps[5].SetActive(true);
    }
    public void Tuto7CloseBoard() {
        TutoSteps[5].SetActive(false);
        TutoSteps[6].SetActive(true);
    }
    public void Tuto8End() {
        TutoSteps[6].SetActive(false);
        TutoSteps[7].SetActive(true);
    }
}