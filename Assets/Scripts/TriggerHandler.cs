using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
  private void OnTriggerEnter2D(Collider2D other)
  {

          // Assuming "Player" is the tag of your player object.
          // You can modify the condition based on your actual tag or other criteria.

          // Notify the player's OrderHandler script about the trigger.
          other.GetComponent<OrderHandler>().OnTriggerEnter2D(this.GetComponent<PolygonCollider2D>());

  }

  private void OnTriggerExit2D(Collider2D other)
  {

          // Notify the player's OrderHandler script about leaving the trigger.
          other.GetComponent<OrderHandler>().OnTriggerExit2D(this.GetComponent<PolygonCollider2D>());

  }
}
