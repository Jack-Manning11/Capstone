using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pirate : MonoBehaviour
{
    public DialogueBoxSender PirateDialogue;
    public GameObject PirateCollider;
    public Transporter elevator;

    public GameObject PuzzleSolution;

    private string[] newPirateText;

    private void Start()
    {
        newPirateText[0] = "YAAARRRRR, Thanks for finding me treasure!!!!";
    }

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
        if (PuzzleSolution.GetComponent<PirateSolution>().solved)
        {
            PirateCollider.GetComponent<PolygonCollider2D>().isTrigger = true;
            PirateDialogue.mainDialogue = newPirateText;
            elevator.setGameStage(4);
            //this.GetComponent<GameObject>().SetActive(false);
        }
    }
}
