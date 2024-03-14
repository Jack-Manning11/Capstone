using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unlockableDoor : MonoBehaviour
{
    [SerializeField] private GameObject popup;
    [SerializeField] private GameObject[] highlights;
    [SerializeField] private GameObject[] numberSlots;
    private int[] indexes = {0,0,0,0};
    [SerializeField] private Sprite[] numberSprites;
    [SerializeField] private bool inRange = false;
    private bool isInteracting = false;
    private int pos = 0;
    private bool solved = false;
    private int[] solution = {1,1,2,5};
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject doorToUnlock;

    void Update()
    {
      if(checkWin()){
          doorToUnlock.GetComponent<SpriteRenderer>().enabled = false;
          PolygonCollider2D[] polygonColliders = doorToUnlock.GetComponents<PolygonCollider2D>();
          foreach(PolygonCollider2D collider in polygonColliders){
            collider.enabled = false;
          }
          PolygonCollider2D trigger = GetComponent<PolygonCollider2D>();
          trigger.enabled = false;
          isInteracting = false;
          pos = -1;
          clearHighlights();
          SpriteRenderer popupSprite = popup.GetComponent<SpriteRenderer>();
          popupSprite.enabled = false;
          indexes = new int[] {0, 0, 0, 0};
          foreach (GameObject number in numberSlots){
            number.GetComponent<SpriteRenderer>().enabled = false;
          }
          player.GetComponent<NewMover>().setMoveLock(false);
      }
      if(isInteracting){
        if(Input.GetKeyDown(KeyCode.P)){
          isInteracting = false;
          pos = -1;
          clearHighlights();
          SpriteRenderer popupSprite = popup.GetComponent<SpriteRenderer>();
          popupSprite.enabled = false;
          foreach (GameObject number in numberSlots){
            number.GetComponent<SpriteRenderer>().enabled = false;
          }
          player.GetComponent<NewMover>().setMoveLock(false);
        }
        if(pos == 0){
          clearHighlights();
          highlights[0].GetComponent<SpriteRenderer>().enabled = true;
          if(Input.GetKeyDown(KeyCode.O)){
            if(indexes[0] != 9){
              indexes[0] = indexes[0] + 1;
            } else {
              indexes[0] = 0;
            }
            numberSlots[0].GetComponent<SpriteRenderer>().sprite = numberSprites[indexes[0]];
          }
          if(Input.GetKeyDown(KeyCode.D)){
            pos = 1;
          }
        } else if (pos == 1){
          clearHighlights();
          highlights[1].GetComponent<SpriteRenderer>().enabled = true;
          if(Input.GetKeyDown(KeyCode.O)){
            if(indexes[1] != 9){
              indexes[1] = indexes[1] + 1;
            } else {
              indexes[1] = 0;
            }
            numberSlots[1].GetComponent<SpriteRenderer>().sprite = numberSprites[indexes[1]];
          }
          if(Input.GetKeyDown(KeyCode.D)){
            pos = 2;
          } else if (Input.GetKeyDown(KeyCode.A)) {
            pos = 0;
          }
        } else if (pos == 2){
          clearHighlights();
          highlights[2].GetComponent<SpriteRenderer>().enabled = true;
          if(Input.GetKeyDown(KeyCode.O)){
            if(indexes[2] != 9){
              indexes[2] = indexes[2] + 1;
            } else {
              indexes[2] = 0;
            }
            numberSlots[2].GetComponent<SpriteRenderer>().sprite = numberSprites[indexes[2]];
          }
          if(Input.GetKeyDown(KeyCode.D)){
            pos = 3;
          } else if (Input.GetKeyDown(KeyCode.A)) {
            pos = 1;
          }
        } else if (pos == 3){
          clearHighlights();
          highlights[3].GetComponent<SpriteRenderer>().enabled = true;
          if(Input.GetKeyDown(KeyCode.O)){
            if(indexes[3] != 9){
              indexes[3] = indexes[3] + 1;
            } else {
              indexes[3] = 0;
            }
            numberSlots[3].GetComponent<SpriteRenderer>().sprite = numberSprites[indexes[3]];
          }
          if(Input.GetKeyDown(KeyCode.A)){
            pos = 2;
          }
        }

      }
      if(inRange && Input.GetKeyDown(KeyCode.O) && !isInteracting) {
        SpriteRenderer popupSprite = popup.GetComponent<SpriteRenderer>();
        player.GetComponent<NewMover>().setMoveLock(true);
        popupSprite.enabled = true;
        pos = 0;
        foreach (GameObject number in numberSlots){
          number.GetComponent<SpriteRenderer>().enabled = true;
        }
        isInteracting = true;
      }
    }

    private void clearHighlights(){
      foreach(GameObject highlight in highlights){
        highlight.GetComponent<SpriteRenderer>().enabled = false;
      }
    }

    public void OnTriggerEnter2D(Collider2D other){
      inRange = true;
    }

    public void OnTriggerExit2D(Collider2D other){
      inRange = false;
    }

    private bool checkWin(){
      for(int i = 0; i < 4; i++){
        if(indexes[i] != solution[i]){
          return false;
        }
      }
      return true;
    }
}
