using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDist : MonoBehaviour {

    private static CardDist _instance = null;
    public static CardDist instance
    {
        get
        {
            if (_instance == null)
                Debug.Log("CardDist is NULL");
            return _instance;
        }
    }

    public GameObject P1Dist;
    public GameObject P2Dist;
    public GameObject res;
    GameObject Split1;
    GameObject Split2;
    public bool Split1made;
    public bool Split2made;
    public int index;
    // Use this for initialization
    void Awake() {
        _instance = this;
        Split1made = false;
        Split2made = false;
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Split1made)
        {
            if (Split1.transform.eulerAngles.z < 270)
            {
                Split1.transform.Rotate(0f, 0f, 5f);
                Split1.transform.position = Vector3.MoveTowards(Split1.transform.position, P1Dist.transform.position, Time.deltaTime * 6f);
            }
            else {
                Split1made = false;
                StartCoroutine(Delete(Split1));
                Decks.instance.SetDeck(Cards.instance.PlayCard[index].CardIndex - 1);
                index += 2;
            }
        }
        if (Split2made)
        {
            if (Split2.transform.eulerAngles.z < 270)
            {
                Split2.transform.Rotate(0f, 0f, 5f);
                Split2.transform.position = Vector3.MoveTowards(Split2.transform.position, P2Dist.transform.position, Time.deltaTime * 6f);
            }
            else {
                Split2made = false;
                StartCoroutine(Delete(Split2));
                Decks.instance.SetBack();
            }
        }
    }

    public void Create()
    {
        if (GameManager.instance.isHost)
        {
            Split1 = GameObject.Instantiate<GameObject>(res);
            Split1made = true;
        }
        else {
            Split2 = GameObject.Instantiate<GameObject>(res);
            Split2made = true;
        }
        StartCoroutine(Create2());
        
        
    }

    IEnumerator Create2() {

        yield return new WaitForSeconds(1f);
        if (GameManager.instance.isHost)
        {
            Split2 = GameObject.Instantiate<GameObject>(res);
            Split2made = true;
        }
        else {
            Split1 = GameObject.Instantiate<GameObject>(res);
            Split1made = true;
        }
    }
    IEnumerator Delete(GameObject obj)
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(obj);
    }
}
