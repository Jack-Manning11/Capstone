using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonWireChild : MonoBehaviour
{
    public string type; //button or wire
    public string color; //Color

    public DialogueBoxSender puzzleSender;
    public ButtonWirePuzzle puzzle;
    private string[] newDialogue = new string[1];

    private bool canBeSelected = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        canBeSelected = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        canBeSelected = false;
    }
    void Update()
    {
        if (canBeSelected && Input.GetKeyDown(KeyCode.O))
        {
            if (type == "wire")
            {
                puzzle.currentWire = color;
                Debug.Log("**You now have " + puzzle.getCurrentWire() + " wire and " + puzzle.getCurrentButton() + " button**");
                newDialogue[0] = "**You now have " + puzzle.getCurrentWire() + " wire and " + puzzle.getCurrentButton() + " button**";
                puzzleSender.mainDialogue = newDialogue;
                puzzleSender.TriggerDialogue();
            }
            else if (type == "button")
            {
                puzzle.currentButton = color;
                Debug.Log("**You now have " + puzzle.getCurrentWire() + " wire and " + puzzle.getCurrentButton() + " button**");
                newDialogue[0] = "**You now have " + puzzle.getCurrentWire() + " wire and " + puzzle.getCurrentButton() + " button**";
                puzzleSender.mainDialogue = newDialogue;
                puzzleSender.TriggerDialogue();
            }
            else
            {
                Debug.Log("invalid Type");
            }
        }
    }
}
