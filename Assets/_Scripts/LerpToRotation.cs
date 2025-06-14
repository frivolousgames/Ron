using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToRotation : MonoBehaviour
{
    Quaternion endRot;
    [SerializeField]
    float rotSpeed;

    private void Awake()
    {
        endRot = Quaternion.Euler(322.600006f, 87.8883972f, 93.2912369f);
    }

    private void Update()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, endRot, rotSpeed * Time.deltaTime);
    }
}
