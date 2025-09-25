using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpermShipHitController : MonoBehaviour
{
    [SerializeField]
    UnityEvent hit;
    [SerializeField]
    UnityEvent airborne;
    public static bool isHit;
    public static bool isGrabbed;
    public static Vector3 hitPos;
    public static int hitPower;
    public static bool isAirborne;
    Collider col;
    ObjectPooler pooler;

    //Health
    [SerializeField]
    UnityEvent healthAdd;
    public static float healthAmount;

    private void Awake()
    {
        col = GetComponent<Collider>();
        isHit = false;
        isGrabbed = false;
        pooler = new ObjectPooler();
    }

    private void OnEnable()
    {
        isHit = false;
        isGrabbed = false;
        if (PersistantPlayerData.isGrabbed)
        {
            //Debug.Log("Pos: " + PersistantPlayerData.otherGameObject.position + " " + "FrontTrans: " + frontTrans.position + " " + "BackTrans: " + backTrans.position);
            hitPower = PersistantPlayerData.hitPower;
            hitPos = PersistantPlayerData.otherGameObject.position;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 22)
        {
            if (!isHit)
            {
                //Debug.Log(other.gameObject.name);
                //Debug.Log("HitController Hit");
                //hitPos = other.gameObject.transform.position;
                hitPos = col.ClosestPoint(other.transform.position);
                pooler.PoolObjects(PooledObjectArrays.bloodHitsArray, hitPos, Quaternion.identity, Vector3.zero);
                hitPower = other.gameObject.GetComponent<EnemyWeapon>()._hitPower;
                isAirborne = other.gameObject.GetComponent<EnemyWeapon>()._isAirborne;
                hit.Invoke();
                isHit = true;
            }
        }
        else if (other.gameObject.CompareTag("GrabCollider"))
        {
            if (!isGrabbed)
            {
                Debug.Log("Hit: Grab");
                PersistantPlayerData.otherGameObject = other.gameObject.GetComponent<SetGrabHitpointTransform>().hitpointTransform;
                PersistantPlayerData.hitPower = other.gameObject.GetComponent<EnemyWeapon>()._hitPower;
                isGrabbed = true;
                PersistantPlayerData.isGrabbed = true;
            }
        }
        if (other.gameObject.layer == 23)
        {
            healthAmount = other.gameObject.GetComponent<HealthAddItem>().healthAmount;
            healthAdd.Invoke();
            Debug.Log("Health Item: " + healthAmount);
            other.gameObject.SetActive(false);
        }
    }
}
