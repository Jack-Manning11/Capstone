using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class NewMover : MonoBehaviour
{
    public Rigidbody2D body;
    public SpriteRenderer spriteRenderer;

    public List<Sprite> nSprites;
    public List<Sprite> neSprites;
    public List<Sprite> eSprites;
    public List<Sprite> seSprites;
    public List<Sprite> sSprites;
    public List<Sprite> swSprites;
    public List<Sprite> wSprites;
    public List<Sprite> nwSprites;
    public List<Sprite> nIdleSprites;
    public List<Sprite> neIdleSprites;
    public List<Sprite> eIdleSprites;
    public List<Sprite> seIdleSprites;
    public List<Sprite> sIdleSprites;
    public List<Sprite> swIdleSprites;
    public List<Sprite> wIdleSprites;
    public List<Sprite> nwIdleSprites;

    public bool moveLock = false;

    public float walkSpeed;
    public float frameRate;
    private float idleTime;
    private string lastMoved = "South";
    private bool isMoving = false;

    private Vector2 direction;

    public ControlManager controlManager;

    private int directionUpDown = 0;
    private int directionLeftRight = 0;

    Vector2 move;
    void Update() {
    if(moveLock == false){
        if (controlManager.moveUp)
        {
            directionUpDown = 1;
            move = new Vector2 (directionLeftRight, directionUpDown);
        }
        if (controlManager.moveDown)
        {
            directionUpDown = -1;
            move = new Vector2(directionLeftRight, directionUpDown);
        }
        if (controlManager.moveLeft)
        {
            directionLeftRight = -1;
            move = new Vector2(directionLeftRight, directionUpDown);
        }
        if (controlManager.moveRight)
        {
            directionLeftRight = 1;
            move = new Vector2(directionLeftRight, directionUpDown);
        }
        if (!controlManager.moveUp && !controlManager.moveDown && !controlManager.moveLeft && !controlManager.moveRight)
        {
            directionUpDown = 0;
            directionLeftRight = 0;
            move = new Vector2(directionLeftRight, directionUpDown);
        }

        direction = move.normalized;
        walkSpeed = 2.5f;
    } else {
        direction = Vector2.zero; // Set direction to zero if moveLock is true
        isMoving = false; // Set isMoving to false when moveLock is true
        walkSpeed = 0f;
    }
    if (move.x > 0 && move.y > 0)
    {
    // If moving diagonally, divide the vertical velocity by 2
      direction.y /= 2f;
    }
    body.velocity = direction * walkSpeed;
    // Check if the player is moving (if velocity is non-zero)
    if (body.velocity != Vector2.zero) {
        isMoving = true;
    }

    List<Sprite> directionSprites = GetSprites();

    if(isMoving){
        float playTime = Time.time - idleTime;
        int frame = Mathf.RoundToInt(playTime * frameRate) % 8;

        spriteRenderer.sprite = directionSprites[frame];
    } else {
        idleTime = Time.time;
        List<Sprite> idleSprites = GetIdleSprites();
        int frame = Mathf.RoundToInt(idleTime * frameRate) % 8;

        spriteRenderer.sprite = idleSprites[frame];
    }
}

    List<Sprite> GetSprites(){
      List<Sprite> selectedSprites = null;
      isMoving = false;

      if(direction.y > 0){
        isMoving = true;
        if(direction.x > 0){
          selectedSprites = neSprites;
          lastMoved = "NorthEast";
        } else if (direction.x < 0){
          selectedSprites = nwSprites;
          lastMoved = "NorthWest";
        } else {
          selectedSprites = nSprites;
          lastMoved = "North";
        }
      } else if (direction.y < 0){
        isMoving = true;
        if(direction.x > 0){
          selectedSprites = seSprites;
          lastMoved = "SouthEast";
        } else if (direction.x < 0){
          selectedSprites = swSprites;
          lastMoved = "SouthWest";
        } else {
          selectedSprites = sSprites;
          lastMoved = "South";
        }
      } else {
        if(direction.x > 0){
          isMoving = true;
          selectedSprites = eSprites;
          lastMoved = "East";
        } else if (direction.x < 0){
          isMoving = true;
          selectedSprites = wSprites;
          lastMoved = "West";
        }
      }

      return selectedSprites;
    }

    List<Sprite> GetIdleSprites(){
      if(lastMoved == "South"){
        return sIdleSprites;
      } else if(lastMoved == "North"){
        return nIdleSprites;
      } else if(lastMoved == "East"){
        return eIdleSprites;
      } else if(lastMoved == "West"){
        return wIdleSprites;
      } else if(lastMoved == "NorthEast"){
        return neIdleSprites;
      } else if(lastMoved == "SouthEast"){
        return seIdleSprites;
      } else if(lastMoved == "SouthWest"){
        return swIdleSprites;
      } else {
        return nwIdleSprites;
      }
    }

    public void setMoveLock(bool locked){
      moveLock = locked;
    }
}
