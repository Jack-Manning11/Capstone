using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Transporter : MonoBehaviour
{
        public Vector2 targetPosition;

        [SerializeField] private GameObject ElevatorUIOffUp;
        [SerializeField] private GameObject ElevatorUIOffDown;
        [SerializeField] private GameObject ElevatorUIOffDownCracked;
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

        //Story Based Controls
        private int gameStage = 1;
        //1 is the first floor (elevator only goes up)
        //2 is the second floor (evevator is broken until the sound puzzle is solved)
        private bool soundLabDone = false;
        public void setSoundLabDone(bool b)
        {
            soundLabDone = b;
        }

        public SmoothCamera mainCamera;

        public AudioSource bgMusic;
        public AudioSource elevatorMusic;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameStage != 1) ElevatorUIOffDown.GetComponent<SpriteRenderer>().enabled = true;
        else ElevatorUIOffDownCracked.GetComponent<SpriteRenderer>().enabled = true;
        ElevatorUIOffUp.GetComponent<SpriteRenderer>().enabled = true;
        playerSelectingDirection = false;
        selectedButton = 2;
        inElevator = true;
        Debug.Log("enter floor " + TransporterFloor);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ElevatorUIOffDown.GetComponent<SpriteRenderer>().enabled = false;
        ElevatorUIOffDownCracked.GetComponent<SpriteRenderer>().enabled = false;
        ElevatorUIOffUp.GetComponent<SpriteRenderer>().enabled = false;
        UpUiOn.GetComponent<SpriteRenderer>().enabled = false;
        DownUIOn.GetComponent<SpriteRenderer>().enabled = false;
        inElevator = false;
        Debug.Log("Exit floor " + TransporterFloor);
    }

    private void Update()
    {
        //Should happen at any point
        if (soundLabDone && gameStage < 2) gameStage = 2;

        //Standing in the Elevatoy
        if (inElevator && notMoving)
        {
            //Wait for player to select
            if (Input.GetKeyUp(KeyCode.O) && playerSelectingDirection == false)
            {
                animator.SetBool("Open", false); //Close Door

                playerSelectingDirection = true; //Player has begun the direction selection process
                Player.GetComponent<NewMover>().enabled = false;

                selectedButton = 0;
                upIsOn = false;
            }

            //Once the player has chosen to move:
            else if (playerSelectingDirection == true)
            {
                if (Input.GetKeyUp(KeyCode.W))
                {
                    selectedButton = 0;
                }
                if (Input.GetKeyUp(KeyCode.S) && gameStage != 1)
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
                        mainCamera.smoothness = 10;

                        elevatorMusic.Play();
                        bgMusic.Pause();
                    }
                    else
                    {
                        Debug.Log("Already at the top floor");
                        //Play a no go sound
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
                        if (gameStage == 2) //After the sound puzzle, go all the way to the basement
                        {
                            target = new Vector3(startPosition.x, startPosition.y - (2f * moveDistance), 0);
                            elevatorTarget = new Vector3(Elevator.transform.position.x, Elevator.transform.position.y - (2f * moveDistance), 0);
                        }
                        else
                        {
                            target = new Vector3(startPosition.x, startPosition.y - moveDistance, 0);
                            elevatorTarget = new Vector3(Elevator.transform.position.x, Elevator.transform.position.y - moveDistance, 0);
                        }
                        premoveSortingOrder = Player.GetComponent<SpriteRenderer>().sortingOrder;

                        mainCamera.smoothness = 10;

                        elevatorMusic.Play();
                        bgMusic.Pause();
                    }
                    else
                    {
                        Debug.Log("Already at the bottom floor");
                        //Play a no go sound
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
                TransporterFloor++;
                mainCamera.smoothness = 2;
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

            if (gameStage == 2) t += Time.deltaTime / 20;
            else t += Time.deltaTime / 10;
            Player.transform.position = Vector3.Lerp(startPosition, target, t);
            Elevator.transform.position = Vector3.Lerp(ElevatorStartPosition, elevatorTarget, t);

            if (Player.transform.position.y == target.y)
            {
                Debug.Log("Reached Down");
                if (gameStage == 2)
                {
                    TransporterFloor = TransporterFloor - 2;
                    gameStage = 3;
                }
                else TransporterFloor--;
                mainCamera.smoothness = 2;
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
        yield return new WaitForSeconds(2.0f);
        Debug.Log("EndWait");
        elevatorMusic.Stop();
        bgMusic.UnPause();
        Player.GetComponent<NewMover>().enabled = true;
    }

    IEnumerator ShiftElevatorLayer()
    {
        foreach(GameObject elevatorPiece in elevatorItems)
        {
            if (elevatorPiece.GetComponent<SpriteRenderer>().sortingLayerName != "UI") elevatorPiece.GetComponent<SpriteRenderer>().sortingOrder = elevatorPiece.GetComponent<SpriteRenderer>().sortingOrder + (Player.GetComponent<SpriteRenderer>().sortingOrder - premoveSortingOrder);
        }
        yield return null;
    }
}
