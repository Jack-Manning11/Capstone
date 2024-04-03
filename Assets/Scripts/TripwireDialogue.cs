using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripwireDialogue : MonoBehaviour
{
    private int numberOfConvo = 0;

    private void OnTriggerEnter2D(Collider2D other) //When the player enters the dialogue box
    {
        numberOfConvo++;
        if (numberOfConvo == 1)
        {
            this.GetComponent<DialogueBoxSender>().TriggerDialogue();
        }
    }
}
