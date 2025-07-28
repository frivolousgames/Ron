using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneShooter : MonoBehaviour
{
    [SerializeField]
    Camera mainCam;

    [SerializeField]
    Transform crosshairs;

    [SerializeField]
    GameObject farTarget;
    [SerializeField]
    float farTargetDistance;

    [SerializeField]
    float offsetX;
    [SerializeField]
    float offsetY;

    ObjectPooler pooler;

    float shootWait;
    Coroutine shootRoutine;

    private void Awake()
    {
        pooler = new ObjectPooler();
        shootWait = PlayerInfo.vehicleWeaponShootWait[PlayerController.selectedVehicleWeapon];
    }

    private void OnEnable()
    {
        shootRoutine = StartCoroutine(Shoot());
    }

    private void OnDisable()
    {
        StopCoroutine(shootRoutine);
    }

    private void Update()
    {
        
        Vector3 farPos = mainCam.ScreenToWorldPoint(new Vector3(crosshairs.position.x, crosshairs.position.y, farTargetDistance));
        float offsetXNew = Mathf.Round(crosshairs.localPosition.x * 10f)  * .01f * offsetX;
        float offsetYNew = Mathf.Round(crosshairs.localPosition.y * 10f) * .01f * offsetY;
        farTarget.transform.position = new Vector3(farPos.x + offsetXNew, farPos.y + offsetYNew , farTargetDistance);
        transform.rotation = Quaternion.LookRotation((farTarget.transform.position - transform.position).normalized, Vector3.up);
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            while (Input.GetMouseButton(0))
            {
                pooler.PoolObjects(PooledObjectArrays.airplaneBulletsArray, transform.position, transform.rotation, Vector3.zero);
                yield return new WaitForSeconds(shootWait);
            }
            yield return null;
        }
    }
}
