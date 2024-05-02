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
        [SerializeField] private GameObject ElevatorBackLeft;
        [SerializeField] private GameObject ElevatorBackRight;
        [SerializeField] private GameObject ElevatorFloor;
        [SerializeField] private GameObject ElevatorTop;
        [SerializeField] private GameObject ElevatorDoor;
        [SerializeField] private GameObject ElevatorFrontRight;
        [SerializeField] private GameObject ElevatorFrontLeft;
        [SerializeField] private GameObject Player;
        [SerializeField] private GameObject Elevator;

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
        public void setGameStage(int i)
        {
            gameStage = i;
        }
        public int getGameStage()
        {
            return gameStage;
        }
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
        public AudioSource RockMusic;
        public AudioSource ErrorNoise;

        public ControlManager controlManager;

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
        if (soundLabDone && gameStage < 2)
        {
            elevatorMusic = RockMusic;
            gameStage = 2;
        }

            //Standing in the Elevator
            if (inElevator && notMoving)
        {
            //Wait for player to select
            if ((Input.GetKeyUp(KeyCode.O) || controlManager.select) && playerSelectingDirection == false)
            {
                controlManager.select = false;
                animator.SetBool("Open", false); //Close Door

                playerSelectingDirection = true; //Player has begun the direction selection process
                Player.GetComponent<NewMover>().enabled = false;

                selectedButton = 0;
                upIsOn = false;
            }

            //Once the player has chosen to move:
            else if (playerSelectingDirection == true)
            {
                if ((Input.GetKeyUp(KeyCode.W) || controlManager.moveUp) && gameStage != 3)
                {
                    controlManager.moveUp = false;
                    selectedButton = 0;
                }
                if ((Input.GetKeyUp(KeyCode.S) || controlManager.moveDown) && gameStage != 1 && gameStage != 3)
                {
                    controlManager.moveDown = false;
                    selectedButton = 1;
                }
                if ((Input.GetKeyUp(KeyCode.P) || controlManager.back)) //Exit the elevator
                {
                    controlManager.back = false;
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
            if ((Input.GetKeyUp(KeyCode.O) || controlManager.select) && playerSelectingDirection == true)
            {
                controlManager.select = false;
                if (selectedButton == 0)
                {
                    Debug.Log("UP");
                    if(TransporterFloor == -1){
                      Debug.Log("Basement Going to First!");
                      StartCoroutine(basementtofirst());
                    } else if (TransporterFloor == 0){
                      Debug.Log("First Going to Second!");
                      StartCoroutine(firsttosecond());
                    }
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
                        ErrorNoise.Play();
                    }
                }
                else if (selectedButton == 1)
                {
                    Debug.Log("DOWN");
                    if(TransporterFloor == 0){
                      Debug.Log("First Going to Basement!");
                      StartCoroutine(firsttobasement());
                    }
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
                            Debug.Log("Second Going to Basement!");
                            StartCoroutine(secondtobasement());
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
                        ErrorNoise.Play();
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
                mainCamera.smoothness = 2;
                notMoving = true;
                selectedButton = 2;
                movingUp = false;
                animator.SetBool("Open", true); //Open Door
                TransporterFloor++;
                //StartCoroutine(ShiftElevatorLayer());
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
                else {
                  TransporterFloor--;
                }
                mainCamera.smoothness = 2;
                notMoving = true;
                selectedButton = 2;
                movingDown = false;
                animator.SetBool("Open", true); //Open Door
                //StartCoroutine(ShiftElevatorLayer());
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

    IEnumerator firsttosecond()
    {
        Debug.Log("starting");
        
        // Wait for a few seconds
        ElevatorBackLeft.GetComponent<SpriteRenderer>().sortingOrder = 250;
        ElevatorBackRight.GetComponent<SpriteRenderer>().sortingOrder = 250;
        ElevatorFloor.GetComponent<SpriteRenderer>().sortingOrder = 250;
        ElevatorDoor.GetComponent<SpriteRenderer>().sortingOrder = 252;
        ElevatorTop.GetComponent<SpriteRenderer>().sortingOrder = 252;
        ElevatorFrontLeft.GetComponent<SpriteRenderer>().sortingOrder = 252;
        ElevatorFrontRight.GetComponent<SpriteRenderer>().sortingOrder = 252;
        //PLAYER IN THE MIDDLE

        yield return new WaitForSeconds(3f);
        // Set the sorting order of all objects to the second specified value
        ElevatorBackLeft.GetComponent<SpriteRenderer>().sortingOrder = 20;
        ElevatorBackRight.GetComponent<SpriteRenderer>().sortingOrder = 20;
        ElevatorFloor.GetComponent<SpriteRenderer>().sortingOrder = 20;
        ElevatorDoor.GetComponent<SpriteRenderer>().sortingOrder = 22;
        ElevatorTop.GetComponent<SpriteRenderer>().sortingOrder = 22;
        ElevatorFrontLeft.GetComponent<SpriteRenderer>().sortingOrder = 22;
        ElevatorFrontRight.GetComponent<SpriteRenderer>().sortingOrder = 22;

        Debug.Log("ending");
    }

    IEnumerator secondtobasement()
    {
        // Set the sorting order of all objects to the first specified value
        foreach (GameObject obj in elevatorItems)
        {
            if (obj.GetComponent<SpriteRenderer>().sortingLayerName != "UI") obj.GetComponent<SpriteRenderer>().sortingOrder = 20;
        }

        // Wait for a few seconds
        yield return new WaitForSeconds(0.5f);

        // Set the sorting order of all objects to the second specified value
        foreach (GameObject obj in elevatorItems)
        {
            if (obj.GetComponent<SpriteRenderer>().sortingLayerName != "UI") obj.GetComponent<SpriteRenderer>().sortingOrder = 250;
        }

        yield return new WaitForSeconds(0.5f);

        // Set the sorting order of all objects to the second specified value
        foreach (GameObject obj in elevatorItems)
        {
            if (obj.GetComponent<SpriteRenderer>().sortingLayerName != "UI") obj.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
    }

    IEnumerator basementtofirst()
    {
        // Set the sorting order of all objects to the first specified value
        foreach (GameObject obj in elevatorItems)
        {
            if (obj.GetComponent<SpriteRenderer>().sortingLayerName != "UI") obj.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }

        // Wait for a few seconds
        yield return new WaitForSeconds(0.5f);

        // Set the sorting order of all objects to the second specified value
        foreach (GameObject obj in elevatorItems)
        {
            if (obj.GetComponent<SpriteRenderer>().sortingLayerName != "UI") obj.GetComponent<SpriteRenderer>().sortingOrder = 250;
        }
    }

    IEnumerator firsttobasement()
    {
        // Set the sorting order of all objects to the first specified value
        foreach (GameObject obj in elevatorItems)
        {
            if (obj.GetComponent<SpriteRenderer>().sortingLayerName != "UI") obj.GetComponent<SpriteRenderer>().sortingOrder = 250;
        }

        // Wait for a few seconds
        yield return new WaitForSeconds(0.5f);

        // Set the sorting order of all objects to the second specified value
        foreach (GameObject obj in elevatorItems)
        {
            if (obj.GetComponent<SpriteRenderer>().sortingLayerName != "UI") obj.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
    }
}
