using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneController : MonoBehaviour
{
    Rigidbody rb;

    [Header("BASIC")]
    #region Basic
    [SerializeField]
    GameObject ragdoll;
    public Vector3 ragdollDirection;
    #endregion
    [Space(25.0f)]
    

    float newY;
    float newX;
    Vector3 rotMovement;
    Quaternion lookRotation;
   

    ///PHASE 1////
    
    bool phase_1;
    [Header("PHASE 1")]
    #region PHASE 1
    [SerializeField]
    float speed_1;
    [SerializeField]
    float yHeight_1;
    [SerializeField]
    float ySpeed_1;
    [SerializeField]
    float rotSpeed_1;
    #endregion
    [Space(25.0f)]

    ///PHASE 2///

    bool phase_2;
    [Header("PHASE 2")]
    #region PHASE 2 
    [SerializeField]
    float speed_2;
    [SerializeField]
    float yHeight_2;
    [SerializeField]
    float xLength_2;
    [SerializeField]
    float ySpeed_2;
    [SerializeField]
    float xSpeed_2;
    [SerializeField]
    float ySpeedMulti_2;
    [SerializeField]
    float xSpeedMulti_2;
    [SerializeField]
    float rotSpeed_2;
    [SerializeField]
    float flipWait;

    [SerializeField]
    float flipRotSpeed;
    #endregion
    bool flipStarted;
    bool isFlipping;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        phase_1 = true;
    }

    private void FixedUpdate()
    {
        Phase_1();
        Phase_2();
    }

    void Phase_1()
    {
        if (phase_1)
        { 
            newY = Mathf.SmoothStep(-yHeight_1, yHeight_1, Mathf.PingPong(Time.time * ySpeed_1, 1));
            rb.velocity = new Vector3( -speed_1, newY, 0f);
            rotMovement = new Vector3(0f, 0f, rb.velocity.y);
            lookRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(rb.velocity, Vector3.up), 360f * Time.deltaTime * rotSpeed_1);
            rb.MoveRotation(lookRotation);
        }
    }

    void Phase_2()
    {
        if (phase_2)
        {
            if (!flipStarted)
            {
                StartCoroutine(FlipRoutine());
                flipStarted = true;
            }
        }
    }

    IEnumerator FlipRoutine()
    {
        //b.velocity = new Vector3(-speed_1, 0f, 0f);
        yield return new WaitForSeconds(flipWait);
        isFlipping = true;
        while (isFlipping)
        {
            
            float newYLerp = Mathf.SmoothStep(0f, ySpeed_2, Mathf.PingPong(ySpeedMulti_2 * Time.time, 1));
            float newXLerp = Mathf.SmoothStep(xSpeed_2, 0f,  Mathf.PingPong(xSpeedMulti_2 * Time.time, 1));
            newY = Mathf.SmoothStep(-yHeight_1, yHeight_1, Mathf.PingPong(Time.time * newYLerp, 1));
            newX = Mathf.SmoothStep(xLength_2, -xLength_2, Mathf.PingPong(Time.time * newXLerp, 1));
            rb.velocity = new Vector3(newX, newY, 0f);
            //rb.MovePosition(new Vector3(xHigh, yHigh, transform.position.z));
            //rb.velocity = transform.right * -speed_1;
            //transform.Rotate(Time.deltaTime * rotSpeed_2, 0f, 0f);
            //rb.rotation = Quaternion.Euler(Time.deltaTime * rotSpeed_2, 0f, 0f);
            //lookRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(rb.velocity, Vector3.up), 360f * Time.deltaTime * rotSpeed_2);
            //rb.MoveRotation(Quaternion.Euler(Time.deltaTime * rotSpeed_2, 0f, 0f));
            //rb.MoveRotation(Quaternion.Lerp(transform.rotation, Quaternion.Euler(360f, 0f, 0f), flipRotSpeed * Time.deltaTime));
            //Debug.Log("xHigh: " + xHigh);
            yield return null;
        }
    }

    public void IsHit()
    {

    }

    public void Die()
    {
        ragdoll.transform.position = transform.position;
        ragdoll.transform.rotation = transform.rotation;
        ragdoll.SetActive(true);
        ragdollDirection = rb.velocity;
        gameObject.SetActive(false);
    }

}
