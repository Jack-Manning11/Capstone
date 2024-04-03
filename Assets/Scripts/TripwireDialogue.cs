using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripwireDialogue : MonoBehaviour
{
    private int numberOfConvo = 0;
    [SerializeField] bool canBeRemoved;

    public DialogueBoxSender dialogueBoxSender;
    public DialougeQuiz dialogueQuiz;
    public GameObject ObjectToDestroy;

    private void OnTriggerEnter2D(Collider2D other) //When the player enters the dialogue box
    {
        numberOfConvo++;
        if (numberOfConvo == 1)
        {
            this.GetComponent<DialogueBoxSender>().TriggerDialogue();
        }
    }
    private void Update()
    {
        if (canBeRemoved && dialogueQuiz.senderCopy.nameOfCharacter == dialogueBoxSender.nameOfCharacter && dialogueBoxSender.SuccessfulQuiz == true)
        {
            ObjectToDestroy.SetActive(false);
        }
    }
}
