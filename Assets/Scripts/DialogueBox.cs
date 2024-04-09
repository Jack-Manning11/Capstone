using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DialogueBox : MonoBehaviour
{
    private Queue<string> sentences;

    public bool inConvo = false;

    public GameObject rectangle;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialougeText;
    public TextMeshProUGUI instructionText;

    public bool moving = false;
    float t;
    Vector3 rectStartPosition;
    Vector3 rectTarget;
    Vector3 nameStartPosition;
    Vector3 nameTarget;
    Vector3 dialougeTextStartPosition;
    Vector3 dialougeTextTarget;
    Vector3 instructionTextStartPosition;
    Vector3 instructionTextTarget;
    public float shiftDistance;
    public float rectShiftDistance;
    private float ogRectShiftDistance;

    public NewMover player;

    public Camera mainCamera;

    public DialougeQuiz quiz;

    private DialogueBoxSender sender;

    public Canvas canvas;

    public ControlManager controlManager;
    private void Start() //Create a new queue
    {
        sentences = new Queue<string>();
        ogRectShiftDistance = rectShiftDistance;
    }

    public void StartDialogue(DialogueBoxSender dialouge) //Start by replacign the name and enqueing all the sentences (come from the sender)
    {
        Debug.Log("Starting convo with: " + dialouge.nameOfCharacter);
        nameText.text = dialouge.nameOfCharacter; //Set the Name

        sender = dialouge;

        player.moveLock = true;

        sentences.Clear(); //Clear any leftovers

        foreach (string sentence in dialouge.mainDialogue) // Add all the new sentence to the queue
        {
            sentences.Enqueue(sentence);
        }

        inConvo = true;

        //Slowly Move the rectangle up
        t = 0;
        rectStartPosition = rectangle.transform.position;
        rectTarget = new Vector3(0, -3.6f, 0);
        rectShiftDistance = ogRectShiftDistance;

        nameStartPosition = nameText.transform.position;
        nameTarget = new Vector3(canvas.transform.position.x, nameStartPosition.y + ((canvas.GetComponent<RectTransform>().rect.height)/3f), 0);

        dialougeTextStartPosition = dialougeText.transform.position;
        dialougeTextTarget = new Vector3(canvas.transform.position.x, dialougeTextStartPosition.y + ((canvas.GetComponent<RectTransform>().rect.height) / 3f), 0);

        instructionTextStartPosition = instructionText.transform.position;
        instructionTextTarget = new Vector3(canvas.transform.position.x, instructionTextStartPosition.y + ((canvas.GetComponent<RectTransform>().rect.height) / 3f), 0);
        moving = true; //Moving Up

        DisplayNextSentence();

        //player.moveLock = true;
    }

    public void DisplayNextSentence() //Calls the type sentence coroutine and dequeues old sentences
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        Debug.Log(sentence);
        StopAllCoroutines(); //Stop current coroutine if the player moves forward before the old one is done typing
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence) //Types sentences letter by letter
    {
        dialougeText.text = "";
        foreach (char letter in sentence.ToCharArray()) //loop through the character
        {
            dialougeText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void EndDialogue() //When the user hits the end of a sentence queue, move the box back down
    {
        Debug.Log("End of convo");

        t = 0;
        rectStartPosition = rectangle.transform.position;
        rectTarget = new Vector3(0, - 5.75f, 0);
        rectShiftDistance = ogRectShiftDistance - 2.15f;

        nameStartPosition = nameText.transform.position;
        nameTarget = new Vector3(canvas.transform.position.x, nameStartPosition.y - ((canvas.GetComponent<RectTransform>().rect.height) / 3f), 0);

        dialougeTextStartPosition = dialougeText.transform.position;
        dialougeTextTarget = new Vector3(canvas.transform.position.x, dialougeTextStartPosition.y - ((canvas.GetComponent<RectTransform>().rect.height) / 3f), 0);

        instructionTextStartPosition = instructionText.transform.position;
        instructionTextTarget = new Vector3(canvas.transform.position.x, instructionTextStartPosition.y - ((canvas.GetComponent<RectTransform>().rect.height) / 3f), 0);
        moving = true; //Moving Down

        inConvo = false;

        if (sender.preQuizCheck != null && sender.checkAllPreQuizChecks() == true)
        {
            sender.preQuizCheckBool = true;
        }
        else if(sender.preQuizCheck == null)
        {
            sender.preQuizCheckBool = true;
        }

        if (sender.isQuestionAfter && sender.numberOfConversations > 1 && sender.SuccessfulQuiz == false && sender.preQuizCheckBool == true) //If there is a quiz and the player has already talked to the character once, start the quiz
        {
            Debug.Log("Quiz Start");
            quiz.StartQuiz(sender);
        }
        else player.moveLock = false;
    }

    private void Update()
    {
        if (quiz.inConvo)
        {
            inConvo = true;
        }

        if (moving)
        {
            if (rectTarget.y > rectangle.transform.position.y) Debug.Log("Moving Up");
            else if (rectTarget.y < rectangle.transform.position.y) Debug.Log("Moving Down");

            t += Time.deltaTime * 3;

            rectTarget.x = mainCamera.transform.position.x;
            rectTarget.y = mainCamera.transform.position.y + rectShiftDistance;

            rectangle.transform.position = Vector3.Lerp(rectStartPosition, rectTarget, t);
            nameText.transform.position = Vector3.Lerp(nameStartPosition, nameTarget, t);
            dialougeText.transform.position = Vector3.Lerp(dialougeTextStartPosition, dialougeTextTarget, t);
            instructionText.transform.position = Vector3.Lerp(instructionTextStartPosition, instructionTextTarget, t);

            if (rectangle.transform.position.y == rectTarget.y) //If the destination has been reached
            {
                moving = false;
            }
        }
        else if (inConvo && (Input.GetKeyDown(KeyCode.O) || controlManager.select)) //Display the next sentence if the button is hit while in a conversation.
        {
            DisplayNextSentence();
        }
    }
}
