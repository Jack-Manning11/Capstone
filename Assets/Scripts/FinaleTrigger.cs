using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinaleTrigger : MonoBehaviour
{
    public GameObject Finale;
    private bool runOnce = true;
    [SerializeField] ScreenShake shakeCamera;
    public DialogueBoxSender FinaleDialogue;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (runOnce)
        {
            Finale.SetActive(true);
            shakeCamera.BeginShake(0f, 0f);
            FinaleDialogue.TriggerDialogue();
            runOnce = false;
        }
    }
}
