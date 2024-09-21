using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardModel : MonoBehaviour
{
    public Sprite[] faces;
    public Image currCard;

    public int cardIndex;

    // Start is called before the first frame update
    void Start()
    {
        currCard.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showCurrCard() {
        currCard.enabled = true;
        int randomNum = Random.Range(0,55);
        currCard.sprite = faces[randomNum];

        //let card disappear after 3 secs
        StartCoroutine (HideCardAfterDelay(3));
    }

    IEnumerator HideCardAfterDelay(int sec) {
        yield return new WaitForSeconds (sec);
        currCard.enabled = false;
    }
}
