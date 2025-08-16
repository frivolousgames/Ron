using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBulletMover : MonoBehaviour
{
    protected Rigidbody rb;
    [SerializeField]
    protected float bulletSpeed;
    protected Vector3 startPos;

    protected GameObject[] dust;
    protected Vector3 hitPoint;

    protected GameObject[] bulletHoles;

    protected GameObject[] bloodHits;
    protected GameObject[] chunkHits;
    protected Vector3 hitNormal;

    protected Vector3 spawnOffset;
    protected GameObject spawnedPool;

    [SerializeField]
    protected int damageAmount;

    protected ObjectPooler pooler;
}
