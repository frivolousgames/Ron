using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealthBase : MonoBehaviour
{
    [SerializeField]
    protected int maxHealth;
    [SerializeField]
    protected int health;
    [SerializeField]
    protected UnityEvent die;
    [SerializeField]
    protected bool isDead;
    protected int hitPower;

    void Died(int health)
    {
        if (health <= 0 && !isDead)
        {
            isDead = true;
            die.Invoke();
        }
    }

    public void IsHit()
    {
        hitPower = PlayerInfo.weaponPower[PlayerController.currentWeapon];
        health -= hitPower;
    }
}
