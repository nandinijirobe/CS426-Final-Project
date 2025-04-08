using UnityEngine;

public class PlayerStreetTypeDetector : MonoBehaviour
{
    public BayesianDecisionManager decisionManager;

    private void OnTriggerEnter(Collider other)
    {
        StreetZone zone = other.GetComponent<StreetZone>();
        if (zone != null && decisionManager != null)
        {
            decisionManager.currentStreetType = zone.streetType;
            Debug.Log($"Entered {zone.streetType} zone.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StreetZone zone = other.GetComponent<StreetZone>();
        if (zone != null && decisionManager != null)
        {
            // Reset to Regular on exit unless already in another zone (optional refinement later)
            decisionManager.currentStreetType = BayesianDecisionManager.StreetType.Regular;
            Debug.Log($"Exited {zone.streetType} zone. Reset to Regular.");
        }
    }
}
