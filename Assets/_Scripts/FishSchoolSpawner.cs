using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSchoolSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] fish1;
    [SerializeField]
    GameObject[] fish2;
    [SerializeField]
    GameObject[] fish3;
    GameObject[][] fishPools;
    Color randomColorA;
    Color randomColorB;
    Color randomColorC;

    ObjectPooler pooler;

    [SerializeField]
    Transform fishSpawntrans;

    Coroutine spawnFishRoutine;
    [Tooltip("Minimum number of fish spawned in school")]
    [SerializeField]
    int amountMin;
    [Tooltip("Maximum number of fish spawned in school")]
    [SerializeField]
    int amountMax;
    [Tooltip("Minimum offset of fish on Y axis")]
    [SerializeField]
    float offsetYMin;
    [Tooltip("Maximum offset of fish on Y axis")]
    [SerializeField]
    float offsetYMax;
    [Tooltip("Minimum offset of fish on Y axis after starting new column")]
    [SerializeField]
    float offsetYResetMin;
    [Tooltip("Maximum offset of fish on Y axis after starting new column")]
    [SerializeField]
    float offsetYResetMax;
    [Tooltip("Minimum offset of fish school on screen on Y axis")]
    [SerializeField]
    float screenOffestYMin;
    [Tooltip("Maximum offset of fish school on screen on Y axis")]
    [SerializeField]
    float screenOffestYMax;
    [Tooltip("Minimum offset of fish on X axis")]
    [SerializeField]
    float offsetXMin;
    [Tooltip("Maximum offset of fish on X axis")]
    [SerializeField]
    float offsetXMax;
    [Tooltip("Minimum offset of fish on X axis for each column")]
    [SerializeField]
    float columnOffsetXMin;
    [Tooltip("Maximum offset of fish on X axis for each column")]
    [SerializeField]
    float columnOffsetXMax;
    [Tooltip("Minimum offset of fish on Z axis")]
    [SerializeField]
    float offsetZMin;
    [Tooltip("Maximum offset of fish on Z axis")]
    [SerializeField]
    float offsetZMax;
    [Tooltip("Minimum wait time for spawning new school")]
    [SerializeField]
    float spawnWaitMin;
    [Tooltip("Maximum wait time for spawning new school")]
    [SerializeField]
    float spawnWaitMax;


    private void Awake()
    {
        pooler = new ObjectPooler();
        fishPools = new GameObject[3][]//Add for more fish types
        {
            fish1,
            fish2,
            fish3
        };
    }

    private void Start()
    {
        spawnFishRoutine = StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while(true)
        {
            while (!SpermShipController.isTurning)
            {
                randomColorA = Random.ColorHSV(0, 1, 1, 1);
                randomColorB = Random.ColorHSV(0, 1, 1, 1);
                randomColorC = Random.ColorHSV(0, 1, 1, 1);

                int index = Random.Range(0, fishPools.Length);
                while (fishPools[index][0].activeInHierarchy)
                {
                    index = Random.Range(0, fishPools.Length);
                    yield return null;
                }
                int amount = Random.Range(amountMin, amountMax);
                float screenOffsetY = Random.Range(screenOffestYMin, screenOffestYMax);
                float offsetX = 0f;
                float offsetY = 0f;
                float offsetZ = 1f;
                int thresh = 0;
                int rowAmount = 1;
                for (int i = 0; i < amount; i++)
                {
                    var fishMats = fishPools[index][i].GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials;
                    
                    fishMats[0].SetColor("_Color1", randomColorA);
                    fishMats[0].SetColor("_Color2", randomColorB);
                    fishMats[1].SetColor("_Color", randomColorC);

                    
                    //Debug.Log("offsetY: " + offsetY);
                    pooler.PoolObjects(fishPools[index], new Vector3(fishSpawntrans.position.x + (offsetX * SpermShipController.directionX), fishSpawntrans.position.y + offsetY + screenOffsetY, fishSpawntrans.position.z + offsetZ), fishSpawntrans.rotation, Vector3.zero);
                    thresh++;
                    if(thresh == rowAmount)
                    {
                        rowAmount++;
                        thresh = 0;
                        offsetX += Random.Range(offsetXMin, offsetXMax);
                        if (rowAmount % 2 == 0)
                        {
                            offsetY = Random.Range(offsetYResetMin, offsetYResetMax);
                        }
                        else
                        {
                            offsetY = Random.Range(-.01f, .01f);
                        }
                    }
                    else
                    {
                        offsetX += Random.Range(columnOffsetXMin, columnOffsetXMax);
                        if (offsetY >= 0f)
                        {
                            offsetY += Random.Range(offsetYMin, offsetYMax);
                        }
                        else
                        {
                            offsetY -= Random.Range(offsetYMin, offsetYMax);
                        }
                        offsetY *= -1f;
                        offsetZ = Random.Range(offsetZMin, offsetZMax);
                    }  

                }
                float randomWait = Random.Range(spawnWaitMin, spawnWaitMax);
                yield return new WaitForSeconds(randomWait);
            }
            yield return null;
        }
    }
}
