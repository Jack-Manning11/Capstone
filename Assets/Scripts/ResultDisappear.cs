using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultDisappear : MonoBehaviour
{
    public DialogueBoxSender dialogueBoxSender;
    public DialougeQuiz dialogueQuiz;
    public GameObject ObjectToShutdown;

    private void Update()
    {
        if (dialogueQuiz.senderCopy.nameOfCharacter == dialogueBoxSender.nameOfCharacter && dialogueBoxSender.SuccessfulQuiz == true)
        {
            Debug.Log("Should Disable");
            dialogueBoxSender.SuccessfulQuiz = false;
            dialogueQuiz.senderCopy = dialogueQuiz.burnerBox;
            ObjectToShutdown.SetActive(false);
        }
    }
}
