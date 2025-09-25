using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SwimmingBackgroundAnimals : MonoBehaviour
{
    protected Rigidbody rb;

    [SerializeField]
    protected float speedMin;
    [SerializeField]
    protected float speedMax;
    protected float currentSpeed;

    [SerializeField]
    protected float minHeight;
    [SerializeField]
    protected float maxHeight;
    [SerializeField]
    protected float heightSpeedMin;
    [SerializeField]
    protected float heightSpeedMax;
    protected float height;
    protected float heightSpeed;

    [SerializeField]
    protected Animator anim;
    protected float currentAnimSpeed;
    protected float animOffset;

    [SerializeField]
    protected OnVisibleOffScreen onVis;

    [SerializeField]
    protected float inactiveWait;

    protected ObjectPooler pooler;
    [SerializeField]
    protected string ragdollKey;
    protected GameObject[] ragdolls;

    [SerializeField]
    protected ParticleSystem ps;

    protected Coroutine setInactiveWait;

    protected virtual void SetInactiveByTime()
    {
        setInactiveWait = StartCoroutine(SetInactiveWait());
    }

    protected virtual IEnumerator SetInactiveWait()
    {
        //Debug.Log("Started");
        yield return new WaitForSeconds(inactiveWait);
        while (onVis.isVisible)
        {
            yield return null;
        }
        ps.Stop();
        while (ps.IsAlive())
        {
            yield return null;
        }
        gameObject.SetActive(false);
        yield break;
    }

    public virtual void Die()
    {
        pooler.PoolObjects(ragdolls, transform.position, transform.rotation, Vector3.zero);
        gameObject.SetActive(false);
    }
}
