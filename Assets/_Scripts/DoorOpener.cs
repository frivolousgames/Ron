using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    public static GameObject currentDoor;
    public static bool doorTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            currentDoor = gameObject;
            doorTriggered = true;
            //"Hit E to open"
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            doorTriggered = false;
        }
       
    }
}
