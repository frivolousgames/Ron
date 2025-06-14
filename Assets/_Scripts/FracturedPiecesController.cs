using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FracturedPiecesController : MonoBehaviour
{
    [SerializeField]
    FracturedObjectController controller;
    Color color;

    private void Start()
    {
        color = controller.color;
    }
    private void OnDisable()
    {
        PoolObjects(controller.fractureHits, transform.position, Quaternion.identity, color);
    }

    public void PoolObjects(GameObject[] objs, Vector3 pos, Quaternion rot, Color color) //Add this object and instatiate in awake for any pooled objects
    {
        foreach (GameObject o in objs)
        {
            if (!o.activeInHierarchy)
            {
                o.SetActive(true);
                o.transform.position = pos;
                o.transform.rotation = rot;
                break;
            }
        }
    }
}
