// Source code: https://learn.unity.com/tutorial/flocking#

using UnityEngine;

public class FlockManager : MonoBehaviour
{

    public Vector3 safeSpot;

    public GameObject npcPrefab; // our NPC prefab
    public int numNPC = 20;      // the number of total NPCs
    public GameObject[] allNPCs; // list to store all NPC game objects

    public Vector3 runLimits; // space the NPCs can actually run within 


    public GameObject goalGameObject;      // the target game object all NPCs will run towards (player)
    public Vector3 goalPos = Vector3.zero; // the target location all NPCs will move towards
    public bool disguiseOn;        // this checks if player is supposed to be hidden from papparazi
    public bool inBounds = false;           // this checks if player has entered the NPC bounds
    private BoxCollider flockCollider;      // this actually determines the bounds of the NPCs

    public Bounds flockBounds;       //  this is to store the bounds of the Flock's Box Collider
    private Vector3 flock_minCorner;  //  the corner with the smallest x, y, z values
    private Vector3 flock_maxCorner;  //  the corner with the largest x, y, z values

    [Header("NPC settings")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;          // min speed of an NPC
    [Range(0.0f, 5.0f)]
    public float maxSpeed;          // max speed of an NPC
    [Range(1.0f, 10.0f)]
    public float neighbourDistance; // determines how many other NPCs current NPC can consider its neighbour
    [Range(1.0f, 5.0f)]
    public float rotationSpeed;     // how fast the NPC can rotate


    public GameTimeUiManager gameTimeUiManager;

    public WomanDressUI womanDressUI;

    [Header("Penalty Flash UI")]
    public CanvasGroup penaltyCanvasGroup;  // CanvasGroup on the flash image
    public float penaltyDisplayTime = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        goalPos = goalGameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        disguiseOn = womanDressUI.disguiseOn;
        if (flockCollider.bounds.Contains(goalGameObject.transform.position))
        {
            goalPos = goalGameObject.transform.position; // chase the player
            inBounds = true;
            //Debug.Log("girl is inside bounds");
        }
        else
        {
            goalPos = flockCollider.bounds.center; // go run at center position
            inBounds = false;
            //Debug.Log("girl is out of bounds");
        }
    }

    // This ensures FM is set before any Start() methods are called because Unity runs Awake() before Start()
    // This ensures FM is ready for Flock.cs start function
    void Awake()
    {
        // Added instantiation code here instead of Start() to avoid race conditions

        flockCollider = GetComponent<BoxCollider>(); // get the position of the box collider of this object
        flockBounds = flockCollider.bounds;          // this gets the bounding box of the collider
        flock_minCorner = flockBounds.min;
        flock_maxCorner = flockBounds.max;

        allNPCs = new GameObject[numNPC];             // initalizes list to store all NPC game objects
        runLimits = flockCollider.bounds.size * 0.5f; // this means that the run limits is about half the size of the box collider for this game object

        for (int i = 0; i < numNPC; i++)
        {
            // This sets the position as to where the NPC will be instanitaed 
            // The NPC will be instantiated somewhere within the bounds of the box collider
            Vector3 pos = new Vector3(Random.Range(flock_minCorner.x, flock_maxCorner.x),
                                      1.2f,
                                      Random.Range(flock_minCorner.z, flock_maxCorner.z));
            allNPCs[i] = Instantiate(npcPrefab, pos, Quaternion.Euler(0, Random.Range(0f, 360f), 0)); // NOTE: Quaternion.identity means that there the NPC is not being instantiated with any specific rotation
            DeductTime deductTime = allNPCs[i].GetComponent<DeductTime>();
            deductTime.gameTimeUiManager = gameTimeUiManager;
            deductTime.penaltyCanvasGroup = penaltyCanvasGroup;
            allNPCs[i].GetComponent<Flock>().FM = this; // Refers to the GameObject this script is attached to
        }
    }

}