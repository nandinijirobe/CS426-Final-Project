// This script was made by following a tutorial made by Sevastian Graves 
// youtube video: https://www.youtube.com/watch?v=LOC5GJ5rFFw&list=PLD_vBJjpCwJtrHIW1SS5_BNRk6KZJZ7_d&index=2

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;


    // class from input controls package
    InputSystem_Actions inputActions;
    // CameraHandler cameraHandler;

    Vector2 movementInput;
    Vector2 cameraInput;

    public void OnEnable() {

        if (inputActions == null) {
            inputActions = new InputSystem_Actions();
            inputActions.Player.Move.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.Player.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            // Note: => is a lambda expression: (parameters) => expression_or_block_of_code
            // Note: performed is an event; += is the listener? 
        }

        inputActions.Enable();
    }
    
    public void OnDisable() {
        inputActions.Disable();
    }

    public void TickInput(float delta) {
        MoveInput(delta);
    }

    public void MoveInput(float delta) {

        // get player direcctional input
        horizontal = movementInput.x;
        vertical = movementInput.y;

        // calculate total movement 
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        
        // get camera input?
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;
    }
}

 
