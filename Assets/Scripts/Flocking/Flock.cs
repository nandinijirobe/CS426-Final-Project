// Source code: https://learn.unity.com/tutorial/flocking#

using UnityEngine;

public class Flock : MonoBehaviour
{
    float speed; // speed of NPC
    bool turning = false; // determines whether the NPC should turn around to stay within bounds

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed); // gives the NPC a random speed
    }

    // Update is called once per frame
    void Update()
    {
        Bounds b = new Bounds(FlockManager.FM.transform.position, FlockManager.FM.runLimits * 2);

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
            //Vector3 direction = FlockManager.FM.transform.position - transform.position;
            Vector3 direction = FlockManager.FM.goalPos - transform.position;
            direction.y = 0; // prevents NPCs from moving up or down
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(direction),
                                                  FlockManager.FM.rotationSpeed * Time.deltaTime);
        }
        else
        {

            if (Random.Range(0, 100) < 10) // randomly change the speed
            {
                speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
            }

            if (Random.Range(0, 100) < 10) // randomly apply the rules too
            {
                ApplyRules(); // this will turn fish towards the direction it needs to be moving in
            }
        }


        if (Vector3.Distance(transform.position, FlockManager.FM.goalPos) < 1f)
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


    /*
     
    This actually applies the flocking behavior to the NPC. 3 Rules:
    - Cohesion: Moves together with other nearby NPCs
    - Alignment: Moves in same direction as other NPCs
    - Avoidance: Avoid bumping into other NPCs
     
     */
    void ApplyRules()
    {
        GameObject[] gos; // gos means game objects. This refers to all the NPCs in the Flock Manager
        gos = FlockManager.FM.allNPCs;
        Vector3 vcentre = Vector3.zero; // center of nearby group
        Vector3 vavoid = Vector3.zero; // avoidance vector
        float gSpeed = 0.0f; // group average speed
        float nDistance; // neighbour distance
        int groupSize = 0; // all NPCs within in that distance

        foreach (GameObject go in gos)
        { // Iterate through all NPCs in the group 
            if (go != this.gameObject)
            { // Ignore self
                nDistance = Vector3.Distance(go.transform.position, this.transform.position); // Get distance from this NPC to that other NPC
                if (nDistance <= FlockManager.FM.neighbourDistance)
                { // Check if it is within a neighbourly distance
                    vcentre += go.transform.position;
                    groupSize++;

                    if (nDistance < 1.0f)
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
            vcentre = vcentre / groupSize + (FlockManager.FM.goalPos - this.transform.position); // get the centre of the group

            speed = gSpeed / groupSize; // get the speed of the group
            Vector3 direction = (vcentre + vavoid) - transform.position;

            if (speed > FlockManager.FM.maxSpeed)
            {
                speed = FlockManager.FM.maxSpeed;
            }

            if (direction != Vector3.zero)
            { // rotate and move towards new position
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(direction),
                                                      FlockManager.FM.rotationSpeed * Time.deltaTime);

            }
        }
    }
}
