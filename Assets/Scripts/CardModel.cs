using JetBrains.Annotations;
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
    public int score = 0;
    //public int maxCards = 52;
    public int extraLives = 0; // Number of lives for Jokers

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
    public Text hintText;

    // Start is called before the first frame update
    void Start()
    {
        nextbutton.interactable = false;
        DisableBetButtons();
        // not showing the current card
        currCard.enabled = false;

        createCardOrder();
        ShuffleCards();

        while (IsAbilityCard(cardsOrder[0]))
        {
            ShuffleCards(); // Reshuffle if the first card is an ability card
        }


        ShowFirstCard();
        nextbutton.interactable = true;
        Debug.Log("Cards Order: " + string.Join(", ", cardsOrder));
    


    //initially set win/lose panels inactive
        toggleLosePanel(false);
        toggleWinPanel(false);
    }


    bool IsAbilityCard(int cardIndex)
    {
        int cardNumber = cardIndex % 13;
        return (cardNumber == 10 || cardNumber == 11 || cardNumber == 12 || cardIndex >= 52); // J, Q, K, or Joker
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

        DisableBetButtons();


        int indexInStack = cardsOrder[cursorInCardOrder];
        int preInStack = cardsOrder[cursorInCardOrder-1];
        int cardNumber = indexInStack % 13;
        //TODO: notice that cardNumber hasen't take care of 2 joker cards rn,
        // they will be considered as card 1 or 2. need to address this later
        int cardSuit = indexInStack / 13;
        int prevCardNumber = preInStack % 13;
        int prevCardSuit = preInStack / 13;

        int pointsForRound = 1;
        // Debug.Log("cardNumber: " + cardNumber);
        // Debug.Log("prevCardNumber: " + prevCardNumber);

        // Handle Joker cards (assuming 52 and 53 are jokers)
        if (indexInStack >= 52)
        {
            HandleJoker();
            return;
        }


        if (IsAbilityCard(cardsOrder[cursorInCardOrder]))
        {
            HandleFaceCard(cardNumber, indexInStack / 13, pointsForRound);
            return;
        }



        //TODO: 

        if (betOperation.Equals("spade"))
        {
            if (cardSuit ==0)
            {
                score += 1* pointsForRound;
                Debug.Log("Correct!");
            }
            else
            {
                if (extraLives > 0) // Check if player has an extra life from a Joker
                {
                    extraLives--;
                    Debug.Log("Incorrect, but you have an extra life! Lives left: " + extraLives);
                }
                else
                {
                    Debug.Log("Incorrect! Game Over.");
                    StartCoroutine(ShowGameOver(indexInStack));
                    return;
                }
            }
        }
        if (betOperation.Equals("club"))
        {
            if (cardSuit == 1)
            {
                score = score + 1 * pointsForRound;
                Debug.Log("Correct!");
            }
            else
            {
                if (extraLives > 0) // Check if player has an extra life from a Joker
                {
                    extraLives--;
                    Debug.Log("Incorrect, but you have an extra life! Lives left: " + extraLives);
                }
                else
                {
                    Debug.Log("Incorrect! Game Over.");
                    StartCoroutine(ShowGameOver(indexInStack));
                    return;
                }
            }
        }
        if (betOperation.Equals("heart"))
        {
            if (cardSuit == 2)
            {
                score = score + 1 * pointsForRound;
                Debug.Log("Correct!");
            }
            else
            {
                if (extraLives > 0) // Check if player has an extra life from a Joker
                {
                    extraLives--;
                    Debug.Log("Incorrect, but you have an extra life! Lives left: " + extraLives);
                }
                else
                {
                    Debug.Log("Incorrect! Game Over.");
                    StartCoroutine(ShowGameOver(indexInStack));
                    return;
                }
            }
        }
        if (betOperation.Equals("diamond"))
        {
            if (cardSuit == 3)
            {
                score = score + 1 * pointsForRound;
                Debug.Log("Correct!");
            }
            else
            {
                if (extraLives > 0) // Check if player has an extra life from a Joker
                {
                    extraLives--;
                    Debug.Log("Incorrect, but you have an extra life! Lives left: " + extraLives);
                }
                else
                {
                    Debug.Log("Incorrect! Game Over.");
                    StartCoroutine(ShowGameOver(indexInStack));
                    return;
                }
            }
        }



        if (betOperation.Equals("up"))
        {
            if (cardNumber > prevCardNumber)
            {
                score = score + 1 * pointsForRound;
                Debug.Log("Correct!");
            }
            else
            {
                if (extraLives > 0) // Check if player has an extra life from a Joker
                {
                    extraLives--;
                    Debug.Log("Incorrect, but you have an extra life! Lives left: " + extraLives);
                }
                else
                {
                    Debug.Log("Incorrect! Game Over.");
                    StartCoroutine(ShowGameOver(indexInStack));
                    return;
                }
            }
        }
        if (betOperation.Equals("equal"))
        {
            if (cardNumber == prevCardNumber)
            {
                score = score + 3 * pointsForRound;
                Debug.Log("Correct!");
            }
            else
            {
                if (extraLives > 0) // Check if player has an extra life from a Joker
                {
                    extraLives--;
                    Debug.Log("Incorrect, but you have an extra life! Lives left: " + extraLives);
                }
                else
                {
                    Debug.Log("Incorrect! Game Over.");
                    StartCoroutine(ShowGameOver(indexInStack));
                    return;
                }
            }
        }
        if (betOperation.Equals("down"))
        {
            if (cardNumber < prevCardNumber)
            {
                score = score + 1 * pointsForRound;
                Debug.Log("Correct!");
            }
            else
            {
                if (extraLives > 0) // Check if player has an extra life from a Joker
                {
                    extraLives--;
                    Debug.Log("Incorrect, but you have an extra life! Lives left: " + extraLives);
                }
                else
                {
                    Debug.Log("Incorrect! Game Over.");
                    StartCoroutine(ShowGameOver(indexInStack));
                    return;
                }
            }
        }





        cursorInCardOrder++;

        if (score == 11)
        {
            toggleWinPanel(true); // Show win if all cards are guessed
            return;
        }


        

        StartCoroutine(HideCardAfter(1)); // this is to hide the back card image
        StartCoroutine(BecomePrevCard(indexInStack));
   
        // Debug.Log("cursorInCardOrder aaa:" + cursorInCardOrder);
        //return false;
    }

    private void HandleFaceCard(int cardNumber, int cardSuit, int pointsForRound)
    {
        if (cardNumber == 10) // Jack
        {
            MoveCardToAbilityPanel(); // Move Jack to ability panel
            ProvideColorHint(cardSuit); // Give color hint for next card
            Debug.Log("Jack drawn! Providing hint for the next card.");
            cursorInCardOrder++; // Move to the next card
            ShowNextCard();
        }
        else if (cardNumber == 11) // Queen
        {
            MoveCardToAbilityPanel(); // Move Queen to ability panel
            Debug.Log("Queen drawn! Providing odd/even hint for the next card.");
            ProvideOddEvenHint(cardsOrder[cursorInCardOrder] % 13); // Hint for next card
            cursorInCardOrder++; // Move to the next card
            ShowNextCard();
        }
        else if (cardNumber == 12) // King
        {
            MoveCardToAbilityPanel(); // Move King to ability panel
            Debug.Log("King drawn! Points for this round will be doubled.");
            pointsForRound = 2; // Mark the next round for double points
            cursorInCardOrder++; // Move to the next card
            ShowNextCard();
        }
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



    private void ShowNextCard()
    {


        int nextCardIndex = cardsOrder[cursorInCardOrder];
        StartCoroutine(HideCardAfter(1));
        StartCoroutine(BecomePrevCard(nextCardIndex));
    }

    private void MoveToNextCardWithoutBet()
    {
        cursorInCardOrder++;

        int nextCardIndex = cardsOrder[cursorInCardOrder];
        StartCoroutine(HideCardAfter(1));
        StartCoroutine(BecomePrevCard(nextCardIndex));
    }


    public void ShowCurrCard()
    {
        int indexInStack = cardsOrder[cursorInCardOrder];

        //let card back appears and disappear after 1 sec
        currCard.enabled = true;

        EnableBetButtons();

    }

    private void HandleJoker()
    {
        Debug.Log("Joker! ");
        // For simplicity, assume drawing a joker ends the game

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

        hintText.text = "Hint: The suit color is " + colorHint;
    }

    private void ProvideOddEvenHint(int cardNumber)
    {
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

        toggleLosePanel(true);  // Show the Game Over panel after the final card
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
}
