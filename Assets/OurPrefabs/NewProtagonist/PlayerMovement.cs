// This script was modified by following a tutorial made by Sevastian Graves 
// youtube video: https://www.youtube.com/watch?v=LOC5GJ5rFFw&list=PLD_vBJjpCwJtrHIW1SS5_BNRk6KZJZ7_d&index=2


// lets make him move
// using __ imports namespace
// Namespaces are collection of classes, data types
using System.Collections;
using System.Collections.Generic;
// using System.Numerics;
using UnityEngine;

// MonoBehavior is the base class from which every Unity Script Derives
public class PlayerMovement : MonoBehaviour
{

    PlayerManager playerManager;
    Transform cameraObject;
    InputHandler inputHandler;
    public Vector3 moveDirection;

    [HideInInspector]
    public Transform t;
    public Rigidbody rb;
    public GameObject normalCamera; // for later development

    PlayerSoundFXHandler soundFXHandler;

// SerializedField allows you to see it in Inspector
    [Header("Movement Stats")] // what does this do?
    [SerializeField] public float walkingSpeed = 5;
    [SerializeField] public float sprintingSpeed = 20;
    [SerializeField] public float rotationSpeed = 10;
    public float force = 700f;

    [Header("Roll")]
    Vector3 rollDirection;


    // Start is called before the first frame update
    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        rb = GetComponent<Rigidbody>();
        t = GetComponent<Transform>();

        inputHandler = GetComponent<InputHandler>();

        soundFXHandler = GetComponent<PlayerSoundFXHandler>();
        cameraObject = Camera.main.transform;
    }

    #region Movement
    Vector3 normalVector;
    Vector3 targetPosition;

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
        HandleSoundFX();
    }

    public void HandleRotation() {
        if (!playerManager.canRotate) {return;}

        Vector3 targetDir = Vector3.zero;
        float moveOverride = inputHandler.moveAmount;

        targetDir = cameraObject.forward * inputHandler.vertical;
        targetDir += cameraObject.right * inputHandler.horizontal;

        targetDir.Normalize();
        targetDir.y = 0;

        if(targetDir == Vector3.zero) {
            targetDir = t.forward; 
        }

        float rs = rotationSpeed;
        // // Quaternion returns a rotation that rotates x degrees around the x axis and so on
        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(t.rotation, tr, rs * Time.deltaTime);

        t.rotation = targetRotation;
    }

    public void HandleMovement() {
        if (!playerManager.canMove) {return;}

        // move direction based on camera perspective and inputs
        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;


        if (playerManager.isSprinting)
        {
            playerManager.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
        }
        else 
        {
            playerManager.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
        }     
    }

    public void PerformRoll()
    {
        if (playerManager.isPerformingAction) {return;}
        // if (inputHandler.moveAmount > 0) {}

        rollDirection = cameraObject.forward * inputHandler.vertical;
        rollDirection += cameraObject.right * inputHandler.horizontal;

        rollDirection.y = 0;
        rollDirection.Normalize();
        Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
        // playerManager.transform.rotation = playerRotation;
        t.rotation = playerRotation;

        // perform a roll animation
        playerManager.animatorHandler.PlayerTargetActionAnimation("Roll", true);

        // add custom movement to animation
        // playerManager.characterController.Move(rollDirection * 10 * Time.deltaTime);
    }
    
    public void HandleSprinting()
    {
        // don't sprint while rolling or jumping
        if (playerManager.isPerformingAction) {
            playerManager.isSprinting = false;
        }
        
        if (inputHandler.moveAmount > 0)
        {
            playerManager.isSprinting = true;
        }
        else // if player is stationary, don't sprint
        {
            playerManager.isSprinting = false;
        }

    }
    

    private void HandleSoundFX()
    {
        if (playerManager.isPerformingAction) {
            soundFXHandler.StopLoop();
            return;
        }

        if (moveDirection.magnitude > 0.1f) // moving
        {
            if (playerManager.isSprinting)
            {
                soundFXHandler.PlayLoop(soundFXHandler.sprintSFX);
                Debug.Log("Playing sprinting sound");
            }
            else
            {
                soundFXHandler.PlayLoop(soundFXHandler.walkSFX);
                Debug.Log("Playing walking sound");
            }
        }
        else // standing still
        {
            soundFXHandler.StopLoop();
            Debug.Log("Stopping sound");
        }
    }

    #endregion
}

