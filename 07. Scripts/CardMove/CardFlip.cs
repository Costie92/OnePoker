using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFlip : MonoBehaviour {

    // Use this for initialization
    public bool flip;
    public Sprite P1Sprite, P2Sprite;
    public SpriteRenderer P1SetCard, P2SetCard;
    public bool CheckP1Surr;
    public bool CheckP2Surr;
    private static CardFlip _instance = null;
    public static CardFlip instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("CardFlip is NULL");
            return _instance;
        }
    }

    private void Awake()
    {
        CheckP1Surr = false;
        CheckP2Surr = false;
        _instance = this;
        P1SetCard = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        P2SetCard = this.transform.GetChild(1).GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update () {
        if (flip)
        {
            if (this.transform.eulerAngles.y >= 90)
            {
                Flip();
            }
            if (this.transform.eulerAngles.y < 180)
                this.transform.Rotate(0f, 3f, 0f);
            else
            {

                if (CheckP2Surr)
                {
                    Debug.Log("P2Surrender & Win");
                    CheckP2Surr = false;
                    GameManager.instance.WinGame();
                }
                else if (CheckP1Surr)
                {
                    Debug.Log("P1Surrender & Lose");
                    CheckP1Surr = false;
                    GameManager.instance.LoseGame();
                }
                else
                {
                    Debug.Log("NoOneSurrender Compare");
                    GameManager.instance.Compare();
                }
                flip = false;
            }
        }
    }
    public void SetSprite(Sprite P1, Sprite P2) {
        P1Sprite = P1;
        P2Sprite = P2;
    }
    public void Flip() {
        P1SetCard.sprite = P1Sprite;
        P2SetCard.sprite = P2Sprite;
    }
    public void ResetRotate() {

        this.transform.eulerAngles = new Vector3(0,0,0);
    }
    public void P1Surr() {
        CheckP1Surr = true;
    }
    public void P2Surr()
    {
        CheckP2Surr = true;
    }
}
