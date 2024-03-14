using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LedgeCollider : MonoBehaviour
{
    GameObject grabCollider;
    GameObject ledgePlatform;

    MeshRenderer platformRect;

    Vector3 ledgeCornerPos;

    bool facingRight;
    bool collided;

    public static GameObject currentPlatform;
    public static Vector3 currentLedgeCornerPos;
    public static bool currentFacingRight;

    public UnityEvent grab;

    

    private void Awake()
    {
        ledgePlatform = transform.parent.gameObject;
        platformRect = ledgePlatform.GetComponent<MeshRenderer>();
        ledgeCornerPos = new Vector3(platformRect.bounds.min.x, platformRect.bounds.max.y, 0f);
        
    }

    private void Start()
    {
        grabCollider = GameObject.FindGameObjectWithTag("GrabCollider");
        if (ledgeCornerPos.x > grabCollider.gameObject.transform.parent.position.x)
        {
            facingRight = true;
        }
        else
        {
            ledgeCornerPos.x = platformRect.bounds.max.x;
            facingRight = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == grabCollider)
        {
            if (!collided)
            {
                currentPlatform = ledgePlatform;
                currentLedgeCornerPos = ledgeCornerPos;
                currentFacingRight = facingRight;
                grab.Invoke();
                Debug.Log("Collided");
                collided = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == grabCollider)
        {
            Debug.Log("Exited");
            collided = false;
        }
    }
}
