using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SwimmingBackgroundAnimals : MonoBehaviour
{
    protected Rigidbody rb;

    [SerializeField]
    protected float normalSpeed;
    [SerializeField]
    protected float escapeSpeed;
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
    [SerializeField]
    protected float animNormalSpeed;
    [SerializeField]
    protected float animEscapeSpeed;
    protected float currentAnimSpeed;
    protected float animOffset;

    protected Coroutine setInactiveWait;


}
