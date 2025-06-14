using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DwarfTossController : MonoBehaviour
{
    [SerializeField]
    GameObject dwarfRagdoll;
    [SerializeField]
    GameObject dwarfRagdollRig;

    [SerializeField]
    GameObject dwarfChild;

    Animator anim;
    [SerializeField]
    Animator dwarfAnim;

    [SerializeField]
    bool isSwinging;

    public static float launchSpeed;
    public static Vector3 launchAngle;

    [SerializeField]
    RectTransform arrowRect;
    [SerializeField]
    float arrowRotMin;
    [SerializeField]
    float arrowRotMax;
    [SerializeField]
    float arrowRotSpeed;

    [SerializeField]
    GameObject dwarfCam;

    [SerializeField]
    GameObject meterPanel;

    [SerializeField]
    Vector3 gravity;

    ///Distance///
    [SerializeField]
    TMP_Text playerDistanceText;
    Vector3 ragdollStartPos;
    [SerializeField]
    Transform ragdollSpine;
    float playerDistance;
    bool isAddingDistance;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        isSwinging = false;
        Physics.gravity = gravity;
        playerDistance = 0f;
    }

    private void Start()
    {
        ragdollStartPos = ragdollSpine.position;
    }

    private void OnEnable()
    {
        playerDistance = 0f;
    }

    private void Update()
    {
        anim.SetBool("isSwinging", isSwinging);
        RotateArrow();
        StopSwinging();
        SetPlayerDistance();
    }
    public void StartSwinging()
    {
        isSwinging = true;
        meterPanel.SetActive(true);
    }
    void StopSwinging()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            isSwinging = false;
        }
    }

    public void LaunchDwarf()
    {
        if(!isSwinging)
        {
            anim.SetTrigger("Launch");
            meterPanel.SetActive(false);
        }
    }
    public void ActivateDwarfRagdoll()
    {
        dwarfRagdoll.SetActive(true);
        dwarfCam.SetActive(true);
        isAddingDistance = true;
    }

    void RotateArrow()
    {
        if (isSwinging)
        {
            float zRot = Mathf.Lerp(arrowRotMin, arrowRotMax, Mathf.PingPong(Time.time * arrowRotSpeed, 1));
            arrowRect.rotation = Quaternion.Euler(0f, 0f, zRot);
        }
    }

    ///Distance///
    
    void SetPlayerDistance()
    {
        if(isAddingDistance)
        {
            playerDistance = Vector3.Distance(ragdollStartPos, ragdollSpine.position);
            playerDistanceText.text = Mathf.Round(playerDistance).ToString() + " ft";
            //Debug.Log("PlayerDistance: " + playerDistance);
        }
    }
}
