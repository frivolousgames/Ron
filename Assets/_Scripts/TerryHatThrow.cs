using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TerryHatThrow : MonoBehaviour
{
    Rigidbody rb;

    Vector3 startPos;
    [SerializeField]
    Transform startSpawnPos;
    [SerializeField]
    Transform endPos;

    GameObject player;
    float playerDir;
    Vector3 playerPos;

    Vector3 direction;
    bool flyingForward;
    bool flyingBackwards;
    bool returning;
    [SerializeField]
    float forwardSpeed;
    [SerializeField]
    float pingPongSpeed;
    [SerializeField]
    float pingPongWidth;
    [SerializeField]
    float pingPongWidthMulti;
    float pingPongWidthStart;
    [SerializeField]
    float distanceThreshold;
    [SerializeField]
    float returnThreshold;
    [SerializeField]
    UnityEvent hatReturned;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        pingPongWidthStart = pingPongWidth;
    }

    private void OnEnable()
    {
        transform.position = startSpawnPos.position;
        startPos = transform.position;
        flyingForward = true;
        StartCoroutine(FlyRoutine());
    }

    private void OnDisable()
    {
        pingPongWidth = pingPongWidthStart;
        StopAllCoroutines();
        hatReturned.Invoke();
    }

    private void Update()
    {
        playerPos = player.transform.position;
        DistanceCheck();
        ReturnDistanceCheck();
        EndDistanceCheck();
    }

    IEnumerator FlyRoutine()
    {
        while(flyingForward)
        {
            FlyForward();
            GrowWidth(1);
            yield return null;
        }
        while (flyingBackwards)
        {
            //FlyBackward();
            //GrowWidth(-1);
            ReturnToStart();
            yield return null;
        }
        while (returning)
        {
            ReturnToStart();
            yield return null;
        }
        gameObject.SetActive(false);
        yield break;
    }

    void FlyForward()
    {
        float newZ = Mathf.SmoothStep(-pingPongWidth, pingPongWidth, Mathf.PingPong(Time.time * pingPongSpeed, 1));
        rb.velocity = new Vector3(forwardSpeed, 0f, newZ);
    }

    void FlyBackward()
    {
        float newZ = Mathf.SmoothStep(-pingPongWidth, pingPongWidth, Mathf.PingPong(Time.time * pingPongSpeed, 1));
        direction = (endPos.position - transform.position).normalized;
        rb.velocity = new Vector3(direction.x * forwardSpeed, direction.y * forwardSpeed, newZ);
    }

    void GrowWidth(float i)
    {
        if(pingPongWidth > 0)
        {
            pingPongWidth += pingPongWidthMulti * i;
        }
    }

    void DistanceCheck()
    {
        if((transform.position.x - startPos.x) > distanceThreshold)
        {
            if (flyingForward)
            {
                flyingForward = false;
                flyingBackwards = true;
            }
        }
    }

    void ReturnDistanceCheck()
    {
        if ((transform.position.x - endPos.position.x) < returnThreshold)
        {
            if (flyingBackwards)
            {
                flyingBackwards = false;
                returning = true;
            }
        }
    }

    void EndDistanceCheck()
    {
        if (transform.position.x < endPos.position.x)
        {
            if (returning)
            {
                returning = false;
            }
        }
    }

    void ReturnToStart()
    {
        direction = (endPos.position - transform.position).normalized;
        rb.velocity = direction * forwardSpeed * 3;
    }
}
