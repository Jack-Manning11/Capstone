using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBoxSender : MonoBehaviour
{
    public string nameOfCharacter;

    [TextArea(1, 3)]
    public string[] mainDialogue;
    public string[] preQuestionDialogue;
    public string[] postQuestionDialogue;

    public int numberOfConversations = 0;

    public string question, answerA, answerB, answerC, answerD;
    public int correctAnswerIndex;

    public bool isQuestionAfter;

    [SerializeField] private GameObject dialogueBox;

    private bool canBeSelected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //PLayer is in selecting range
        canBeSelected = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canBeSelected = false;
    }

    private void Update()
    {
        if (canBeSelected && Input.GetKeyDown(KeyCode.O) && !dialogueBox.GetComponent<DialogueBox>().inConvo)
        {
            numberOfConversations++;
            TriggerDiaglouge();
        }
    }

    public void TriggerDiaglouge()
    {
        if (numberOfConversations == 2 && isQuestionAfter)
        {
            mainDialogue = preQuestionDialogue;
        }
        dialogueBox.GetComponent<DialogueBox>().StartDialogue(this);
    }

    public void SuccessfulQuiz()
    {
        //Send some information about next steps. 
    }
}
