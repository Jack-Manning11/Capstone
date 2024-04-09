using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffaloBobber : MonoBehaviour
{
    public GameObject buffaloParent;

    void Start()
    {
        // Assuming "parentObject" is the parent GameObject
        Transform parentTransform = buffaloParent.transform;
        
        foreach (Transform childTransform in parentTransform)
        {
            GameObject childObject = childTransform.gameObject;
            if (childTransform.tag == "Buffalo") //So we dont count any other triggers or hint indicators
            {
                Debug.Log(childTransform.gameObject.name);
                StartCoroutine(RandomWaitThenStart(childObject));
            }
        }
    }

    IEnumerator RandomWaitThenStart(GameObject childObject)
    {
        float randomNumber = Random.Range(0f, 3f); //Randomized Delay
        yield return new WaitForSeconds(randomNumber);

        childObject.GetComponent<Animator>().enabled = true; //Start Animation
    }
}
