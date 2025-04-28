// Source code: https://learn.unity.com/tutorial/flocking#

using UnityEngine;


public class Flock : MonoBehaviour
{
    float speed;            // speed of NPC
    bool turning = false;   // determines whether the NPC should turn around to stay within bounds
    public FlockManager FM;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (FM != null && speed == 0)
        {
            speed = Random.Range(FM.minSpeed, FM.maxSpeed); // set the speed of this NPC
        }

        if (FM != null) // This section turn or moves the NPC
        {
            Bounds b = FM.flockBounds;

            if (!b.Contains(transform.position))
            { // checks if NPC is within the bounding box around FM
                turning = true;
            }
            else
            {
                turning = false;
            }

            if (turning) // This changes the NPC to the opposite direction and slowly rotates it 
            {
                Vector3 direction = FM.goalPos - transform.position;
                direction.y = 0; // prevents NPCs from moving up or down
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(direction),
                                                      FM.rotationSpeed * Time.deltaTime);
            }
            else
            {
                if (FM.inBounds)
                {
                    // chase the player directly
                    Vector3 playerDirection = (FM.goalGameObject.transform.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation,
                                                          Quaternion.LookRotation(playerDirection),
                                                          FM.rotationSpeed * Time.deltaTime);

                }
                else {
                    if (Random.Range(0, 100) < 10) // randomly change the speed
                    {
                        speed = Random.Range(FM.minSpeed, FM.maxSpeed);
                    }

                    if (Random.Range(0, 100) < 10) // randomly apply the rules too
                    {
                        ApplyRules(); // this will turn NPC towards the direction it needs to be moving in
                    }

                }

                
            }


            if (Vector3.Distance(transform.position, FM.goalPos) < 1f)
            {
                GetComponent<Animator>().SetBool("isTalking", true); // start talking animation
            }
            else
            {
                GetComponent<Animator>().SetBool("isTalking", false); // stop talking animation

            }

            this.transform.Translate(0, 0, speed * Time.deltaTime);
            Vector3 pos = transform.position;
            pos.y = 4.25f;
            transform.position = pos;
        }

    }


    /*
     
    This actually applies the flocking behavior to the NPC. 3 Rules:
    - Cohesion: Moves together with other nearby NPCs
    - Alignment: Moves in same direction as other NPCs
    - Avoidance: Avoid bumping into other NPCs
     
     */
    void ApplyRules()
    {
        GameObject[] gos;               // gos means game objects. This refers to all the NPCs in the Flock Manager
        gos = FM.allNPCs;
        Vector3 vcentre = Vector3.zero; // center of nearby group
        Vector3 vavoid = Vector3.zero;  // avoidance vector
        float gSpeed = 0.0f;            // group average speed
        float nDistance;                // neighbour distance
        int groupSize = 0;              // all NPCs within in that distance

        foreach (GameObject go in gos)
        { // Iterate through all NPCs in the group 
            if (go != this.gameObject) // Ignore self
            {
                nDistance = Vector3.Distance(go.transform.position, this.transform.position); // Get distance from this NPC to that other NPCs
                if (nDistance <= FM.neighbourDistance)
                { // Check if it is within a neighbourly distance
                    vcentre += go.transform.position;
                    groupSize++;

                    if (nDistance < 1.5f)
                    {
                        vavoid = vavoid + (this.transform.position - go.transform.position); // Move away from the nearby NPC
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed += anotherFlock.speed;
                }
            }
        }

        if (groupSize > 0)
        {
            vcentre = vcentre / groupSize + (FM.goalPos - this.transform.position); // get the centre of the group

            speed = gSpeed / groupSize; // get the speed of the group
            Vector3 direction = (vcentre + vavoid) - transform.position;

            if (speed > FM.maxSpeed)
            {
                speed = FM.maxSpeed;
            }

            if (direction != Vector3.zero)
            { // rotate and move towards new position
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(direction),
                                                      FM.rotationSpeed * Time.deltaTime);

            }
        }
    }
}