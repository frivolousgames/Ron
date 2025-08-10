using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler
{
    public void PoolObjects(GameObject[] objs, Vector3 pos, Quaternion rot, Vector3 spawnOffset) //Add this object and instatiate in awake for any pooled objects
    {
        foreach (GameObject o in objs)
        {
            if (!o.activeInHierarchy)
            {
                o.SetActive(true);
                o.transform.position = pos + spawnOffset;
                o.transform.rotation = rot;
                break;
            }
        }
    }

    public void PoolObjects(GameObject obj, Vector3 pos, Quaternion rot, Vector3 spawnOffset) //Add this object and instatiate in awake for any pooled objects
    {
        if (!obj.activeInHierarchy)
        {
            obj.SetActive(true);
            obj.transform.position = pos + spawnOffset;
            obj.transform.rotation = rot;
        }
    }
}
