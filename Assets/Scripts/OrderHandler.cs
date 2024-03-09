using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> enteredObjects = new List<GameObject>();
    private int baseLayer = 496;

    public void OnTriggerEnter2D(Collider2D other)
    {
        GameObject triggerObject = other.gameObject;
        if (!enteredObjects.Contains(triggerObject) && triggerObject.tag != "Exempt"){
            enteredObjects.Add(triggerObject);
            UpdateOrderInLayer();
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        GameObject triggerObject = other.gameObject;

        if (enteredObjects.Contains(triggerObject)){
            enteredObjects.Remove(triggerObject);
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
        GetComponent<SpriteRenderer>().sortingOrder = 600;
      }
    }

    public void addObjects(GameObject[] gameObjects){
        foreach (GameObject obj in gameObjects){
            enteredObjects.Add(obj);
        }
    }

    public void removeObjects(GameObject[] gameObjects){
        foreach (GameObject obj in gameObjects){
            enteredObjects.Remove(obj);
        }
    }
}
