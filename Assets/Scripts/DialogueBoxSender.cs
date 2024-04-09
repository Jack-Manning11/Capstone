using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBoxSender : MonoBehaviour
{
    public string nameOfCharacter;

    [TextArea(1, 3)]
    public string[] mainDialogue;
    public string[] OGDialogue;

    public bool isQuestionAfter;

    public string[] preQuestionDialogue;
    public string[] postQuestionDialogueRight;
    public string[] postQuestionDialogueWrong;
    private string[] needMoreInfo = new string[1];
    public string question, answerA, answerB, answerC, answerD;
    public char correctAnswerChar;

    public int numberOfConversations = 0;

    public bool SuccessfulQuiz = false;

    [SerializeField] private GameObject dialogueBox;

    public bool canBeSelected = false;
    private bool movingCheck = false;

    private bool hasBeenTalkedTo = false;
    public DialogueBoxSender[] preQuizCheck; //see if there is a person that has to have been talked to first
    public bool preQuizCheckBool = false;

    private void Start()
    {
        needMoreInfo[0] = "**Looks like I need more information before I continue**";
        OGDialogue = mainDialogue;
    }
    public bool checkAllPreQuizChecks()
    {
        foreach (DialogueBoxSender dialogueBoxSender in preQuizCheck)
        {
            if (dialogueBoxSender.getHasBeenTalkedTo() == false)
            {
                return false;
            }
        }
        return true;
    }
    public bool getHasBeenTalkedTo()
    {
        return hasBeenTalkedTo;
    }

    private void OnTriggerEnter2D(Collider2D other) //When the player is in range of the dialogue box
    {
        //PLayer is in selecting range
        canBeSelected = true;
    }
    private void OnTriggerExit2D(Collider2D other) //When the player leaves the dialogue box
    {
        canBeSelected = false;
    }

    private void Update()
    {
        if (canBeSelected && !movingCheck && (Input.GetKeyDown(KeyCode.O) || dialogueBox.GetComponent<ControlManager>().select) && !dialogueBox.GetComponent<DialogueBox>().inConvo && mainDialogue != null) //If the player is in range, they press O, and they are not currently in a dialogue
        {
            dialogueBox.GetComponent<ControlManager>().select = false;
            
            hasBeenTalkedTo = true;
            TriggerDialogue();
        }
    }

    public void TriggerDialogue() //General call dialogue function.
    {
        if (checkAllPreQuizChecks() == true && !SuccessfulQuiz && mainDialogue != OGDialogue) numberOfConversations = 1;


        //After the first, Quiz with prechecks that are done
        if (numberOfConversations == 1 && isQuestionAfter && preQuizCheck != null && checkAllPreQuizChecks() == true && mainDialogue != preQuestionDialogue) //After the first conversation, needs to switch to prequestion dialogue (if there is any)
        {
            Debug.Log("TESTED");
            mainDialogue = preQuestionDialogue;
        }
        //After the first, Quiz with prechecks that arent done
        else if (numberOfConversations == 1 && isQuestionAfter && preQuizCheck != null && mainDialogue != preQuestionDialogue)
        {
            Debug.Log("PreQuestionDialogue, pre quiz check");
            numberOfConversations = 0;
            mainDialogue = needMoreInfo;
        }
        //After the first, quiz without prechecks
        else if (numberOfConversations == 1 && isQuestionAfter && preQuizCheck == null && mainDialogue != preQuestionDialogue)
        {
            Debug.Log("PreQuestionDialogue, no pre quiz check");
            mainDialogue = preQuestionDialogue;
        }
        //Quiz after successful completion
        else if (isQuestionAfter && SuccessfulQuiz && numberOfConversations == 3)
        {
            mainDialogue = postQuestionDialogueRight;
        }

            if (dialogueBox.GetComponent<DialogueBox>().moving == true) //If the box is moving, prevent the trigger until it stops moving (prevents movement overriding each other)
        {
            movingCheck = true;
            StartCoroutine(WaitSeconds());
        }
        else
        {
            numberOfConversations++;
            dialogueBox.GetComponent<DialogueBox>().StartDialogue(this);
        }
    }

    IEnumerator WaitSeconds() //Wait a set amount of time to allow the movement to finish
    {
        Debug.Log("Waiting");
        yield return new WaitForSeconds(1f);
        movingCheck = false;
    }

    public void PostQuizDialogueRight() //If the quiz is answered correctly, replace the dialogue with post question correct dialouge and then call the dialogue box
    {
        Debug.Log("Quiz was right");
        mainDialogue = postQuestionDialogueRight;
        dialogueBox.GetComponent<DialogueBox>().StartDialogue(this);
        SuccessfulQuiz = true;
    }

    public void PostQuizDialogueWrong() //If the quiz is answered incorrectly, replace the dialogue with post question wrong dialouge and then call the dialogue box
    {
        Debug.Log("Quiz was Wrong");
        numberOfConversations = 1;
        mainDialogue = postQuestionDialogueWrong;
        dialogueBox.GetComponent<DialogueBox>().StartDialogue(this);
        SuccessfulQuiz = false;
    }
}
