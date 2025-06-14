using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerSpin : MonoBehaviour
{
    [SerializeField]
    float speed;

    private void Update()
    {
        transform.Rotate(speed * Time.deltaTime, 0f, 0f);
    }
}
