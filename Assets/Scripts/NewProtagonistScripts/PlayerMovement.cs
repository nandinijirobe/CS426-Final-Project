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
    
    [HideInInspector]
    public Transform t;
    public Rigidbody rb;
    public GameObject normalCamera; // for later development

    PlayerSoundFXHandler soundFXHandler;

// SerializedField allows you to see it in Inspector
    [Header("Movement Settings")] // what does this do?
    [SerializeField] public Vector3 moveDirection;
    [SerializeField] public float walkingSpeed = 5;
    [SerializeField] public float sprintingSpeed = 20;
    [SerializeField] public float rotationSpeed = 10;
    [SerializeField] public float pushDecayRate = 1.0f;
    [SerializeField] public float pushForce = 10;
    Vector3 externalPushVelocity;
    Vector3 pushDirection;
    

    [Header("Jump")]
    [SerializeField] float jumpHeight = 4;
    [SerializeField] float jumpForwardSpeed = 4;
    Vector3 jumpDirection;


    [Header("Roll")]
    Vector3 rollDirection;

    [Header("Ground check and jumping")]
    [SerializeField] float gravityForce = -40f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckSpehereRadius = 0.3f;
    [SerializeField] Vector3 yVelocity;
    [SerializeField] public float groundedVelocity = -20;
    [SerializeField] public float fallStartVelocity = -5;
    bool fallVeloctyHasBeenSet = false;
    float inAirTimer = 0; // not needed, but could be used to add falling animation

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
    // Vector3 normalVector;
    // Vector3 targetPosition;

    public void HandleAllMovement()
    {
        ApplyNPCPushVelocity();
        ApplyPushVelocity();
        HandleMovement();
        HandleRotation();
        HandleSoundFX();
        HandleYVelocity();
        HandleJumpingMovement();
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

        // rollDirection = cameraObject.forward * inputHandler.vertical;
        // rollDirection += cameraObject.right * inputHandler.horizontal;

        // rollDirection = t.forward;

        // rollDirection.y = 0;
        // rollDirection.Normalize();
        // Quaternion playerRotation = Quaternion.LookRotation(rollDirection);

        moveDirection = t.forward;
        moveDirection.y = 0;
        moveDirection.Normalize();
        Quaternion playerRotation = Quaternion.LookRotation(moveDirection);

        t.rotation = playerRotation;

        // perform a roll animation
        playerManager.animatorHandler.PlayerTargetActionAnimation("Roll", true);
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
    

    // handles gravity and falling speed
    private void HandleYVelocity()
    {
        GroundCheck();

        if (playerManager.isGrounded)
        {
            // if we're not trying to jump
            if (yVelocity.y < 0)
            {
                inAirTimer = 0;
                fallVeloctyHasBeenSet = false;
                yVelocity.y = groundedVelocity;
                // Debug.Log("ground and not trying to jump");
            }
            else 
            {
                // Debug.Log("grounded and trying to jump. Velocy: " + yVelocity.y);
            }
        }
        else 
        {
            // makes starting fall velocity a little smoother
            if (!playerManager.isJumping && !fallVeloctyHasBeenSet)
            {
                fallVeloctyHasBeenSet = true;
                yVelocity.y = fallStartVelocity;
            }

            inAirTimer += Time.deltaTime;
            yVelocity.y += gravityForce * Time.deltaTime;
        }

        
        playerManager.characterController.Move(yVelocity * Time.deltaTime);

        // yVelocity.y = groundedVelocity;
        // characterController.Move(yVelocity * Time.deltaTime);
    }

    private void GroundCheck()
    {
        playerManager.isGrounded = Physics.CheckSphere(playerManager.transform.position, groundCheckSpehereRadius, groundLayer, QueryTriggerInteraction.Ignore);
    }

    private void HandleJumpingMovement()
    {
        if (playerManager.isJumping)
        {
            playerManager.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
        }
    }

    public void PerformJump()
    {
        if (playerManager.isPerformingAction) {return;}

        if (playerManager.isJumping) {return;}

        if (!playerManager.isGrounded) {return;}

        playerManager.animatorHandler.PlayerTargetActionAnimation("Jump", true, true, true, true);
        playerManager.isJumping = true;

        jumpDirection = cameraObject.forward * inputHandler.vertical;
        jumpDirection += cameraObject.right * inputHandler.horizontal;
        jumpDirection.Normalize();
        moveDirection.y = 0;

        if (jumpDirection != Vector3.zero)
        {
            // sprint => jump direction at full distance
            if (playerManager.isSprinting)
            {
                jumpDirection *= 1;
            }
            else // walking => jump distance at quater distance
            {
                jumpDirection *= 0.25f;
            }
        }
    }

    public void ApplyJumpingVelocity()
    {
        yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        // Debug.Log("Applying jump velocity");
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
                // Debug.Log("Playing sprinting sound");
            }
            else
            {
                soundFXHandler.PlayLoop(soundFXHandler.walkSFX);
                // Debug.Log("Playing walking sound");
            }
        }
        else // standing still
        {
            soundFXHandler.StopLoop();
            // Debug.Log("Stopping sound");
        }
    }

    public void ExternalPush(Vector3 push)
    {
        // Debug.Log("Apply push velocity: " + push);
        externalPushVelocity = push;
        externalPushVelocity.y = 0;
        moveDirection = Vector3.zero;
        playerManager.canMove = false;
    }

    public void ApplyPushVelocity()
    {
        if (externalPushVelocity.magnitude > 0.1f)
        {
            // Vector3 pushDirection = externalPushVelocity.normalized;
            // float pushSpeed = externalPushVelocity.magnitude;

            // Debug.Log("pushing the player");
            playerManager.characterController.Move(externalPushVelocity * Time.deltaTime);
            externalPushVelocity = Vector3.Lerp(externalPushVelocity, Vector3.zero, pushDecayRate * Time.deltaTime);

            // pushSpeed = Mathf.Lerp(pushSpeed, 0f, pushDecayRate * Time.deltaTime);
            // externalPushVelocity = pushDirection * pushSpeed;


            // play falling and rolling animation only if already rolling
            if (playerManager.animatorHandler.anim.GetBool("GetUp"))
            {
                playerManager.animatorHandler.PlayerTargetActionAnimation("Stumble", true, false);
                playerManager.animatorHandler.anim.SetBool("GetUp", false);
            }
        }
        else 
        {
            // play get up animation
            playerManager.animatorHandler.anim.SetBool("GetUp", true);
        }
    }

    // for when fans push the player
    public void ApplyPush(Vector3 sourcePosition)
    {
        pushDirection = (transform.position - sourcePosition).normalized;
        pushDirection.y = 0; // Don't push up or down
    }

    private void ApplyNPCPushVelocity()
    {
        if (pushDirection.magnitude > 0.1f)
        {
            Debug.Log("NPC pushing the player");
            playerManager.characterController.Move(pushDirection * pushForce * Time.deltaTime);
            pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushDecayRate * Time.deltaTime);
        }
        
    }
    #endregion
}

