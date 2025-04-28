using Unity.Hierarchy;
using UnityEngine;

public class BasicTeleport : MonoBehaviour
{
    public Transform target = null;
    public PlayerMoneyManager moneyManager;
    public GameObject player;
    public Canvas taxiCanvas;
    public float proximityDistance = 5f;

    private void Start()
    {
        taxiCanvas.enabled = false;
    }
    private void Update()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        Debug.Log("This is the current distance away from taxi: " + distance);
        if (distance < proximityDistance)
        {
            taxiCanvas.enabled = true;
            if (Input.GetKeyDown(KeyCode.T)) { 
                player.transform.position = target.transform.position;
                GetComponent<AudioSource>().Play();
                moneyManager.DeductMoney(200);
                taxiCanvas.enabled = false;
            }
        }
        else {
            taxiCanvas.enabled = false;
        }
        
       
    }

}