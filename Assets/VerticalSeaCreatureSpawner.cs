using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalSeaCreatureSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] creatures;

    [SerializeField]
    float delaymin;
    [SerializeField]
    float delaymax;

    Coroutine spawnRoutine;
    bool isSpawning;

    ObjectPooler pooler;

    [SerializeField]
    Transform spawnTrans;

    [SerializeField]
    Vector3 spawnOffset;

    private void Awake()
    {
        pooler = new ObjectPooler();
    }

    private void OnEnable()
    {
        isSpawning = true;
        spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(Random.Range(delaymin, delaymax));
        while(isSpawning)
        {
            int i = Random.Range(0, creatures.Length);
            while (creatures[i].activeInHierarchy)
            {
                yield return null;
            }
            while (SpermShipController.isTurning)
            {
                yield return null;
            }
            pooler.PoolObjects(creatures[i], spawnTrans.position, spawnTrans.rotation, spawnOffset);
            yield return new WaitForSeconds(Random.Range(delaymin, delaymax));
        }
    }
}
