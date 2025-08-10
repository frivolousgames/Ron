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
        pooler.PoolObjects(fish[chosenFishIndex], new Vector3(fishSpawnTrans.position.x + (offsetX * SpermShipController.directionX), fishSpawnTrans.position.y + screenOffsetY, fishSpawnTrans.position.z + offsetZ), fishSpawnTrans.rotation, Vector3.zero);
        Debug.Log("Pos: " + fishSpawnTrans.position.x);
    }

    IEnumerator SpawnDelay()
    {
        while(true)
        {
            while (!SpermShipController.isTurning)
            {
                yield return new WaitForSeconds(Random.Range(spawnDelayMin, spawnDelayMax));
                screenOffsetY = Random.Range(screenOffsetYMin, screenOffsetYMax);
                //randomize weighted (rare to common) to get chosenFishIndex
                chosenFishIndex = 2; //TEMP
                SpawnFish();
                yield return null;
            }
            yield return null;    
        }
    }
}
