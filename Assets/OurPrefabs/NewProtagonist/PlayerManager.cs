using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    InputHandler inputHandler; 
    CameraHandler cameraHandler;
    PlayerMovement playerMovement;
    public AnimatorHandler animatorHandler;

    public CharacterController characterController;

    [Header("Flags")]
    public bool isJumping = false;
    public bool isRolling = false;
    public bool isPerformingAction = false;

    public bool canMove = true;
    public bool canRotate = true;

    public void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        cameraHandler = CameraHandler.singleton;
        inputHandler = GetComponent<InputHandler>();
        playerMovement = GetComponent<PlayerMovement>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        animatorHandler.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        // // Time.deltaTime represents the time that passed since the last frame
        float delta = Time.deltaTime;

        inputHandler.TickInput(delta);
        HandlePlayerMovementInput();
       
    }

    private void HandlePlayerMovementInput()
    {
        float delta = Time.deltaTime;
        playerMovement.HandleAllMovement();
        animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0);

        // if (animatorHandler.canRotate)
        // {
        //     playerMovement.HandleRotation();
        // }

        if (cameraHandler != null) {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, inputHandler.cameraHorizontal, inputHandler.cameraVertical);
        }
    }

    // private void FixedUpdate() {
    //     float delta = Time.fixedDeltaTime;

    //     // if (cameraHandler != null) {
    //     //     cameraHandler.FollowTarget(delta);
    //     //     cameraHandler.HandleCameraRotation(delta, inputHandler.cameraHorizontal, inputHandler.cameraVertical);
    //     // }
    // }
}
