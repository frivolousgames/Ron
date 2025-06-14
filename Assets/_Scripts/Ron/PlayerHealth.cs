using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    float maxHealth;
    [SerializeField]
    float health;
    public static float _maxHealth;
    public static float _health;

    private void Awake()
    {
        //health = maxHealth;
        _maxHealth = maxHealth;
        _health = health;//temp until serializer is set up
    }

    private void Update()
    {
        _maxHealth = maxHealth;
        _health = health;
    }
    public void Damage()
    {
        health -= PlayerHitController.hitPower;
        //Debug.Log("Power: " + PlayerHitController.hitPower);
    }

    public void AddHealth()
    {
        health += PlayerHitController.healthAmount;
    }
}
