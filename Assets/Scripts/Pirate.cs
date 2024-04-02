using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pirate : MonoBehaviour
{
    public DialogueBoxSender PirateDialogue;
    public Transporter elevator;

    private void OnTriggerExit2D(Collider2D other) //When the player leaves the dialogue box
    {
        if (elevator.getGameStage() != 4) //Once the puzzle is solved this all goes away
        {
            PirateDialogue.TriggerDialogue();
            this.GetComponent<PolygonCollider2D>().isTrigger = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (puzzle solved)
        {
            this.GetComponent<PolygonCollider2D>().isTrigger = false;
            elevator.setGameStage(4);
            PirateDialogue.SetActive(false);
        }
        */
    }
}
