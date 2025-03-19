using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    InputHandler inputHandler; 
    CameraHandler cameraHandler;
    PlayerMovement playerMovement;

    void Start()
    {
        cameraHandler = CameraHandler.singleton;
        inputHandler = GetComponent<InputHandler>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // // Time.deltaTime represents the time that passed since the last frame
        float delta = Time.deltaTime;

        inputHandler.TickInput(delta);
        playerMovement.HandleMovement(delta);
    }

    private void FixedUpdate() {
        float delta = Time.fixedDeltaTime;

        if (cameraHandler != null) {
            cameraHandler.FollowTarget(delta);
            cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
        }
    }
}
