using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARGIndicator : MonoBehaviour
{
    public GameObject indicator;
    private void OnTriggerEnter2D(Collider2D other)
    {
        indicator.GetComponent<SpriteRenderer>().enabled = true;
        indicator.GetComponent<Animator>().enabled = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        indicator.GetComponent<SpriteRenderer>().enabled = false;
        indicator.GetComponent<Animator>().enabled = false;
    }
}
