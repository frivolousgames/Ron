using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanManController : MonoBehaviour
{
    Rigidbody rb;

    Vector3 startPosition;
    Quaternion startRotation;

    bool spinning;
    [SerializeField]
    float spinSpeed;
    public GameObject fanBlades;

    GameObject player;
    Vector3 playerPos;
    Quaternion playerRot;

    bool isAttacking;
    [SerializeField]
    float attackDistance;
    [SerializeField]
    float attackSpeed;

    [SerializeField]
    GameObject wiring;

    [SerializeField]
    Collider weaponCol;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private void OnEnable()
    {
        spinning = true;
        weaponCol.enabled = true;
    }

    private void OnDisable()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        wiring.SetActive(false);
    }

    private void Update()
    {
        if (spinning)
        {
            fanBlades.transform.Rotate(Vector3.up, Time.deltaTime * spinSpeed);
        }
        //playerDistance = Vector3.Distance(player.transform.position, transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                wiring.SetActive(true);
                StartCoroutine(AttackRoutine());
            }
        }
    }

    public void HitGround()
    {
        spinning = false;
        rb.velocity = Vector3.zero;
        weaponCol.enabled = false;
    }

    IEnumerator AttackRoutine()
    {
        float i = 0;
        playerRot = Quaternion.LookRotation((player.transform.position - transform.position).normalized);
        playerPos = player.transform.position;
        while (spinning)
        {
            Attack(i);
            i += attackSpeed * Time.deltaTime;
            yield return null;
        }
        yield break;
    }

    void Attack(float i)
    {
        rb.MovePosition(Vector3.Lerp(transform.position,player.transform.position, i));
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, playerRot, i * 3));
    }
}
