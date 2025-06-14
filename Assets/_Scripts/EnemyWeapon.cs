using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField]
    int hitPower;
    [HideInInspector]
    public int _hitPower;
    [SerializeField]
    bool isAirborne;
    [HideInInspector]
    public bool _isAirborne;

    private void Awake()
    {
        _hitPower = hitPower;
        _isAirborne = isAirborne;
    }
}
