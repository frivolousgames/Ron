using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamRotate : MonoBehaviour
{
    public Transform ape;
    public CinemachineVirtualCamera virtualCamera;
    public float rotSpeed;
    private void Update()
    {
        transform.RotateAround(ape.position, new Vector3(0, 1, 0), Time.deltaTime * rotSpeed);
    }
}
