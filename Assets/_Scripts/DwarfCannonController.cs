using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DwarfCannonController : MonoBehaviour
{
    [SerializeField]
    GameObject cannonBarrel;
    Quaternion startRot;
    [SerializeField]
    float rotMin;
    [SerializeField]
    float rotMax;
    [SerializeField]
    float rotSpeed;

    bool isRotating;
    bool isFilled;

    [SerializeField]
    float initRotTime;
    [SerializeField]
    float initRotDelay;

    [SerializeField]
    UnityEvent fire;
    [SerializeField]
    UnityEvent filled;

    Animator anim;

    [SerializeField]
    GameObject smoke;

    [SerializeField]
    GameObject dwarfRagdoll;
    [SerializeField]
    Transform ragdollSpine;

    [SerializeField]
    GameObject cannonCam;

    float myTime = 0f;

    private void Awake()
    {
        anim =  GetComponent<Animator>();
        startRot = cannonBarrel.transform.rotation;
    }

    private void OnEnable()
    {
        isRotating = false;
        isFilled = false;
        myTime = .5f;
    }
    private void Update()
    {
        RotateBarrel();
        Fire();
    }
    void RotateBarrel()
    {
        if (isRotating)
        {
            float zRot = Mathf.Lerp(rotMin, rotMax, Mathf.PingPong(myTime * rotSpeed, 1));
            cannonBarrel.transform.localRotation = Quaternion.Euler(0f, zRot, 0f);
            myTime += Time.deltaTime;
            //Debug.Log("MyTime: " + myTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dwarf"))
        {
            if (!isFilled)
            {
                isFilled = true;
                filled.Invoke();
                dwarfRagdoll.SetActive(false);
                cannonCam.SetActive(true);
                StartCoroutine("InitialRot");
                Debug.Log("Filled");
            }
        }
    }

    IEnumerator InitialRot()
    {
        yield return new WaitForSeconds(initRotDelay);
        while (cannonBarrel.transform.localEulerAngles.y > 25.5f)
        {
            cannonBarrel.transform.Rotate(new Vector3(0f, 1f, 0f), initRotTime * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(initRotDelay);
        isRotating = true;
        yield break;
    }

    void Fire()
    {
        if(isRotating)
        {
            ragdollSpine.transform.position = smoke.transform.position;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isRotating = false;
                anim.SetTrigger("Fire");
                smoke.SetActive(true);
                dwarfRagdoll.SetActive(true);
                fire.Invoke();
                cannonCam.SetActive(false);
                Debug.Log("Fired");
            }
        }
    }

    public void ResetCannon()
    {
        isFilled = false;
        cannonBarrel.transform.rotation = startRot;
        myTime = .5f;
    }
}
