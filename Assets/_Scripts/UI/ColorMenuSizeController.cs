using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorMenuSizeController : MonoBehaviour
{
    GridLayoutGroup grid;
    int count;
    [SerializeField]
    int maxCount;

    private void OnEnable()
    {
        grid = GetComponent<GridLayoutGroup>();
        count = transform.childCount;
        if (count <= maxCount)
        {
            
            grid.constraintCount = count;
        }
        else
        {
            grid.constraintCount = maxCount;
        }
    }
}
