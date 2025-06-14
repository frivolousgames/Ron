using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperController : MonoBehaviour
{
    Animator anim;
    public bool hasBlunt;
    public int num;

    [SerializeField]
    float floatSpeed;
    [SerializeField]
    float floatHeight;

    [SerializeField]
    Material glowMat;

    float emissionAmount;
    [SerializeField]
    float glowMulti;

    string[] reaperSpeech;

    ///ROCK PAPER SCISSORS///

    [SerializeField]
    bool isRPS;
    [SerializeField]
    int RPSNum;
    [SerializeField]
    bool RPSWon;
    [SerializeField]
    bool RPSLost;

    [SerializeField]
    GameObject RPSCam;

    [SerializeField]
    GameObject blunt;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        hasBlunt = true;
        glowMat.SetFloat("_EmissionMultiplier", 1000);
    }
    private void Update()
    {
        anim.SetBool("hasBlunt", hasBlunt);
        anim.SetInteger("Num", num);
        float y = Mathf.SmoothStep(-floatHeight, floatHeight, Mathf.PingPong(Time.time * floatSpeed, 1));
        transform.position = new Vector3(transform.position.x, transform.position.y + y, transform.position.z);
        emissionAmount = glowMat.GetFloat("_EmissionMultiplier");

        ///ROCK PAPER SCISSORS

        anim.SetBool("isRPS", isRPS);
        anim.SetInteger("RPSNum", RPSNum);
        anim.SetBool("RPSWon", RPSWon);
        anim.SetBool("RPSLost", RPSLost);

    }

    public void SetNum()
    {
        if(num == 0)
        {
            num = 1;
        }
        else
        {
            num = 0;
        }
    }

    public void TakeAHit()
    {
        anim.SetTrigger("hitBlunt");
    }

    public void CherryGlow()
    {
        StartCoroutine(MaterialGlow());
    }

    IEnumerator MaterialGlow()
    {
        float i = 0;
        while(emissionAmount < 100000)
        {
            glowMat.SetFloat("_EmissionMultiplier", i);
            i += glowMulti;
            yield return null;
        }
        float j = 0;
        while (emissionAmount > 1000)
        {
            glowMat.SetFloat("_EmissionMultiplier", i);
            i -= glowMulti;
            yield return null;
        }
        yield break;
    }

    ///RPS

    public void SetRPSNum()
    {
        RPSNum = Random.Range(0, 3);
    }

    public void RPSTrue()
    {
        isRPS = true;
        blunt.SetActive(false);
        RPSCam.SetActive(true);
    }

    public void RPSFalse()
    {
        isRPS = false;
        RPSCam.SetActive(false);
        blunt.SetActive(true);
    }

    public void StartRPS()
    {
        if(isRPS)
        {
            anim.SetTrigger("RPSStart");
        }
    }

    public void RPSRoundEnd()
    {
        if (isRPS)
        {
            anim.SetTrigger("RPSRoundEnd");
        }
    }
}
