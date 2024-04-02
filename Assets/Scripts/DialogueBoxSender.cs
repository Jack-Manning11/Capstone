using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBoxSender : MonoBehaviour
{
    public string nameOfCharacter;

    [TextArea(1, 3)]
    public string[] mainDialogue;

    public bool isQuestionAfter;

    public string[] preQuestionDialogue;
    public string[] postQuestionDialogueRight;
    public string[] postQuestionDialogueWrong;
    public string question, answerA, answerB, answerC, answerD;
    public char correctAnswerChar;

    public int numberOfConversations = 0;

    public bool SuccessfulQuiz = false;

    [SerializeField] private GameObject dialogueBox;

    private bool canBeSelected = false;

    private bool hasBeenTalkedTo = false;
    public DialogueBoxSender preQuizCheck; //see if there is a person that has to have been talked to first
    public bool preQuizCheckBool = false;
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
        if (canBeSelected && Input.GetKeyDown(KeyCode.O) && !dialogueBox.GetComponent<DialogueBox>().inConvo && mainDialogue != null) //If the player is in range, they press O, and they are not currently in a dialogue
        {
            hasBeenTalkedTo = true;
            numberOfConversations++;
            TriggerDialogue();
        }
        /*
        if (numberOfConversations == 2 && isQuestionAfter && preQuizCheck != null && preQuizCheck.getHasBeenTalkedTo() == true && mainDialogue != preQuestionDialogue) //After the first conversation, needs to switch to prequestion dialogue (if there is any)
        {
            Debug.Log("TESTED");
            mainDialogue = preQuestionDialogue;
        }
        else if (numberOfConversations == 2 && isQuestionAfter && preQuizCheck == null && mainDialogue != preQuestionDialogue)
        {
            Debug.Log("PreQuestionDialogue, no pre quiz check");
            mainDialogue = preQuestionDialogue;
        }
        */
    }

    public void TriggerDialogue() //General call dialogue function.
    {
        if (numberOfConversations == 2 && isQuestionAfter && preQuizCheck != null && preQuizCheck.getHasBeenTalkedTo() == true && mainDialogue != preQuestionDialogue) //After the first conversation, needs to switch to prequestion dialogue (if there is any)
        {
            Debug.Log("TESTED");
            mainDialogue = preQuestionDialogue;
        }
        else if (numberOfConversations == 2 && isQuestionAfter && preQuizCheck == null && mainDialogue != preQuestionDialogue)
        {
            Debug.Log("PreQuestionDialogue, no pre quiz check");
            mainDialogue = preQuestionDialogue;
        }
        else if (numberOfConversations == 2 && isQuestionAfter && preQuizCheck != null && mainDialogue != preQuestionDialogue)
        {
            Debug.Log("PreQuestionDialogue, pre quiz check");
            mainDialogue = preQuestionDialogue;
        }
        else if (isQuestionAfter && numberOfConversations == 3)
        {
            mainDialogue = postQuestionDialogueRight;
        }

        if (dialogueBox.GetComponent<DialogueBox>().moving == true) //If the box is moving, prevent the trigger until it stops moving (prevents movement overriding each other)
        {
            canBeSelected = false;
            StartCoroutine(WaitSeconds());
        }
        else
        {
            dialogueBox.GetComponent<DialogueBox>().StartDialogue(this);
        }

    }

    IEnumerator WaitSeconds() //Wait a set amount of time to allow the movement to finish
    {
        Debug.Log("Waiting");
        yield return new WaitForSeconds(0.5f);
        canBeSelected = true;
        dialogueBox.GetComponent<DialogueBox>().StartDialogue(this);
    }

    public void PostQuizDialogueRight() //If the quiz is answered correctly, replace the dialogue with post question correct dialouge and then call the dialogue box
    {
        mainDialogue = postQuestionDialogueRight;
        SuccessfulQuiz = true;

        if (dialogueBox.GetComponent<DialogueBox>().moving == true)
        {
            StartCoroutine(WaitSeconds());
        }
        else
        {
            dialogueBox.GetComponent<DialogueBox>().StartDialogue(this);
        }
    }

    public void PostQuizDialogueWrong() //If the quiz is answered incorrectly, replace the dialogue with post question wrong dialouge and then call the dialogue box
    {
        mainDialogue = postQuestionDialogueWrong;
        SuccessfulQuiz = false;
        if (dialogueBox.GetComponent<DialogueBox>().moving == true)
        {
            StartCoroutine(WaitSeconds());
        }
        else
        {
            dialogueBox.GetComponent<DialogueBox>().StartDialogue(this);
        }
    }
}
