using UnityEngine;

public class OutfitChanger : MonoBehaviour
{
    public GameObject uiPanel; // assign in Inspector
    public WomanDressUI uiScript; // reference to the UI script
    private bool isInsideStore = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Clothing Store") && !uiPanel.activeSelf)
        {
            isInsideStore = true;
            uiPanel.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Clothing Store"))
        {
            isInsideStore = false;
            uiPanel.SetActive(false);
        }
    }
}
