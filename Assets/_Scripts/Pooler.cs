using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    public GameObject[] bullets;
    int spawnPos;
    public Transform[] spawnPosTrans;
    Transform currentSpawnPos;

    //private void Start()
    //{
    //    foreach (var bullet in bullets)
    //    {
    //        bullet.SetActive(false);
    //    }
    //}

    public void Pool()
    {
        spawnPos = PlayerInfo.bulletSpawn[PlayerController.currentWeapon];
        currentSpawnPos = spawnPosTrans[spawnPos];
        if (currentSpawnPos == null)
        {
            Debug.LogError("currentSpawnPos is NULL");
        }
        foreach (GameObject b in bullets)
        {
            if(!b.activeInHierarchy)
            {
                b.SetActive(true);
                b.transform.position = currentSpawnPos.position;
                b.transform.rotation = currentSpawnPos.rotation;
                break;
            }
        }
    }
}
