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
            int k = Random.Range(0,numOfCardsleft);;
            int value = cardsOrder[k];
            cardsOrder[k] = cardsOrder[numOfCardsleft];
            cardsOrder[numOfCardsleft] = value;
        }
        //Debug.Log("Cards Order: " + string.Join(", ", cardsOrder));
    }

    public void ShowCurrCard()
    {
        int indexInStack = cardsOrder[cursorInCardOrder];

        //let card back appears and disappear after 1 sec
        currCard.enabled = true;
        StartCoroutine (HideCardAfter(1));

        //create a new image and fly to the left area
        StartCoroutine (BecomePrevCard(indexInStack));

        // move cursor to the next card
        cursorInCardOrder ++;
        
    }

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
}
