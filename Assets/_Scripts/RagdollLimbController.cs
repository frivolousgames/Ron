using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderwaterGutsController : MonoBehaviour
{
    [SerializeField]
    Rigidbody[] boneRb;
    [SerializeField]
    float rbSpeed;
    Vector3 rbDirection;

    List<Vector3> bonePos;

    private void Awake()
    {
        bonePos = new List<Vector3>();
    }

    private void OnEnable()
    {
        rbDirection = EnemyController.ragdollDirection;
        foreach (var bone in boneRb)
        {
            bonePos.Add(bone.gameObject.transform.localPosition);
            bone.AddForce(new Vector3(-rbDirection.x * Random.Range(1f, 3f) * rbSpeed, Random.Range(2f, 5f) * rbSpeed, -rbDirection.z * Random.Range(1f, 3f) * rbSpeed), ForceMode.Impulse);
        }
    }

    private void OnDisable()
    {
        for(int i = 0; i < boneRb.Length; i++)
        {
            boneRb[i].gameObject.transform.localPosition = bonePos[i];
        }
    }
}
