using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialougeQuiz : MonoBehaviour
{
    public bool inConvo = false;

    public Canvas quizCanvas;
    public List<GameObject> boxes = new List<GameObject>();
    public TextMeshProUGUI nameForQuiz;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI answerAText;
    public TextMeshProUGUI answerBText;
    public TextMeshProUGUI answerCText;
    public TextMeshProUGUI answerDText;

    public Camera mainCamera;

    private int correctAnswerIndex;
    public bool SuccessfulQuiz = false;

    public DialogueBoxSender burnerBox;
    public DialogueBoxSender senderCopy;

    public void StartQuiz(DialogueBoxSender dialogueSender)
    {
        //Make all things visible
        foreach (GameObject box in boxes)
        {
            box.GetComponent<SpriteRenderer>().enabled = true;
        }
        quizCanvas.GetComponent<Canvas>().enabled = true;


        //Add all the answers, names, and question to the quiz canvas
        nameForQuiz.text = dialogueSender.nameOfCharacter;
        questionText.text = dialogueSender.question;
        answerAText.text = dialogueSender.answerA;
        answerBText.text = dialogueSender.answerB;
        answerCText.text = dialogueSender.answerC;
        answerDText.text = dialogueSender.answerD;
        
        //Type all the answers
        StopAllCoroutines();
        StartCoroutine(TypeSentence(dialogueSender.question, questionText));
        StartCoroutine(TypeSentence(answerAText.text, answerAText));
        StartCoroutine(TypeSentence(answerBText.text, answerBText));
        StartCoroutine(TypeSentence(answerCText.text, answerCText));
        StartCoroutine(TypeSentence(answerDText.text, answerDText));

        //Determine which box is linked to the correct answer
        switch (dialogueSender.correctAnswerChar)
        {
            case 'A':
                correctAnswerIndex = 1;
                break;
            case 'B':
                correctAnswerIndex = 2;
                break;
            case 'C':
                correctAnswerIndex = 3;
                break;
            case 'D':
                correctAnswerIndex = 4;
                break;
            default:
                Debug.Log("Invalid Quiz Answer: " + dialogueSender.name);
                break;
        }

        //Start Quiz
        inConvo = true;

        //Make a copy of the sender (determines which quiz was completed later since the quiz box is universal)
        senderCopy = dialogueSender;
    }

    IEnumerator TypeSentence(string sentence, TextMeshProUGUI textBox) //Same as dialogue box type
    {
        textBox.text = "";
        foreach (char letter in sentence.ToCharArray()) //loop through the character
        {
            textBox.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    //Determine the colors of the boxes and the highlighted "hovered" versions
    private int currentlySelectedBox; //1-4
    Color OriginalRed =  new Color(241 / 255f, 67 / 255f, 63 / 255f, 1);
    Color OriginalYellow = new Color(1, 202 / 255f, 58 / 255f, 1);
    Color OriginalGreen = new Color(138 / 255f, 201 / 255f, 38 / 255f, 1);
    Color OriginalBlue = new Color(25 / 255f, 130 / 255f, 196 / 255f, 1);
    public Color[] OriginalColors;
    Color HoverRed = new Color(1, 89 / 255f, 94 / 255f, 1);
    Color HoverYellow = new Color(247/255f, 233 / 255f, 103 / 255f, 1);
    Color HoverGreen = new Color(169 / 255f, 207 / 255f, 84 / 255f, 1);
    Color HoverBlue = new Color(112 / 255f, 183 / 255f, 186 / 255f, 1);
    public Color[] HoveredColors; //Red, Yellow, Green, Blue

    private void Start()
    {
        //On start, makethe color arrays
        OriginalColors = new Color []{ OriginalRed, OriginalGreen, OriginalYellow, OriginalBlue };
        HoveredColors = new Color[] { HoverRed, HoverGreen, HoverYellow, HoverBlue };

        //Set the first box to be "hovered"
        boxes[1].GetComponent<SpriteRenderer>().color = HoveredColors[0];
        currentlySelectedBox = 1; //1 is A (Top left), 2 is B (bottom left), 3 is C (top right), 4 is D (bottom right)

        senderCopy = burnerBox;
    }

    private void Update()
    {
        if (inConvo)
        {
            //Set the color of the hovered box
            boxes[currentlySelectedBox].GetComponent<SpriteRenderer>().color = HoveredColors[currentlySelectedBox-1];

            //Set the color of every other box
            for (int i = 1; i <= 4;  i++) //1-4 since index 0 is the dialogue box itself (makes it easier when revelaing the boxes at the beginning)
            {
                if (i != currentlySelectedBox)
                {
                    boxes[i].GetComponent<SpriteRenderer>().color = OriginalColors[i - 1];
                }
            }

            //When the player is in the quiz and selects a response
            if (Input.GetKeyDown(KeyCode.O))
            {
                if (currentlySelectedBox == correctAnswerIndex)
                {
                    //Success!
                    SuccessfulQuiz = true;
                    inConvo = false;
                    foreach (GameObject box in boxes)
                    {
                        box.GetComponent<SpriteRenderer>().enabled = false;
                    }
                    quizCanvas.GetComponent<Canvas>().enabled = false;
                    senderCopy.PostQuizDialogueRight();
                    senderCopy.numberOfConversations = 1;
                }
                else
                {
                    //Failure!
                    SuccessfulQuiz = false;
                    inConvo = false;
                    foreach (GameObject box in boxes)
                    {
                        box.GetComponent<SpriteRenderer>().enabled = false;
                    }
                    quizCanvas.GetComponent<Canvas>().enabled = false;
                    senderCopy.PostQuizDialogueWrong();
                    senderCopy.numberOfConversations = 1;
                    senderCopy = burnerBox;
                }
            }

            //Determines the movement of the players hovering icon
            if (currentlySelectedBox == 1)
            {
                if (Input.GetKeyDown(KeyCode.D)) 
                {
                    currentlySelectedBox = 3;
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    currentlySelectedBox = 2;
                }
            }
            if (currentlySelectedBox == 2)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    currentlySelectedBox = 1;
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    currentlySelectedBox = 4;
                }
            }
            if (currentlySelectedBox == 3)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    currentlySelectedBox = 1;
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    currentlySelectedBox = 4;
                }
            }
            if (currentlySelectedBox == 4)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    currentlySelectedBox = 2;
                }
                if (Input.GetKeyDown(KeyCode.W))
                {
                    currentlySelectedBox = 3;
                }
            }
        }
    }
}
