using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FanBladeController : MonoBehaviour
{
    [SerializeField]
    private UnityEvent hitFloor;

    Collider hitCollider;
    [SerializeField]
    GameObject groundChunks;

    public static bool hit;

    private void Awake()
    {
        hitCollider = GetComponent<Collider>();
    }
    private void OnEnable()
    {
        hit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            if (!hit)
            {
                hit = true;
                hitFloor.Invoke();
                groundChunks.transform.position = hitCollider.bounds.center;
                groundChunks.transform.rotation = Quaternion.identity;
                groundChunks.SetActive(true);
            }
        }
    }
}
