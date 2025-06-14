using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    int maxHealth;
    [SerializeField]
    int health;
    [SerializeField]
    UnityEvent die;
    [SerializeField]
    bool isDead;
    int hitPower;

    private void OnEnable()
    {
        health = maxHealth;
        isDead = false;
    }

    private void Update()
    {
        Died(health);
    }

    void Died(int health)
    {
        if(health <= 0 && !isDead)
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

    public void VehicleIsHit()
    {
        hitPower = PlayerInfo.vehicleWeaponPower[PlayerController.selectedVehicleWeapon];
        health -= hitPower;
    }
}
