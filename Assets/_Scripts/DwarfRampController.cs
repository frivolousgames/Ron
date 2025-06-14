using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DwarfRampController : MonoBehaviour
{
    [SerializeField]
    UnityEvent rampLaunch;
    bool isHit;

    [SerializeField]
    GameObject bloodHit;
    private void OnTriggerEnter(Collider other)
    {
        if (!isHit)
        {
            if (other.gameObject.CompareTag("Dwarf"))
            {
                isHit = true;
                rampLaunch.Invoke();
                if(bloodHit != null)
                {
                    bloodHit.SetActive(true);
                }
                Debug.Log("Ramp");

            }
        }
    }
}
