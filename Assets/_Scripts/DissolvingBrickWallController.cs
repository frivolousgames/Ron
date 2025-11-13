using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolvingBrickWallController : MonoBehaviour
{
    public static bool isDissolving;
    [SerializeField]
    GameObject wall;
    [SerializeField]
    GameObject bricks;
    [SerializeField]
    GameObject brickRemnants;
    [SerializeField]
    GameObject particles;

    private void Awake()
    {
        isDissolving = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (!isDissolving)
            { 
                wall.SetActive(false);
                bricks.SetActive(true);
                brickRemnants.SetActive(true);
                particles.SetActive(true);
                isDissolving = true;
                Debug.Log("Dissolving");
            }
        }
    }
}
