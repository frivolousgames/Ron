using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DwarfController : MonoBehaviour
{
    [SerializeField]
    Rigidbody[] rbs;

    ///Start///
    bool launched;
    [SerializeField]
    float launchSpeed;
    Vector3 launchAngle;
    [SerializeField]
    float launchAngleMulti;
    [SerializeField]
    GameObject arrow;

    ///Ramp///
    [SerializeField]
    float rampSpeed;
    [SerializeField]
    Vector3 rampAngle;

    ///Tramp///
    [SerializeField]
    float trampSpeed;
    [SerializeField]
    Vector3 trampAngle;

    ///Spike board///
    [SerializeField]
    float velDelay;

    ///Spin Board///
    [SerializeField]
    float spinVelDiv;
    [SerializeField]
    float spinVelMulti;

    ///Cannon///
    [SerializeField]
    GameObject cannonBarrel;
    [SerializeField]
    float cannonSpeed;
    [SerializeField]
    float cannonAngleMulti;

    ///Fan///
    [SerializeField]
    float fanPower;

    ///End///
    bool isEnding;
    bool isGrounded;
    bool isOver;
    [SerializeField]
    float endWaitTime;
    [SerializeField]
    UnityEvent roundOver;


    private void Awake()
    {
        float arrowZ = Mathf.Abs(arrow.transform.rotation.normalized.z) * launchAngleMulti;
        launchAngle = new Vector3(arrowZ, .5f, 0f);
        //Debug.Log("Launch Angle: " + launchAngle);
        LaunchDwarf(launchSpeed);
    }
    private void OnEnable()
    {

    }
    private void Update()
    {
        isGrounded = CheckIsGrounded.isGrounded;

    }
    private void FixedUpdate()
    {
        foreach (var rbs in rbs)
        {
            rbs.velocity = Vector3.ClampMagnitude(rbs.velocity, 100f);
        }
        Debug.Log("Vel: " + rbs[0].velocity.magnitude);
        
    }

    ///Start///
    void LaunchDwarf(float speed)
    {
        foreach (var rbs in rbs)
        {
            rbs.AddForce(launchAngle * speed, ForceMode.Impulse);
        }
    }

    ///Ramp///
    public void RampLaunch()
    {
        float velMag = Mathf.Round(rbs[0].velocity.magnitude) / 100 + 1;
        foreach (var rb in rbs)
        {
            rb.AddForce(rampAngle * rampSpeed * velMag, ForceMode.Impulse);
        }
    }

    ///Trampoline///
    public void TrampolineLaunch()
    {
        float velMag = Mathf.Round(rbs[0].velocity.magnitude) / 100 + 1;

        foreach (var rb in rbs)
        {
            rb.AddForce(trampAngle * trampSpeed * velMag, ForceMode.Impulse);
        }
    }

    ///Spike Board///
    public void SpikeBoardHit()
    {
        StartCoroutine(StopVelocity()); 
    }

    IEnumerator StopVelocity()
    {
        foreach (var rb in rbs)
        {
            rb.isKinematic = true; 
        }
        yield return new WaitForSeconds(velDelay);
        foreach (var rb in rbs)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
        }
        yield break;
    }

    ///Spin Board///
    public void SpinBoardPositive()
    {
        foreach (var rb in rbs)
        {
            rb.AddForce(Vector3.right * spinVelMulti, ForceMode.Impulse);
        }
    }

    public void SpinBoardNegative()
    {
        foreach (var rb in rbs)
        {
            
            rb.velocity = -rb.velocity * spinVelDiv;
        }
    }

    ///Cannon///

    public void ResetVelocity()
    {
        foreach (var rb in rbs)
        {

            rb.velocity = Vector3.zero;
        }
    }

    public void CannonLaunch()
    {
        float cannonZ = Mathf.Abs(cannonBarrel.transform.localRotation.normalized.y) * cannonAngleMulti;
        launchAngle = new Vector3(1f, cannonZ, 0f);
        Debug.Log("Angle: " + launchAngle);
        LaunchDwarf(cannonSpeed);
    }

    ///Fan///
    
    public void FanLift()
    {
        launchAngle = new Vector3(.1f /*+ rbs[0].velocity.x*/, 1f + Mathf.Abs(rbs[0].velocity.y), 0f);
        foreach (var rbs in rbs)
        {
            rbs.AddForce(launchAngle * fanPower, ForceMode.Force);
        }
    }

    ///End///
    void EndTry()
    {
        if (!isEnding && !isOver)
        {
            if (isGrounded && rbs[0].velocity.magnitude < .15f)
            {
                StartCoroutine(EndWait());
            }
        }   
    }

    IEnumerator EndWait()
    {
        while (isGrounded)
        {
            yield return new WaitForSeconds(endWaitTime);
            isOver = true;
            roundOver.Invoke();
            yield break;
        }
        isEnding = false;
        yield break;
    }
}
