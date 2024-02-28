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

    public Camera mainCamera;

    
    public DialougeQuiz quiz;

    private DialogueBoxSender sender;

    private void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(DialogueBoxSender dialouge)
    {
        Debug.Log("Starting convo with: " + dialouge.nameOfCharacter);
        nameText.text = dialouge.nameOfCharacter; //Set the Name

        sender = dialouge;

        sentences.Clear(); //Clear any leftovers  

        foreach (string sentence in dialouge.mainDialogue) // Add all teh new sentence to the queue
        {
            //Debug.Log("enqueued");
            sentences.Enqueue(sentence);
        }

        inConvo = true;

        //Jump Text upwards
        //rectangle.transform.position = new Vector3(rectangle.transform.position.x, rectangle.transform.position.y + 1.2f, 0);
        //nameText.transform.position = new Vector3(nameText.transform.position.x, nameText.transform.position.y + 1.2f, 0);
        //dialougeText.transform.position = new Vector3(dialougeText.transform.position.x, dialougeText.transform.position.y + 1.2f, 0);

        //Slowly Move the rectangle up
        t = 0;
        rectStartPosition = rectangle.transform.position;
        rectTarget = new Vector3(mainCamera.transform.position.x, rectStartPosition.y + shiftDistance, 0);
        nameStartPosition = nameText.transform.position;
        nameTarget = new Vector3(mainCamera.transform.position.x, nameStartPosition.y + shiftDistance, 0);
        dialougeTextStartPosition = dialougeText.transform.position;
        dialougeTextTarget = new Vector3(mainCamera.transform.position.x, dialougeTextStartPosition.y + shiftDistance, 0);
        instructionTextStartPosition = instructionText.transform.position;
        instructionTextTarget = new Vector3(mainCamera.transform.position.x, instructionTextStartPosition.y + shiftDistance, 0);
        moving = true; //Moving Up

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
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

    IEnumerator TypeSentence (string sentence)
    {
        dialougeText.text = "";
        foreach (char letter in sentence.ToCharArray()) //loop through the character
        {
            dialougeText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void EndDialogue()
    {
        Debug.Log("End of convo");

        t = 0;
        rectStartPosition = rectangle.transform.position;
        rectTarget = new Vector3(mainCamera.transform.position.x, rectStartPosition.y - shiftDistance, 0);
        nameStartPosition = nameText.transform.position;
        nameTarget = new Vector3(mainCamera.transform.position.x, nameStartPosition.y - shiftDistance, 0);
        dialougeTextStartPosition = dialougeText.transform.position;
        dialougeTextTarget = new Vector3(mainCamera.transform.position.x, dialougeTextStartPosition.y - shiftDistance, 0);
        instructionTextStartPosition = instructionText.transform.position;
        instructionTextTarget = new Vector3(mainCamera.transform.position.x, instructionTextStartPosition.y - shiftDistance, 0);
        moving = true; //Moving Down

        inConvo = false;
        if (sender.isQuestionAfter && sender.numberOfConversations > 1)
        {
            quiz.StartQuiz(sender);
        }
        
    }

    private void Update()
    {
        if (moving)
        {
            if (rectTarget.y > rectangle.transform.position.y) Debug.Log("Moving Up");
            else if (rectTarget.y < rectangle.transform.position.y) Debug.Log("Moving Down");

            t += Time.deltaTime * 3;

            rectTarget.x = mainCamera.transform.position.x;
            nameTarget.x = mainCamera.transform.position.x;
            dialougeTextTarget.x = mainCamera.transform.position.x;
            instructionTextTarget.x = mainCamera.transform.position.x;

            rectangle.transform.position = Vector3.Lerp(rectStartPosition, rectTarget, t);
            nameText.transform.position = Vector3.Lerp(nameStartPosition, nameTarget, t);
            dialougeText.transform.position = Vector3.Lerp(dialougeTextStartPosition, dialougeTextTarget, t);
            instructionText.transform.position = Vector3.Lerp(instructionTextStartPosition, instructionTextTarget, t);

            if (rectangle.transform.position.y == rectTarget.y) //DesinationReacher
            {
                moving = false;
            }
        }
        else if (inConvo && Input.GetKeyUp(KeyCode.O))
        {
            DisplayNextSentence();
        }
    }
}
