using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdollController : MonoBehaviour
{
    [SerializeField]
    GameObject[] arms;

    [SerializeField]
    GameObject[] legs;

    [SerializeField]
    GameObject[] intestines;

    [SerializeField]
    GameObject[] livers;

    [SerializeField]
    GameObject[] ragdollArmBones;

    [SerializeField]
    GameObject[] ragdollLegBones;

    List<Vector3> armStartScales;
    List<Vector3> legStartScales;

    Vector3 spawnOffset = Vector3.zero;

    ObjectPooler pooler;
    private void Awake()
    {
        armStartScales = new List<Vector3>();
        legStartScales = new List<Vector3>();
        SaveStartScales(armStartScales, ragdollArmBones);
        SaveStartScales(legStartScales, ragdollLegBones);

        pooler = new ObjectPooler();
    }

    private void OnEnable()
    {
        SpawnLimbs(arms, ragdollArmBones);
        SpawnLimbs(legs, ragdollLegBones);
        SpawnGuts();
    }

    private void OnDisable()
    {
        ResetBoneScales(armStartScales, ragdollArmBones);
        ResetBoneScales(legStartScales, ragdollLegBones);
    }

    void SpawnLimbs(GameObject[] limbs, GameObject[] bones)
    {
        int i = Random.Range(0, 3);
        for (int j = 0; j < i; j++)
        {
            pooler.PoolObjects(limbs, transform.position, Quaternion.identity, Vector3.zero);
            bones[j].transform.localScale = Vector3.zero;
        }   
    }

    void SpawnGuts()
    {
        int i = Random.Range(0, 3);
        if (i == 0)
        {
            pooler.PoolObjects(intestines, transform.position, Quaternion.identity, Vector3.zero);
        }
        else if (i == 1)
        {
            pooler.PoolObjects(livers, transform.position, Quaternion.identity, Vector3.zero);
        }
    }

    void SaveStartScales(List<Vector3> startScales, GameObject[] limbs)
    {
        foreach(GameObject l in limbs)
        {
            startScales.Add(l.transform.localScale);
        }
    }

    void ResetBoneScales(List<Vector3> startScales, GameObject[] limbs)
    {
        for(int i = 0; i < limbs.Length; i++)
        {
            limbs[i].transform.localScale = startScales[i];
        }
    }
}
