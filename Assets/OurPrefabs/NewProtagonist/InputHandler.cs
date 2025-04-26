// This script was made by following a tutorial made by Sevastian Graves 
// youtube video: https://www.youtube.com/watch?v=LOC5GJ5rFFw&list=PLD_vBJjpCwJtrHIW1SS5_BNRk6KZJZ7_d&index=2

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    [Header("Player Movement Input")]
    [SerializeField] Vector2 movementInput;
    public float horizontal;
    public float vertical;
    public float moveAmount;


    [Header("Camera Movement Input")]
    [SerializeField] Vector2 cameraInput;
    public float cameraHorizontal;
    public float cameraVertical;

    [Header("Player Action Input")]
    [SerializeField] bool jumpInput = false;
    [SerializeField] bool rollInput = false;


    // class from input controls package
    InputSystem_Actions inputActions;
    CameraHandler cameraHandler;
    public PlayerMovement playerMovement;

    public void OnEnable() {

        if (inputActions == null) {
            inputActions = new InputSystem_Actions();
            inputActions.Player.Move.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.Player.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            inputActions.Player.Jump.performed += i => jumpInput = true;
            inputActions.Player.Roll.performed += i => rollInput = true;

            // Note: => is a lambda expression: (parameters) => expression_or_block_of_code
            // Note: performed is an event; += is the listener? 
        }

        inputActions.Enable();
    }
    
    public void OnDisable() {
        inputActions.Disable();
    }

    public void TickInput(float delta) {
        MoveInput();
        JumpInput();
        RollInput();
    }

    public void MoveInput() {

        // get player direcctional input
        horizontal = movementInput.x;
        vertical = movementInput.y;

        // calculate total movement 
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        
        if (moveAmount <= 0.5 && moveAmount > 0.5)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount > 0.5f && moveAmount <= 1)
        {
            moveAmount = 1;
        }

        // get camera input
        cameraHorizontal = cameraInput.x;
        cameraVertical = cameraInput.y;
    }

    private void JumpInput()
    {
        // if (jumpInput)
        // {
        //     jumpInput = false;
        //     playerMovement.PerformRoll();
        // }
    }

    private void RollInput()
    {
        if (rollInput)
        {
            rollInput = false;
            playerMovement.PerformRoll();
        }
    }
}

 
