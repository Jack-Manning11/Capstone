using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> enteredObjects = new List<GameObject>();
    private IEnumerator reduceCoroutine;
    private IEnumerator increaseCoroutine;
    private float lerpSpeed = 1f;

    IEnumerator ReduceOpacity(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer == null)
            yield break;

        Material material = renderer.material;
        Color startColor = material.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0.2f);

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            material.color = Color.Lerp(startColor, targetColor, elapsedTime);
            elapsedTime += Time.deltaTime * lerpSpeed;
            yield return null;
        }

        material.color = targetColor;
    }

    // Coroutine to increase opacity back to 100%
    IEnumerator IncreaseOpacity(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer == null)
            yield break;

        Material material = renderer.material;
        Color startColor = material.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            material.color = Color.Lerp(startColor, targetColor, elapsedTime);
            elapsedTime += Time.deltaTime * lerpSpeed;
            yield return null;
        }

        material.color = targetColor;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        GameObject triggerObject = other.gameObject;
        if (!enteredObjects.Contains(triggerObject) && triggerObject.tag != "IgnoreOpacity"){
            enteredObjects.Add(triggerObject);
            reduceCoroutine = ReduceOpacity(triggerObject);
            StartCoroutine(reduceCoroutine);
            UpdateOrderInLayer();
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        GameObject triggerObject = other.gameObject;

        if (enteredObjects.Contains(triggerObject)){
            enteredObjects.Remove(triggerObject);
            increaseCoroutine = IncreaseOpacity(triggerObject);
            StartCoroutine(increaseCoroutine);
            UpdateOrderInLayer();
        }
    }

    private void UpdateOrderInLayer(){
      if(enteredObjects.Count > 0){
        int smallestOrder = int.MaxValue;
        GameObject targetObject = null;

        foreach (GameObject obj in enteredObjects)
        {
            if (obj.GetComponent<SpriteRenderer>() != null)
            {
                int orderInLayer = obj.GetComponent<SpriteRenderer>().sortingOrder;

                if (orderInLayer < smallestOrder)
                {
                    smallestOrder = orderInLayer;
                    targetObject = obj;
                }
            }
        }

        if (targetObject != null)
        {
            int playerOrder = targetObject.GetComponent<SpriteRenderer>().sortingOrder - 1;
            GetComponent<SpriteRenderer>().sortingOrder = playerOrder;
        }
      } else {
        GetComponent<SpriteRenderer>().sortingOrder = 999;
      }
    }
}
