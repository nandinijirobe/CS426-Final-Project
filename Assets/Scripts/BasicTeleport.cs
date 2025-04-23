using UnityEngine;

public class BasicTeleport : MonoBehaviour
{
    public Transform target = null;
    public PlayerMoneyManager moneyManager;
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit taxi");
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Player") {
            Debug.Log("Entering Portal #1");
            other.gameObject.transform.position = target.position;
            GetComponent<AudioSource>().Play();
            Debug.Log("Going to Portal #2");

            moneyManager.DeductMoney(200);
        }
    }
}