// This script was made by following a tutorial made by Sevastian Graves 
// youtube video: https://www.youtube.com/watch?v=LOC5GJ5rFFw&list=PLD_vBJjpCwJtrHIW1SS5_BNRk6KZJZ7_d&index=2


using UnityEngine;
public class CameraHandler : MonoBehaviour {

    [Header("Camera Settings")]
    [SerializeField] float leftRightRotationSpeed = 220;
    [SerializeField] float upDownRotationSpeed = 220;
    [SerializeField] float minPivot = -10; // lowest point you can look down
    [SerializeField] float maxPivot = 20; // highest point you can look up
    
    public float cameraSphereRadius = 0.2f;
    public float cameraCollisionOffset = 0.2f;
    public float minCollisionOffset = 0.2f;
    
    private float cameraSmoothSpeed = 1; // large number = longer for camera to reach its position

    [Header("Camera Values")]
    [SerializeField] LayerMask collisionLayers;
    [SerializeField] float leftRightLookAngle;
    [SerializeField] float upDownLookAngle; 
    public float targetCameraPosition;
    private float defaultCameraPosition;



    public Transform targetT;
    public Transform cameraT;
    public Transform cameraPivotT;
    private Transform myT;

    private Vector3 cameraTPosition;
    
    private Vector3 cameraFollowVelocity = Vector3.zero;
    

    public static CameraHandler singleton;
    // public float lookSpeed = 0.1f;
    // public float followSpeed = 0.1f;
    // public float pivotSpeed = 0.03f;
    
    
    


    public void Awake() {
        singleton = this;
        
    }

    public void Start() {
        myT = transform;
        defaultCameraPosition = cameraT.localPosition.z; // forward/ backwards/ distance from player
        // ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
    }

    // updates camera position so that it follows the player
    public void FollowTarget(float delta) {
        Vector3 targetPosition = Vector3.SmoothDamp(myT.position, targetT.position, ref cameraFollowVelocity, cameraSmoothSpeed * Time.deltaTime); 
        myT.position = targetPosition; 

        HandleCameraCollision();
    }

    // updates the camera posiiton based on mouse input
    public void HandleCameraRotation(float delta, float horizontalInput, float verticalInput) {

        leftRightLookAngle += (horizontalInput * leftRightRotationSpeed) * Time.deltaTime;
        upDownLookAngle -= (verticalInput * upDownRotationSpeed) * Time.deltaTime;

        // limit camera position based on max/min settings
        upDownLookAngle = Mathf.Clamp(upDownLookAngle, minPivot, maxPivot);


        Vector3 rotation = Vector3.zero;
        Quaternion targetRotation;

        // rotate this gameobject on left and right
        rotation.y = leftRightLookAngle;
        targetRotation = Quaternion.Euler(rotation);
        myT.rotation = targetRotation;

        // rotate pivot object up and down
        rotation = Vector3.zero;
        rotation.x = upDownLookAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivotT.localRotation = targetRotation;
    }

    private void HandleCameraCollision() {
        targetCameraPosition = defaultCameraPosition;
        RaycastHit hit;
        Vector3 direction = cameraT.position - cameraPivotT.position;
        direction.Normalize();


        // SphereCast: a sphere that surrounds the camera. If it collides with other colliders, returns true
        if (Physics.SphereCast(cameraPivotT.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetCameraPosition), collisionLayers)) {
            float disanceFromHitObject = Vector3.Distance(cameraPivotT.position, hit.point);
            targetCameraPosition = -(disanceFromHitObject - cameraSphereRadius);
        }

        if (Mathf.Abs(targetCameraPosition) < cameraSphereRadius) {
            targetCameraPosition = -cameraSphereRadius;
        }

        cameraTPosition.z = Mathf.Lerp(cameraT.localPosition.z, targetCameraPosition, 0.2f);
        cameraT.localPosition = cameraTPosition;
    }    
}
