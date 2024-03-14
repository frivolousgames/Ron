using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInt : MonoBehaviour
{
    public void RandomizeInt(int num)
    {
        num = Random.Range(0, num);
    }
}
