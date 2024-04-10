using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TheEndgame : MonoBehaviour
{
    public DialogueBoxSender equipmentRoomSender;
    private bool canBeSelected = false;
    public NewMover player;
    public GameObject WinnerPhoto;
    public ControlManager controlManager;

    public GameObject ERHintIndicator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        canBeSelected = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        canBeSelected = false;
    }
    void Update()
    {
        if (equipmentRoomSender.getHasBeenTalkedTo())
        {
            this.GetComponent<DialogueBoxSender>().enabled = false;
        }
        
        if (equipmentRoomSender.getHasBeenTalkedTo() && canBeSelected && (Input.GetKeyDown(KeyCode.O) || controlManager.select))
        {
            player.moveLock = true;
            WinnerPhoto.SetActive(true);
            StartCoroutine(WaitThenJump());
        }

        if (!equipmentRoomSender.getHasBeenTalkedTo() && canBeSelected && (Input.GetKeyDown(KeyCode.O) || controlManager.select))
        {
            ERHintIndicator.SetActive(true);
        }
    }

    IEnumerator WaitThenJump()
    {
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene("credits");
    }
}
