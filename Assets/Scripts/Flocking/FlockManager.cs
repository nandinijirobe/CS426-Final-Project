// Source code: https://learn.unity.com/tutorial/flocking#

using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public static FlockManager FM; // Allows you to access variables of this script in another script
    public GameObject npcPrefab; // Store the NPC prefab here
    public int numNPC = 20; // Total number of NPCs that will be part of the flock
    public GameObject[] allNPCs; // All NPC gameobjects will be part of this list
    public Vector3 runLimits = new Vector3(5, 0, 5); // Boundry size the NPCs can spawn or move in from the FlockManager 
    public GameObject goalGameObject;
    public Vector3 goalPos = Vector3.zero; // The target location all NPCs will move towards

    [Header("NPC settings")]
    [Range(0.0f, 5.0f)]
    public float minSpeed; // min speed of an NPC
    [Range(0.0f, 5.0f)]
    public float maxSpeed; // max speed of an NPC
    [Range(1.0f, 10.0f)]
    public float neighbourDistance; // determines how many other NPCs current NPC can consider its neighbour
    [Range(1.0f, 5.0f)]
    public float rotationSpeed; // how fast the NPC can rotate

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allNPCs = new GameObject[numNPC]; // Initalizes list to store all NPC game objects
        for (int i = 0; i < numNPC; i++)
        {
            // This sets the position as to where the NPC will be instanitaed 
            // The NPC will be instantiated somewhere within the bounds of runLimits
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-runLimits.x, runLimits.x),
                                                                1.2f,
                                                                Random.Range(-runLimits.z, runLimits.z));
            allNPCs[i] = Instantiate(npcPrefab, pos, Quaternion.identity); // NOTE: Quaternion.identity means that there the NPC is not being instantiated with any specific rotation
        }

        FM = this; // Refers to the GameObject this script is attached to
        //goalPos = this.transform.position; // Set to the current location of the FM game object
        goalPos = goalGameObject.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        goalPos = goalGameObject.transform.position;
        // NOTE: Removed to make goalPos fixed. 
        //if (Random.Range(0, 100) < 10)
        //{ // This randomly changes the postion of the target position somewhere within the bounds of FM
        //    goalPos = this.transform.position + new Vector3(Random.Range(-runLimits.x, runLimits.x),
        //                                                        1.2f,
        //                                                        Random.Range(-runLimits.z, runLimits.z));
        //}
    }

    // This ensures FM is set before any Start() methods are called because Unity runs Awake() before Start()
    // This ensures FM is ready for Flock.cs start function
    void Awake()
    {
        FM = this;
    }

}
