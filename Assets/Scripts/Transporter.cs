using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Transporter : MonoBehaviour
{
    public Vector2 targetPosition;

    [SerializeField] private GameObject ElevatorUIOffUp;
    [SerializeField] private GameObject ElevatorUIOffDown;
    [SerializeField] private GameObject UpUiOn;
    [SerializeField] private GameObject DownUIOn;

    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Elevator;
    [SerializeField] private List<GameObject> ElevatorList = new List<GameObject>();

    [SerializeField] private int TransporterFloor; //-1 for 1b, 0 for main, 1 for 2nd floor

    private bool inElevator = false;
    private bool playerSelectingDirection = false;

    private bool upIsOn = false;
    private bool downIsOn = false;

    private int selectedButton = 2; //0 for up, 1 for down

    [SerializeField] private float moveDistance; //Needs to stay consistant between floors

    public Animator animator;

    public bool movingUp = false;
    public bool movingDown = false;

    private bool notMoving = true;

    float t;
    Vector3 startPosition;
    Vector3 ElevatorStartPosition;
    Vector3 target;
    Vector3 elevatorTarget;

    private int premoveSortingOrder;
    public List<GameObject> elevatorItems;

    private void OnTriggerEnter2D(Collider2D other)
    {
        playerSelectingDirection = false;
        selectedButton = 2;
        inElevator = true;
        Debug.Log("enter floor " + TransporterFloor);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ElevatorUIOffDown.GetComponent<SpriteRenderer>().enabled = false;
        ElevatorUIOffUp.GetComponent<SpriteRenderer>().enabled = false;
        UpUiOn.GetComponent<SpriteRenderer>().enabled = false;
        DownUIOn.GetComponent<SpriteRenderer>().enabled = false;
        inElevator = false;
        Debug.Log("Exit floor " + TransporterFloor);
    }

    private void Update()
    {
        if (inElevator && notMoving)
        {
            //Debug.Log("in the elevator");

            //Wait for player to select
            if (Input.GetKeyUp(KeyCode.O) && playerSelectingDirection == false)
            {
                animator.SetBool("Open", false); //Close Door

                ElevatorUIOffDown.GetComponent<SpriteRenderer>().enabled = true;
                ElevatorUIOffUp.GetComponent<SpriteRenderer>().enabled = true;

                playerSelectingDirection = true; //PLayer has begun the direction selection process
                Player.GetComponent<NewMover>().enabled = false;

                selectedButton = 0;
            }

            //Once the player has chosen to move:
            else if (playerSelectingDirection == true)
            {
                if (Input.GetKeyUp(KeyCode.W))
                {
                    selectedButton = 0;
                }
                if (Input.GetKeyUp(KeyCode.S))
                {
                    selectedButton = 1;
                }
                if (Input.GetKeyUp(KeyCode.P)) //Exit the elevator
                {
                    animator.SetBool("Open", true); //Open Door

                    UpUiOn.GetComponent<SpriteRenderer>().enabled = false;
                    DownUIOn.GetComponent<SpriteRenderer>().enabled = false;
                    // Player.GetComponent<NewMover>().enabled = true;
                    StartCoroutine(PlayerMoveUnlock());
                    playerSelectingDirection = false;
                    selectedButton = 2;
                }

                //Update the lights to reflect direction
                if (selectedButton == 0 && upIsOn == false) //Up
                {
                    downIsOn = false;
                    upIsOn = true;
                    UpUiOn.GetComponent<SpriteRenderer>().enabled = true;
                    DownUIOn.GetComponent<SpriteRenderer>().enabled = false;
                }
                if (selectedButton == 1 && downIsOn == false) //Down
                {
                    downIsOn = true;
                    upIsOn = false;
                    UpUiOn.GetComponent<SpriteRenderer>().enabled = false;
                    DownUIOn.GetComponent<SpriteRenderer>().enabled = true;
                }
            }

            //Player has chosen direction
            if (Input.GetKeyDown(KeyCode.O) && playerSelectingDirection == true)
            {
                if (selectedButton == 0)
                {
                    Debug.Log("UP");
                    if (TransporterFloor != 1)
                    {
                        //Movement Stuff
                        movingUp = true;
                        inElevator = false;
                        notMoving = false;
                        t = 0;
                        startPosition = Player.transform.position;
                        ElevatorStartPosition = Elevator.transform.position;
                        target = new Vector3(startPosition.x, startPosition.y + moveDistance, 0);
                        elevatorTarget = new Vector3(Elevator.transform.position.x, Elevator.transform.position.y + moveDistance, 0);
                        premoveSortingOrder = Player.GetComponent<SpriteRenderer>().sortingOrder;
                    }
                    else
                    {
                        Debug.Log("Already at the top floor");
                    }
                }
                else if (selectedButton == 1)
                {
                    Debug.Log("DOWN");
                    if (TransporterFloor != -1)
                    {
                        //Movement Stuff
                        movingDown = true;
                        inElevator = false;
                        notMoving = false;
                        t = 0;
                        startPosition = Player.transform.position;
                        ElevatorStartPosition = Elevator.transform.position;
                        target = new Vector3(startPosition.x, startPosition.y - moveDistance, 0);
                        elevatorTarget = new Vector3(Elevator.transform.position.x, Elevator.transform.position.y - moveDistance,0);
                        premoveSortingOrder = Player.GetComponent<SpriteRenderer>().sortingOrder;
                    }
                    else
                    {
                        Debug.Log("Already at the bottom floor");
                    }
                }
            }
        }
        else if (movingUp == true)
        {

            t += Time.deltaTime / 10;
            Player.transform.position = Vector3.Lerp(startPosition, target, t);
            Elevator.transform.position = Vector3.Lerp(ElevatorStartPosition, elevatorTarget, t);

            if (Player.transform.position.y == target.y) //DesinationReacher
            {
                Debug.Log("Reached Up");
                notMoving = true;
                selectedButton = 2;
                movingUp = false;
                animator.SetBool("Open", true); //Open Door
                StartCoroutine(ShiftElevatorLayer());
                StartCoroutine(PlayerMoveUnlock());
            }
        }
        else if (movingDown == true)
        {

            t += Time.deltaTime / 10;
            Player.transform.position = Vector3.Lerp(startPosition, target, t);
            Elevator.transform.position = Vector3.Lerp(ElevatorStartPosition, elevatorTarget, t);

            if (Player.transform.position.y == target.y)
            {
                Debug.Log("Reached Down");
                notMoving = true;
                selectedButton = 2;
                movingDown = false;
                animator.SetBool("Open", true); //Open Door
                StartCoroutine(ShiftElevatorLayer());
                StartCoroutine(PlayerMoveUnlock());
            }
        }
    }

    IEnumerator PlayerMoveUnlock()
    {
        Debug.Log("StartWait");
        yield return new WaitForSeconds(3.0f);
        Debug.Log("EndWait");
        Player.GetComponent<NewMover>().enabled = true;
    }

    IEnumerator ShiftElevatorLayer()
    {
        foreach(GameObject elevatorPiece in elevatorItems)
        {
            elevatorPiece.GetComponent<SpriteRenderer>().sortingOrder = elevatorPiece.GetComponent<SpriteRenderer>().sortingOrder + (Player.GetComponent<SpriteRenderer>().sortingOrder - premoveSortingOrder);
        }
        yield return null;
    }
}
