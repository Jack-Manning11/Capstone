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

    public void StartQuiz(DialogueBoxSender dialougeSender)
    {
        //Make all things visible
        foreach (GameObject box in boxes)
        {
            box.GetComponent<SpriteRenderer>().enabled = true;
        }
        quizCanvas.GetComponent<Canvas>().enabled = true;

        nameForQuiz.text = dialougeSender.nameOfCharacter;
        questionText.text = dialougeSender.question;
        answerAText.text = dialougeSender.answerA;
        answerBText.text = dialougeSender.answerB;
        answerCText.text = dialougeSender.answerC;
        answerDText.text = dialougeSender.answerD;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(dialougeSender.question, questionText));
        StartCoroutine(TypeSentence(answerAText.text, answerAText));
        StartCoroutine(TypeSentence(answerBText.text, answerBText));
        StartCoroutine(TypeSentence(answerCText.text, answerCText));
        StartCoroutine(TypeSentence(answerDText.text, answerDText));

        //Start Quiz
        inConvo = true;
    }

    IEnumerator TypeSentence(string sentence, TextMeshProUGUI textBox)
    {
        textBox.text = "";
        foreach (char letter in sentence.ToCharArray()) //loop through the character
        {
            textBox.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void EndDialogue()
    {
        inConvo = false;
        Debug.Log("End of convo");
    }

    private int currentlySelectedBox; //1-4
    Color OriginalRed = new Color(255, 89, 94);
    Color OriginalYellow = new Color(255, 202, 58);
    Color OriginalGreen = new Color(138, 201, 38);
    Color OrginalBlue = new Color(25, 130, 196);
    public Color[] OriginalColors;
    Color HoverRed = new Color(241, 67, 63);
    Color HoverYellow = new Color(247, 233, 103);
    Color HoverGreen = new Color(169, 207, 84);
    Color HoverBlue = new Color(112, 183, 186);
    public Color[] HoveredColors; //Red, Yellow, Green, Blue

    private void Start()
    {
        OriginalColors = new Color []{ OriginalRed, OriginalRed, OriginalRed, OriginalRed };
        HoveredColors = new Color[] { HoverRed, HoverYellow, HoverGreen, HoverBlue };

        boxes[1].GetComponent<SpriteRenderer>().color = HoveredColors[0];
        currentlySelectedBox = 1; //1 is A (Top left), 2 is B (bottom left), 3 is C (top right), 4 is D (bottom right)
    }

    private void Update()
    {
        if (inConvo)
        {
            boxes[currentlySelectedBox].GetComponent<SpriteRenderer>().color = HoveredColors[currentlySelectedBox-1];
            for (int i = 1; i < 5;  i++) 
            {
                if (i != currentlySelectedBox)
                {
                    boxes[currentlySelectedBox].GetComponent<SpriteRenderer>().color = OriginalColors[currentlySelectedBox - 1];
                }
            }

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
