using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DwarfFanController : MonoBehaviour
{
    [SerializeField]
    UnityEvent fanLift;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Dwarf"))
        {
            fanLift.Invoke();
            Debug.Log("Fan");
        }
    }
}
