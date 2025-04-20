using UnityEngine;
using System.Collections;

public class AuditionNavigator : MonoBehaviour
{
    public Transform player;                         // The player
    public GameObject arrow;                         // The arrow object (child of player)
    public Transform[] auditionCenters;              // Audition centers in order
    public float delayBeforeNextTarget = 3f;
    public float completionDistance = 3f;            // Distance to mark as completed
    public bool shouldLog = false;                   // Toggle distance logging

    private int currentTargetIndex = 0;
    private Transform currentTarget;
    private bool isWaiting = false;

    void Start()
    {
        if (auditionCenters.Length > 0)
        {
            currentTarget = auditionCenters[0];
        }
    }

    void Update()
    {
        if (currentTarget == null || player == null || isWaiting) return;

        Vector3 direction = currentTarget.position - player.position;
        float distance = direction.magnitude;

        if (shouldLog)
        {
            Debug.Log($"Distance to current audition center: {distance:F2} units");
        }

        direction.y = 0f; // Prevent vertical rotation if needed

        // Rotate arrow to face target with model offset
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion modelOffset = Quaternion.Euler(292.76f, 179.95f, 270.05f); // your prefab's default rotation
            arrow.transform.rotation = targetRotation * modelOffset;
        }

        if (distance < completionDistance)
        {
            StartCoroutine(HandleNextTarget());
        }
    }

    IEnumerator HandleNextTarget()
    {
        isWaiting = true;
        arrow.SetActive(false);

        yield return new WaitForSeconds(delayBeforeNextTarget);

        currentTargetIndex++;
        if (currentTargetIndex < auditionCenters.Length)
        {
            currentTarget = auditionCenters[currentTargetIndex];
            arrow.SetActive(true);
        }
        else
        {
            currentTarget = null;
        }

        isWaiting = false;
    }
}
