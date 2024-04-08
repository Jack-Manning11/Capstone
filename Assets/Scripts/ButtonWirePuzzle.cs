using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ButtonWirePuzzle : MonoBehaviour
{
    public string currentWire;
    public string currentButton;

    public ControlManager controlManager;

    private void Start()
    {
        currentWire = "no";
        currentButton = "no";
    }
    public string getCurrentWire()
    {
        return currentWire;
    }
    public string getCurrentButton()
    {
        return currentButton;
    }

    private string winnerWire = "white";
    private string winnerButton = "purple";

    private bool showOnce = false;

    public GameObject door;
    public AudioSource doorSound;

    void Update()
    {
        if (currentWire == winnerWire &&  currentButton == winnerButton && showOnce == false)
        {
            Debug.Log("Victory");
            doorSound.Play();
            door.SetActive(false);
            showOnce = true;
        }
    }
}
