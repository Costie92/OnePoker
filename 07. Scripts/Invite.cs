using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Invite : MonoBehaviour
{

    public GameObject Tuto;
    public GameObject[] TutoText;
    public GameObject GoTutoobj;
    string sysLang;
    // Use this for initialization
    void Start()
    {
        sysLang = Application.systemLanguage.ToString();
        GoTutoobj = GameObject.Find("GoTuto");
        TutoText = new GameObject[3];
        GameObject UI = GameObject.Find("UI Board");
        TutoText[0] = UI.transform.GetChild(0).gameObject;
        TutoText[1] = UI.transform.GetChild(1).gameObject;
        TutoText[2] = UI.transform.GetChild(2).gameObject;
        Tuto = GameObject.Find("Tuto");
        Tuto.GetComponent<Button>().onClick.AddListener(() =>
        { OpenTuto(); });
        GoTutoobj.SetActive(false);
        Lang();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Lang()
    {
        switch (sysLang)
        {
            case "Korean":
                TutoText[0].GetComponentInChildren<Text>().text = "예";
                TutoText[1].GetComponentInChildren<Text>().text = "아니오";
                TutoText[2].GetComponent<Text>().text = "튜토리얼을 보시겠습니까?";
                break;

            case "Japanse":
                TutoText[0].GetComponentInChildren<Text>().text = "はい";
                TutoText[1].GetComponentInChildren<Text>().text = "いいえ";
                TutoText[2].GetComponent<Text>().text = "ツートリアルを見ますか?";
                break;
        default:
                break;
        }
    }
    public void GoTuto()
    {
        SceneManager.LoadScene(2);
    }
    public void OpenTuto() {
        GoTutoobj.SetActive(true);
    }
    public void CloseTuto() {
        GoTutoobj.SetActive(false);
    }
}
