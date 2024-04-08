using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlManager : MonoBehaviour
{
    PlayerControls controls;
    public bool moveUp = false;
    public bool moveDown = false;
    public bool moveLeft = false;
    public bool moveRight = false;
    public bool select = false;
    public bool back = false;
    
    private void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.MoveUp.performed += ctx => moveUp = true;
        controls.Gameplay.MoveUp.canceled += ctx => moveUp = false;
        controls.Gameplay.MoveDown.performed += ctx => moveDown = true;
        controls.Gameplay.MoveDown.canceled += ctx => moveDown = false;
        controls.Gameplay.MoveLeft.performed += ctx => moveLeft = true;
        controls.Gameplay.MoveLeft.canceled += ctx => moveLeft = false;
        controls.Gameplay.MoveRight.performed += ctx => moveRight = true;
        controls.Gameplay.MoveRight.canceled += ctx => moveRight = false;

        controls.Gameplay.Select.performed += ctx => select = true;
        controls.Gameplay.Select.canceled += ctx => select = false;
        controls.Gameplay.Back.performed += ctx => back = true;
        controls.Gameplay.Back.canceled += ctx => back = false;
    }
    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }
}
