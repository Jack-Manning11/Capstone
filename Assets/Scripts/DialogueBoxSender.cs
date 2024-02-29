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

    [SerializeField] private GameObject dialogueBox;

    private bool canBeSelected = false;

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
        if (canBeSelected && Input.GetKeyDown(KeyCode.O) && !dialogueBox.GetComponent<DialogueBox>().inConvo) //If the player is in range, they press O, and they are not currently in a dialogue
        {
            numberOfConversations++;
            TriggerDiaglouge();
        }
    }

    public void TriggerDiaglouge() //General call dialogue function. 
    {
        if (numberOfConversations == 2 && isQuestionAfter) //After the first conversation, needs to switch to prequestion dialogue (if there is any)
        {
            mainDialogue = preQuestionDialogue;
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
        yield return new WaitForSeconds(2);
        canBeSelected = true;
        dialogueBox.GetComponent<DialogueBox>().StartDialogue(this);
    }

    public void PostQuizDialogueRight() //If the quiz is answered correctly, replace the dialogue with post question correct dialouge and then call the dialogue box
    {
        mainDialogue = postQuestionDialogueRight;
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
