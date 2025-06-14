using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunisherController : MonoBehaviour
{
    Animator anim;
    public Animator playerAnim;

    public bool isMoving;
    public bool isRunning;
    public bool isShooting;
    public bool isGrounded;
    public bool isFalling;
    float shootWait;

    public static Vector3 hitPoint;
    public static Vector3 hitNormal;



    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        isMoving = playerAnim.GetBool("isMoving");
        isRunning = playerAnim.GetBool("isRunning");
        isShooting = playerAnim.GetBool("isShooting");
        isGrounded = playerAnim.GetBool("isGrounded");
        isFalling = playerAnim.GetBool("isFalling");

        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isShooting", isShooting);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isFalling", isFalling);

        playerAnim.SetBool("weaponRaised", true);

        //Shoot();

        //Debug.Log("IsShooting: " + isShooting);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, 2f))
        {
            if (hit.collider)
            {
                hitPoint = hit.point;
                hitNormal = hit.normal;
            }
        }
    }

    private void OnEnable()
    {
        shootWait = PlayerInfo.shootWait[PlayerController.currentWeapon];
        //Debug.Log("ShootWait: " + shootWait);
    }

    public void Thriust()
    {
        anim.SetTrigger("Attack");
    }

    IEnumerator ShootWait()
    {
        //shoot
        yield return new WaitForSeconds(shootWait);
        isShooting = false;
    }
}
