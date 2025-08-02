using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnParticleSystemStop : MonoBehaviour
{
    public UnityEvent stopped;
    private void OnParticleSystemStopped()
    {
        stopped.Invoke();
    }
}
