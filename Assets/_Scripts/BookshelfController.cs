using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BookshelfController : MonoBehaviour
{
    [SerializeField]
    GameObject[] books;

    [SerializeField]
    float jumpSpeed;

    bool isClose;
    bool isJumping;

    GameObject player;
    float playerDistance;
    [SerializeField]
    float activateDist;
    [SerializeField]
    float deActivateDist;

    Rigidbody rb;

    [SerializeField]
    ParticleSystem[] pcs;

    [SerializeField]
    UnityEvent start;
    [SerializeField]
    UnityEvent stop;

    [SerializeField]
    UnityEvent reset;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(player.transform.position, transform.position);
        //Debug.Log("PlayerDistance: " + playerDistance);
        if (!isClose)
        {
            if(playerDistance < activateDist)
            {
                isClose = true;
                StartCoroutine(AttackRoutine());
            }
        }
        ResetBooks();
    }

    IEnumerator AttackRoutine()
    {
        isJumping = true;
        float i = 0;
        while (i < 5)
        {
            JumpAround();
            i++;
            Debug.Log("Jumping");
            yield return new WaitForSeconds(.3f);
        }
        isJumping = false;
        foreach (GameObject book in books)
        {
            if (book != null)
            {
                book.SetActive(true);
                yield return new WaitForSeconds(.1f);
            }
            yield return null;
        }
    }
    void JumpAround()
    {
        // transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Time.deltaTime * jumpSpeed);
        rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            start.Invoke();
            JumpDust();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            stop.Invoke();
            foreach(var pc in pcs)
            {
                pc.Stop();
            }
        }
    }

    void JumpDust()
    {
        if (isJumping)
        {
            foreach (var pc in pcs)
            {
                pc.Play();
            }
        }
    }

    private void ResetBooks()
    {
        if(isClose)
        {
            if (playerDistance > deActivateDist)
            {
                isClose = false;
                reset.Invoke();
            }
        } 
    }
}
