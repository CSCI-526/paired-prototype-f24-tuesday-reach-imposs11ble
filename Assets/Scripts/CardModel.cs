using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardModel : MonoBehaviour
{
    public GameObject ParentPanel; //for new gameobject created during the game
    public GameObject losePanel;
    public GameObject winPanel;

    public Sprite[] cardsStack; //playing cards
    public Image currCard;
    private List<int> cardsOrder = new List<int>(); //order of playing cards
    public int cursorInCardOrder = 0;

    public int score = 0;
    public int pointsForRound = 1;
    public int extraLives = 0; // Number of lives for Jokers
    public TextMeshProUGUI hintText;
    public bool hasHint = false;

    public Button nextbutton;
    // betting buttons
    public Button upbutton; 
    public Button equalbutton;
    public Button downbutton;
    public Button spadeButton;
    public Button clubButton;
    public Button heartButton;
    public Button diamondButton;

    // Start is called before the first frame update
    void Start()
    {
        //initially set win/lose panels inactive
        ToggleLosePanel(false);
        ToggleWinPanel(false);
        // not showing the current card and betting buttons
        currCard.enabled = false;
        DisableBetButtons();

        nextbutton.interactable = false; // make next button unclickable

        //shuffle cards and put it into a list
        CreateCardOrder();
        ShuffleCards();

        while (IsAbilityCard(cardsOrder[0]))
        {
            ShuffleCards(); // Reshuffle if the first card is an ability card
        }

        ShowFirstCard();
        nextbutton.interactable = true; // make next button clickable (player ready to bet)
        Debug.Log("Cards Order: " + string.Join(", ", cardsOrder));
    }

    public void ShowCurrCard()
    {
        int indexInStack = cardsOrder[cursorInCardOrder];
        currCard.enabled = true; //show the back of the card
        EnableBetButtons();

        //testing
        int cardNumber = (indexInStack+1) % 13;
        int cardSuit = indexInStack / 13;
        Debug.Log("This card is: " + cardNumber + " and suit: " +cardSuit + " with index: " + indexInStack);
    }

    public void checkBetResult(string betOperation)
    {
        DisableBetButtons(); //stop betting

        int indexInStack = cardsOrder[cursorInCardOrder];
        int cardNumber = (indexInStack+1) % 13;
        int cardSuit = indexInStack / 13;
        int preInStack = cardsOrder[cursorInCardOrder - 1];
        int prevCardNumber = (preInStack+1) % 13; //prev card to compare bigger/same/smaller

        // Handle Joker cards (card index 52 and 53 are Jokers)
        if (indexInStack >= 52)
        {
            HandleJoker();
            return;
        }

        if (IsAbilityCard(indexInStack))
        {
            HandleFaceCard(cardNumber);
            return;
        }

        bool isLose = true;
        if (betOperation.Equals("spade") && (cardSuit == 0))
        {
            isLose = false;
            score += 1* pointsForRound;
            Debug.Log("Guessing Spade Correct!");
        }
        if (betOperation.Equals("club") && (cardSuit == 1))
        {
            isLose = false;
            score = score + 1 * pointsForRound;
            Debug.Log("Guessing Club Correct!");
        }
        if (betOperation.Equals("diamond") && (cardSuit == 2))
        {
            isLose = false;
            score = score + 1 * pointsForRound;
            Debug.Log("Guessing Diamond Correct!");
        }
        if (betOperation.Equals("heart") && (cardSuit == 3))
        {
            isLose = false;
            score = score + 1 * pointsForRound;
            Debug.Log("Guessing Heart Correct!");
        }
        if (betOperation.Equals("up") && (cardNumber > prevCardNumber))
        {
            isLose = false;
            score = score + 1 * pointsForRound;
            Debug.Log("Guessing Bigger Correct!");
        }
        if (betOperation.Equals("equal") && (cardNumber == prevCardNumber))
        {
            isLose = false;
            score = score + 3 * pointsForRound;
            Debug.Log("Guessing same Correct!");
        }
        if (betOperation.Equals("down") && (cardNumber < prevCardNumber))
        {
            isLose = false;
            score = score + 1 * pointsForRound;
            Debug.Log("Guessing smaller Correct!");
        }
        if (isLose) //player guesses wrongly
        {
           if (extraLives > 0) // Check if player has an extra life from a Joker
            {
                extraLives --;
                Debug.Log("Incorrect, but you have an extra life! Lives left: " + extraLives);
            }
            else
            {
                Debug.Log("Incorrect! Game Over.");
                StartCoroutine(ShowGameOver(indexInStack));
                return;
            } 
        }

        cursorInCardOrder ++;

        if (pointsForRound == 2) {
            pointsForRound = 1; //face card used in this round, hence back to 1
        }

        if (score >= 11)
        {
            ToggleWinPanel(true); // player win
            return;
        }

        StartCoroutine(HideCardAfter(1)); // this is to hide the back card image
        StartCoroutine(BecomePrevCard(indexInStack));
    }

    private void HandleFaceCard(int cardNumber)
    {
        MoveCardToAbilityPanel(); // Move face card to ability panel
        int nextInStack = cardsOrder[cursorInCardOrder + 1];
        if (cardNumber == 11) // Jack
        {
            int nextCardSuit = nextInStack / 13;
            ProvideColorHint(nextCardSuit); // Give color hint for next card
            Debug.Log("Jack drawn! Providing hint for the next card.");
        }
        else if (cardNumber == 12) // Queen
        {
            int nextCardNumber = (nextInStack + 1) % 13;
            Debug.Log("Queen drawn! Providing odd/even hint for the next card.");
            ProvideOddEvenHint(nextCardNumber); // Hint for next card
        }
        else if (cardNumber == 0) // King
        {
            Debug.Log("King drawn! Points for this round will be doubled.");
            pointsForRound = 2; // Mark the next round for double points
        }
        cursorInCardOrder ++; // Move to the next card
        //ShowNextCard();
    }

    private void MoveCardToAbilityPanel()
    {
        // Display the ability card in the ability panel (right side)
        GameObject abilityCard = new GameObject();
        Image abilityImage = abilityCard.AddComponent<Image>();
        abilityImage.sprite = cardsStack[cardsOrder[cursorInCardOrder]];

        abilityCard.transform.localScale = new Vector2(0.01938f, 0.01938f);
        abilityCard.transform.position = new Vector2(5, -3); // Move to the right (ability panel)
        abilityCard.GetComponent<RectTransform>().SetParent(ParentPanel.transform);
        abilityCard.SetActive(true);
    }

    /*private void ShowNextCard()
    {
        cursorInCardOrder++; // Move to the next card
        int nextCardIndex = cardsOrder[cursorInCardOrder];
        StartCoroutine(HideCardAfter(1));
        StartCoroutine(BecomePrevCard(nextCardIndex));
    }*/

    /*private void MoveToNextCardWithoutBet()
    {
        cursorInCardOrder++;

        int nextCardIndex = cardsOrder[cursorInCardOrder];
        StartCoroutine(HideCardAfter(1));
        StartCoroutine(BecomePrevCard(nextCardIndex));
    }*/

    private void HandleJoker()
    {
        //Debug.Log("Joker! ");
        extraLives++;
        cursorInCardOrder++;
        StartCoroutine(HideCardAfter(1));
        StartCoroutine(BecomePrevCard(cardsOrder[cursorInCardOrder]));

    }

    private void ProvideColorHint(int cardSuit)
    {
        string colorHint = "";
        if (cardSuit == 0 || cardSuit == 1) // Spades and Clubs
        {
            colorHint = "Black";
        }
        else if (cardSuit == 2 || cardSuit == 3) // Hearts and Diamonds
        {
            colorHint = "Red";
        }
        hasHint = true;
        hintText.text = "Hint: The suit color is " + colorHint;
    }

    private void ProvideOddEvenHint(int cardNumber)
    {
        hasHint = true;
        if (cardNumber % 2 == 0) // Even number
        {
            hintText.text = "Hint: The card is even.";
        }
        else // Odd number
        {
            hintText.text = "Hint: The card is odd.";
        }
    }

    IEnumerator ShowGameOver(int finalCardIndex)
    {
        // Show the final card before displaying "Game Over"
        currCard.enabled = true;
        currCard.sprite = cardsStack[finalCardIndex];

        yield return new WaitForSeconds(2);  // Display the final card for 2 seconds

        ToggleLosePanel(true);  // Show the Game Over panel after the final card
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

    public void ToggleLosePanel(bool isShow) {
        losePanel.SetActive(isShow);
    }

    public void ToggleWinPanel(bool isShow) {
        winPanel.SetActive(isShow);
    }

    private void DisableBetButtons()
    {
        upbutton.gameObject.SetActive(false);
        downbutton.gameObject.SetActive(false);
        equalbutton.gameObject.SetActive(false);
        spadeButton.gameObject.SetActive(false);
        clubButton.gameObject.SetActive(false);
        heartButton.gameObject.SetActive(false);
        diamondButton.gameObject.SetActive(false);
    }

    private void EnableBetButtons()
    {
        upbutton.gameObject.SetActive(true);
        downbutton.gameObject.SetActive(true);
        equalbutton.gameObject.SetActive(true);
        spadeButton.gameObject.SetActive(true);
        clubButton.gameObject.SetActive(true);
        heartButton.gameObject.SetActive(true);
        diamondButton.gameObject.SetActive(true);
    }

    private bool IsAbilityCard(int cardIndex)
    {
        int cardNumber = (cardIndex+1) % 13;
        return (cardNumber == 11 || cardNumber == 12 || cardNumber == 0 || cardIndex >= 52); // J, Q, K, or Joker
    }

    // Function to show the first card at the start of the game
    private void ShowFirstCard()
    {
        int indexInStack = cardsOrder[cursorInCardOrder];
        currCard.enabled = true;
        StartCoroutine(HideCardAfter(1));
        StartCoroutine(BecomePrevCard(indexInStack));
        cursorInCardOrder++;
    }

    //create a list of numbers from 0 to 54 (13*4 + 2 Joker)
    private void CreateCardOrder()
    {
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
    }
}
