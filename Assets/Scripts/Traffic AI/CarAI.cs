using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Controls;

public class CarAI : MonoBehaviour
{

    [SerializeField]
    private List<Vector3> path = null;
    // private List<Transform> path = null;
    [SerializeField]
    private float arriveDistance = 0.3f, lastPointArriveDistance = 0.1f;
    [SerializeField]
    private float turningAngleOffset = 5;
    [SerializeField]
    private Vector3 currentTargetPosition;
    [SerializeField]
    private GameObject raycastStartingPoint = null;  
    // [SerializeField]
    private float collisionRaycastLength = 5.0f;   
    public AiDirector director;

    private int index = 0;

    private float stopTimer = 0f;

    private bool stop;

    [SerializeField]
    private bool collisionStop = false;

    [SerializeField]
    public bool Stop
    {
        get { return stop || collisionStop;}
        set { stop = value;}
    }

    [field: SerializeField]
    public UnityEvent<Vector2> OnDrive { get; set; }

    public Vector3 TargetPosition { get {return currentTargetPosition;}}

    private void Start() 
    {
        // aiDirector = GetComponent<AiDirector>();
        if(path == null || path.Count == 0){
            Stop = true;
            stopTimer = 0f;
        }
        else
        {
            currentTargetPosition = path[index];
        }
    }

    public void SetPath(List<Vector3> path) 
    {
        if(path.Count == 0)
        {
            // change this so that the new path goes back to its starting point
            // set path to reverse path?
            Destroy(gameObject);
            director.updateNumCars();
            return;
        }
        this.path = path;
        index = 0;
        currentTargetPosition = this.path[index];

        // set car to face correct direction
        Vector3 relativepoint = transform.InverseTransformPoint(this.path[index + 1]);
        float angle = Mathf.Atan2(relativepoint.x, relativepoint.z)*Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);

        // make car move in this direction
        Stop = false; 
    }

    internal bool IsThisLastPathIndex() 
    {
        return index >= path.Count - 1;
    }

    public void Update()
    {
        CheckIfArrived();
        Drive();
        CheckForCollisions();
    }

    private void CheckForCollisions() 
    {
        if(Physics.Raycast(raycastStartingPoint.transform.position, transform.forward, collisionRaycastLength, 1 << gameObject.layer))
        {
            collisionStop = true;
        } else {
            collisionStop = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (raycastStartingPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(raycastStartingPoint.transform.position, transform.forward * collisionRaycastLength);
        }

        for (int i=1; i<path.Count; i++) {
            Debug.DrawLine(path[i-1]+Vector3.up*2, path[i]+Vector3.up*2, Color.red);
        }
    }

    private void CheckIfArrived() 
    {
        if(Stop == false)
        {
            // check distance between position (in path) and car
            var distanceToCheck = arriveDistance; // default
            if(index == path.Count-1) 
            {
                distanceToCheck = lastPointArriveDistance;
            }

            // if car has arrived at current target, set the next target point
            if(Vector3.Distance(currentTargetPosition, transform.position) < distanceToCheck) 
            {
                SetNextTargetIndex();
            }
        }
    }

    private void SetNextTargetIndex()
    {
        index++;

        if(index >= path.Count) // if car reached destination
        {
            Stop = true;
            Destroy(gameObject); // change this?
            director.updateNumCars();
        }
        else 
        {
            currentTargetPosition = path[index];
        }
    }

    private void Drive() 
    {
        if(Stop)
        {
            OnDrive?.Invoke(Vector2.zero); // stop the car
            stopTimer += Time.deltaTime;

            if (stopTimer >= 20f) {
                Destroy(gameObject);
                director.updateNumCars();
            }
        }
        else
        {
            stopTimer = 0f;

            Vector3 relativepoint = transform.InverseTransformPoint(currentTargetPosition);
            float angle = Mathf.Atan2(relativepoint.x, relativepoint.z)*Mathf.Rad2Deg;
            var rotateCar = 0;
            if(angle > turningAngleOffset)
            {
                rotateCar = 5; // rotate right
            }
            else if(angle < -turningAngleOffset)
            {
                rotateCar = -5; // rotate left
            }
            OnDrive?.Invoke(new Vector2(rotateCar,1));
            
        }
    }
}
