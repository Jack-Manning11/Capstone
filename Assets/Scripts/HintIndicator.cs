using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintIndicator : MonoBehaviour
{
    public List<GameObject> indicators = new List<GameObject>();

    private bool held = false;

    private bool indicatorsOn = false;

    private float timer;

    public float timeToHold;

    public ControlManager controlManager;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || controlManager.back) held = true;
        if (Input.GetKeyUp(KeyCode.P) || controlManager.back) held = false;

        if (held)
        {
            timer += Time.deltaTime;

            if (timer >= timeToHold && indicatorsOn == false)
            {
                foreach (GameObject indicator in indicators)
                {
                    if (indicator.activeInHierarchy)
                    {
                        indicator.GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
                indicatorsOn = true;
            }
        }
        else
        {
            indicatorsOn = false;
            timer = 0;
            foreach (GameObject indicator in indicators)
            {
                if (indicator.activeInHierarchy)
                {
                    indicator.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }
        
    }
}
