using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateSolution : MonoBehaviour
{
    private bool canBeSelected = false;

    public bool solved = false;

    public ControlManager controlManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        canBeSelected = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canBeSelected = false;
    }
    private void Update()
    {
        if (canBeSelected && (Input.GetKeyDown(KeyCode.O) || controlManager.select))
        {
            solved = true;
        }
    }
}
