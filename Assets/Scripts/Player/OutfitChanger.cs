using System.Collections;
using UnityEngine;

public class OutfitChanger : MonoBehaviour
{
    public GameObject uiPanel; // assign in Inspector
    public WomanDressUI uiScript; // reference to the UI script
    private bool isInsideStore = false;
    public bool disguiseOn = false;

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

    private void Update()
    {
        if (disguiseOn == true) {
            StartCoroutine(StartDisguiseCountDown());
        }
    }

    IEnumerator StartDisguiseCountDown()
    {
        Debug.Log("disguise on");
        disguiseOn = true;
        yield return new WaitForSeconds(25f);
        disguiseOn = false;
        Debug.Log("disguise off");
    }
}
