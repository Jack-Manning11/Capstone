using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : MonoBehaviour
{
    [SerializeField] GameObject secondCow;

    public DialogueBoxSender dialogueBoxSender;
    public DialougeQuiz dialogueQuiz;
    public GameObject firstCow;

    private void Update()
    {
        if (dialogueQuiz.senderCopy.nameOfCharacter == dialogueBoxSender.nameOfCharacter && dialogueBoxSender.SuccessfulQuiz == true)
        {
            Debug.Log("Should Disable");
            dialogueQuiz.senderCopy = dialogueQuiz.burnerBox;
            firstCow.SetActive(false);
            secondCow.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D other) //When the player leaves the dialogue box
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
        secondCow.SetActive(true);
    }
}
