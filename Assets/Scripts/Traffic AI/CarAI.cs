using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
     
    public AiDirector director;

    private int index = 0;

    private bool stop;

    public bool Stop
    {
        get { return stop;}
        set { stop = value;}
    }

    [field: SerializeField]
    public UnityEvent<Vector2> OnDrive { get; set; }

    private void Start() 
    {
        // aiDirector = GetComponent<AiDirector>();
        if(path == null || path.Count == 0){
            Stop = true;
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

    public void Update()
    {
        CheckIfArrived();
        Drive();
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
        }
        else
        {
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
