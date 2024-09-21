using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardModel : MonoBehaviour
{
    public Sprite[] cardsStack;
    public Image currCard;
    private List<int> cardsOrder = new List<int>();
    public int cursorInCardOrder = 0;

    // Start is called before the first frame update
    void Start()
    {
        // not showing the current card
        currCard.enabled = false;

        createCardOrder();
        ShuffleCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void createCardOrder() {
        //create a list of numbers from 0 to 54
        //playing cards has 52 with 2 ghost card
        for(int i=0; i< cardsStack.Length; i++)
        {
            cardsOrder.Add(i);
        }
    }

    private void ShuffleCards() {
        int numOfCardsleft = cardsOrder.Count;
        while (numOfCardsleft > 1)
        {
            numOfCardsleft --;
            int k = Random.Range(0,numOfCardsleft);;
            int value = cardsOrder[k];
            cardsOrder[k] = cardsOrder[numOfCardsleft];
            cardsOrder[numOfCardsleft] = value;
        }
        Debug.Log("Cards Order: " + string.Join(", ", cardsOrder));
    }

    public void ShowCurrCard() {
        currCard.enabled = true;
        int indexInStack = cardsOrder[cursorInCardOrder];
        currCard.sprite = cardsStack[indexInStack];
        // move cursor to the next card
        cursorInCardOrder ++;

        //let card disappear after 3 secs
        StartCoroutine (HideCardAfterDelay(3));
    }

    IEnumerator HideCardAfterDelay(int sec) {
        yield return new WaitForSeconds (sec);
        currCard.enabled = false;
    }
}
