using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleFishSpawner : MonoBehaviour
{
    ObjectPooler pooler;
    [SerializeField]
    GameObject[] fish; //Add new fish here
    int chosenFishIndex;
    [SerializeField]
    Transform fishSpawnTrans;
    float screenOffsetY;
    float offsetZ;
    [SerializeField]
    float spawnDelayMin;
    [SerializeField]
    float spawnDelayMax;
    [SerializeField]
    float screenOffsetYMin;
    [SerializeField]
    float screenOffsetYMax;
    [SerializeField]
    float offsetX;

    Coroutine spawnFishRoutine;

    private void Awake()
    {
        pooler = new ObjectPooler();
        offsetZ = 1f;  
    }

    private void OnEnable()
    {
        spawnFishRoutine = StartCoroutine(SpawnDelay());
    }

    private void OnDisable()
    {
        if(spawnFishRoutine != null)
        {
            StopCoroutine(spawnFishRoutine);
        }
    }

    void SpawnFish()
    {
        Vector3 offset = new Vector3(offsetX * SpermShipController.directionX, screenOffsetY, offsetZ);
        pooler.PoolObjects(fish[chosenFishIndex], fishSpawnTrans.position, fishSpawnTrans.rotation, offset);
        //pooler.PoolObjects(fish[chosenFishIndex], new Vector3(fishSpawnTrans.position.x + (offsetX * SpermShipController.directionX), fishSpawnTrans.position.y + screenOffsetY, fishSpawnTrans.position.z + offsetZ), fishSpawnTrans.rotation, Vector3.zero);
    }

    IEnumerator SpawnDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(spawnDelayMin, spawnDelayMax));
            screenOffsetY = Random.Range(screenOffsetYMin, screenOffsetYMax);
            //randomize weighted (rare to common) to get chosenFishIndex
            chosenFishIndex = Random.Range(0, fish.Length); //TEMP
            while (fish[chosenFishIndex].activeInHierarchy)
            {
                //randomize fish
                yield return null;
            }
            while (SpermShipController.isTurning)
            {
                yield return null;
            }
            SpawnFish();
            yield return null;
        }
    }
}
