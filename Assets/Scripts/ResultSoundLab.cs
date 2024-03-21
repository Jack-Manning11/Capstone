using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultSoundLab : MonoBehaviour
{
    public DialogueBoxSender dialogueBoxSender;
    public DialougeQuiz dialogueQuiz;
    public Transporter elevator;

    void Update()
    {
        if (dialogueQuiz.senderCopy.nameOfCharacter == dialogueBoxSender.nameOfCharacter && dialogueQuiz.SuccessfulQuiz == true)
        {
            Debug.Log("Should Disable");
            dialogueQuiz.SuccessfulQuiz = false;
            dialogueQuiz.senderCopy = dialogueQuiz.burnerBox;
            elevator.setSoundLabDone(true);
        }
    }
}
