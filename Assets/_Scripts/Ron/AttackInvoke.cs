using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackInvoke : MonoBehaviour
{
    public UnityEvent attack;
    public UnityEvent uziShoot;

    public void Attack()
    {
        attack.Invoke();
    }

    public void UziShoot()
    {
        uziShoot.Invoke();
    }
}
