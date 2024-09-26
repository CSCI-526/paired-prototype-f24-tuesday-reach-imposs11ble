using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardModel : MonoBehaviour
{
    public Sprite[] cardsStack;
    public Image currCard;
    public GameObject ParentPanel;
    private List<int> cardsOrder = new List<int>();
    public int cursorInCardOrder = 0;
    public float speed = 10.0f;

    public Button upbutton;
    public Button equalbutton;
    public Button downbutton;
    public Button nextbutton;
    public Button spadeButton;
    public Button clubButton;
    public Button heartButton;
    public Button diamondButton;

    public GameObject losePanel;
    public GameObject winPanel;

    // Start is called before the first frame update
    void Start()
    {
        nextbutton.interactable = false;
        upbutton.gameObject.SetActive(false);
        downbutton.gameObject.SetActive(false);
        equalbutton.gameObject.SetActive(false);
        spadeButton.gameObject.SetActive(false);
        clubButton.gameObject.SetActive(false);
        heartButton.gameObject.SetActive(false);
        diamondButton.gameObject.SetActive(false);
        // not showing the current card
        currCard.enabled = false;

        createCardOrder();
        ShuffleCards();


        // show one card first so user cab bet for the next 
        int indexInStack = cardsOrder[cursorInCardOrder];
        currCard.enabled = true;
        StartCoroutine(HideCardAfter(1));
        StartCoroutine(BecomePrevCard(indexInStack));
        cursorInCardOrder++;
        nextbutton.interactable = true;


        //initially set win/lose panels inactive
        toggleLosePanel(false);
        toggleWinPanel(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void createCardOrder()
    {
        //create a list of numbers from 0 to 54
        //playing cards has 52 with 2 ghost card
        for(int i=0; i< cardsStack.Length; i++)
        {
            cardsOrder.Add(i);
        }
    }

    private void ShuffleCards()
    {
        int numOfCardsleft = cardsOrder.Count;
        while (numOfCardsleft > 1)
        {
            numOfCardsleft --;
            int k = Random.Range(0,numOfCardsleft);
            int value = cardsOrder[k];
            cardsOrder[k] = cardsOrder[numOfCardsleft];
            cardsOrder[numOfCardsleft] = value;
        }
        //Debug.Log("Cards Order: " + string.Join(", ", cardsOrder));
    }

    public void checkBetResult(string betOperation)
    {

        upbutton.gameObject.SetActive(false);
        downbutton.gameObject.SetActive(false);
        equalbutton.gameObject.SetActive(false);
        spadeButton.gameObject.SetActive(false);
        clubButton.gameObject.SetActive(false);
        heartButton.gameObject.SetActive(false);
        diamondButton.gameObject.SetActive(false);

        int indexInStack = cardsOrder[cursorInCardOrder];
        int cardNumber = indexInStack % 13;
        //TODO: notice that cardNumber hasen't take care of 2 joker cards rn,
        // they will be considered as card 1 or 2. need to address this later
        int prevIndex = cursorInCardOrder - 1;
        indexInStack = cardsOrder[prevIndex];
        int prevCardNumber = indexInStack % 13;

        // Debug.Log("cardNumber: " + cardNumber);
        // Debug.Log("prevCardNumber: " + prevCardNumber);
        if (cardNumber > 10) // it is a J/Q/K or Joker card!
        {
            //TODO: add features for these types of card when implementing
            // scoring system
        }
        //TODO: 

        if (betOperation.Equals("spade"))
        {
            if (indexInStack <= 12)
            {
                Debug.Log("Correct!");
            }
            else
            {
                Debug.Log("False!");
            }
        }
        if (betOperation.Equals("club"))
        {
            if (13 <= indexInStack && indexInStack <= 26)
            {
                Debug.Log("Correct!");
            }
            else
            {
                Debug.Log("False!");
            }
        }
        if (betOperation.Equals("heart"))
        {
            if (40 <= indexInStack && indexInStack <= 52)
            {
                Debug.Log("Correct!");
            }
            else
            {
                Debug.Log("False!");
            }
        }
        if (betOperation.Equals("diamond"))
        {
            if (27 <= indexInStack && indexInStack <= 39)
            {
                Debug.Log("Correct!");
            }
            else
            {
                Debug.Log("False!");
            }
        }



        if (betOperation.Equals("up"))
        {
            if (cardNumber > prevCardNumber)
            {
                Debug.Log("Correct!");
            }
            else
            {
                Debug.Log("False!");
            }
        }
        if (betOperation.Equals("up"))
        {
            if (cardNumber == prevCardNumber)
            {
                Debug.Log("Correct!");
            }
            else
            {
                Debug.Log("False!");
            }
        }
        if (betOperation.Equals("down"))
        {
            if (cardNumber < prevCardNumber)
            {
                Debug.Log("Correct!");
            }
            else
            {
                Debug.Log("False!");
            }
        }

        StartCoroutine(HideCardAfter(1)); // this is to hide the back card image
        StartCoroutine(BecomePrevCard(indexInStack));
        cursorInCardOrder++;
        // Debug.Log("cursorInCardOrder aaa:" + cursorInCardOrder);
        //return false;
    }
    public void ShowCurrCard()
    {
        int indexInStack = cardsOrder[cursorInCardOrder];

        //let card back appears and disappear after 1 sec
        currCard.enabled = true;
        //StartCoroutine (HideCardAfter(1)); // this is to hide the back card image

        //create a new image and fly to the left area
        //StartCoroutine (BecomePrevCard(indexInStack));

        Debug.Log("indexInStack: " + indexInStack);
        Debug.Log("cursorInCardOrder: " + cursorInCardOrder);

        // enable 3 bet buttons
        upbutton.gameObject.SetActive(true);
        downbutton.gameObject.SetActive(true);
        equalbutton.gameObject.SetActive(true);
        spadeButton.gameObject.SetActive(true);
        clubButton.gameObject.SetActive(true);
        heartButton.gameObject.SetActive(true);
        diamondButton.gameObject.SetActive(true);

        // move cursor to the next card
        //cursorInCardOrder ++;

    }

    // show current card that user need to bet
    IEnumerator BecomePrevCard(int preIndex)
    {
        // to make it more like flipping
        yield return new WaitForSeconds (1);
        GameObject prevCard = new GameObject();
        Image NewImage = prevCard.AddComponent<Image>();
        NewImage.sprite = cardsStack[preIndex];
        prevCard.transform.localScale = new Vector2(0.01938f,0.01938f);
        prevCard.transform.position = new Vector2(0,-3);
        //Assign the newly created Image GameObject as a Child of the Parent Panel.
        prevCard.GetComponent<RectTransform>().SetParent(ParentPanel.transform);
        prevCard.SetActive(true);

        //wait for 2 secs to stay
        yield return new WaitForSeconds (2);
        //move to the left
        prevCard.transform.position = new Vector2(-6,-3);
    }

    IEnumerator HideCardAfter(int sec) {
        yield return new WaitForSeconds (sec);
        currCard.enabled = false;
    }

    public void toggleLosePanel(bool isShow) {
        losePanel.SetActive(isShow);
    }

    public void toggleWinPanel(bool isShow) {
        winPanel.SetActive(isShow);
    }
}
