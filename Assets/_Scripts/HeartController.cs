using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeartController : MonoBehaviour
{
    [SerializeField]
    UnityEvent heartDamage;
    [SerializeField]
    UnityEvent heartAdd;
    [SerializeField]
    UnityEvent stopPS;

    private void Start()
    {
        StopAdd();
    }

    public void HeartDamage()
    {
        heartDamage.Invoke();
    }

    public void AddHearts()
    {
        heartAdd.Invoke();
    }

    public void StopAdd()
    {
        stopPS.Invoke();
    }
}
