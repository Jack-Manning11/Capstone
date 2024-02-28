using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed = 5f; //Controls velocity multiplier

    Rigidbody2D rb; //Tells script there is a rigidbody, we can use variable rb to reference it in further script

    public Animator animator;

    private float changeX;
    private float changeY;

    private bool allowMovement = true;

    private bool UnlockedArea = false;

    [SerializeField] private bool specailMover;

    static Vector3 startingPos = Vector3.zero;

    public bool checker;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //rb equals the rigidbody on the player
        rb.freezeRotation = true;

        if (specailMover == false)
        {
            this.transform.position = startingPos;
        }

    }


    // Update is called once per frame
    void Update()
    {
        if (allowMovement == true)
        {
            //Moving Code
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) //Down Left 1
            {
                rb.velocity = new Vector3(-1, -0.5f, 0) * speed;
                animator.SetFloat("Direction", 1);
            }
            else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) //Down Right 2
            {
                rb.velocity = new Vector3(1, -0.5f, 0) * speed;
                animator.SetFloat("Direction", 2);
            }
            else if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) //Up Left 3
            {
                rb.velocity = new Vector3(-1, 0.5f, 0) * speed;
                animator.SetFloat("Direction", 3);
            }
            else if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) //Up R 4
            {
                rb.velocity = new Vector3(1, 0.5f, 0) * speed;
                animator.SetFloat("Direction", 4);
            }
            else if (Input.GetKey(KeyCode.S)) //Down 5
            {
                rb.velocity = new Vector3(0, -1, 0) * speed;
                animator.SetFloat("Direction", 5);
            }
            else if (Input.GetKey(KeyCode.W)) //Up 6
            {
                rb.velocity = new Vector3(0, 1, 0) * speed;
                animator.SetFloat("Direction", 6);
            }
            else if (Input.GetKey(KeyCode.A)) //Left 7
            {
                rb.velocity = new Vector3(-1, 0, 0) * speed;
                animator.SetFloat("Direction", 7);
            }
            else if (Input.GetKey(KeyCode.D)) //Right 8
            {
                rb.velocity = new Vector3(1, 0, 0) * speed;
                animator.SetFloat("Direction", 8);
            }
            else //0
            {
                rb.velocity = new Vector3(0, 0, 0) * speed;
                animator.SetFloat("Direction", 0);
            }


            float xMove = Input.GetAxisRaw("Horizontal"); // d key changes value to 1, a key changes value to -1
            float yMove = Input.GetAxisRaw("Vertical"); // w key changes value to 1, s key changes value to -1

            //rb.velocity = new Vector3(xMove, yMove / 2, 0) * speed; // Creates velocity in direction of value equal to keypress (WASD). rb.velocity.y deals with falling + jumping by setting velocity to y.
        }
    }

    public void setAllowMovement(bool set)
    {
        allowMovement = set;
    }

    public void setUnlockedArea(bool set)
    {
        UnlockedArea = set;
    }

    public bool getUnlockedArea()
    {
        return UnlockedArea;
    }
    public void setStartingPos(Vector3 startingcoords)
    {
        startingPos = startingcoords;
    }
}
