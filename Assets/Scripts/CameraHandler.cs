// This script was made by following a tutorial made by Sevastian Graves 
// youtube video: https://www.youtube.com/watch?v=LOC5GJ5rFFw&list=PLD_vBJjpCwJtrHIW1SS5_BNRk6KZJZ7_d&index=2


using UnityEngine;

public class CameraHandler : MonoBehaviour {
    public Transform targetT;
    public Transform cameraT;
    public Transform cameraPivotT;
    private Transform myT;

    private Vector3 cameraTPosition;
    private LayerMask ignoreLayers;
    private Vector3 cameraFollowVelocity = Vector3.zero;

    public static CameraHandler singleton;
    public float lookSpeed = 0.1f;
    public float followSpeed = 0.1f;
    public float pivotSpeed = 0.03f;
    public float targetPosition;
    private float defaultPosition;
    private float lookAngle;
    private float pivotAngle;
    public float minPivot = -35;
    public float maxPivot = 35;

    public float cameraSphereRadius = 0.2f;
    public float cameraCollisionOffset = 0.2f;
    public float minCollisionOffset = 0.2f;


    public void Awake() {
        singleton = this;
        
    }

    public void Start() {
        myT = transform;
        // defaultPosition = cameraT.position.z;
        defaultPosition = (cameraT.position - targetT.position).z;
        ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
    }

    // updates camera position so that it follows the player
    public void FollowTarget(float delta) {
        Vector3 targetPosition = Vector3.SmoothDamp(myT.position, targetT.position, ref cameraFollowVelocity, delta/followSpeed); 
        myT.position = targetPosition; 

        HandleCameraCollision(delta);
    }

    // updates the camera posiiton based on mouse input
    public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput) {

        lookAngle += (mouseXInput * lookSpeed) / delta;
        pivotAngle -= (mouseYInput * pivotSpeed) / delta;

        // limit camera position based on max/min settings
        pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        myT.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;

        targetRotation = Quaternion.Euler(rotation);
        cameraPivotT.localRotation = targetRotation;
    }

    private void HandleCameraCollision(float delta) {
        targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraT.position - cameraPivotT.position;


        // SphereCast: a sphere that surrounds the camera. If it collides with other colliders, returns true
        if (Physics.SphereCast(cameraPivotT.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers)) {
            float dis = Vector3.Distance(cameraPivotT.position, hit.point);
            targetPosition = -(dis - cameraCollisionOffset);
        }

        if (Mathf.Abs(targetPosition) < minCollisionOffset) {
            targetPosition = -minCollisionOffset;
        }

        cameraTPosition.z = Mathf.Lerp(cameraT.localPosition.z, targetPosition, delta / 0.2f);
        cameraT.localPosition = cameraTPosition;
    }    
}

