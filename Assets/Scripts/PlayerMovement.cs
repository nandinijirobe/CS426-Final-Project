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
    Vector3 moveDirection;

    [HideInInspector]
    public Transform t;
    public Rigidbody rb;
    public GameObject normalCamera; // for later development


// SerializedField allows you to see it in Inspector
    [Header("Movement Stats")] // what does this do?
    [SerializeField] public float speed = 5;
    [SerializeField] public float rotationSpeed = 10;
    public float force = 700f;


    // Start is called before the first frame update
    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        rb = GetComponent<Rigidbody>();
        t = GetComponent<Transform>();

        inputHandler = GetComponent<InputHandler>();
        cameraObject = Camera.main.transform;

    }

    #region Movement
    Vector3 normalVector;
    Vector3 targetPosition;

    private void HandleRotaion(float delta) {
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
        Quaternion targetRotation = Quaternion.Slerp(t.rotation, tr, rs * delta);

        t.rotation = targetRotation;
    }

    public void HandleMovement(float delta) {

        HandleRotaion(delta);
        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        float movementSpeed = speed; 
        moveDirection *= speed;

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rb.linearVelocity = projectedVelocity;

    }
    #endregion
}

