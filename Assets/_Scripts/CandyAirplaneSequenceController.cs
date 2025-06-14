using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyAirplaneSequenceController : MonoBehaviour
{
    [SerializeField]
    GameObject planes;

    private void OnEnable()
    {
        planes.SetActive(true);
    }
}
