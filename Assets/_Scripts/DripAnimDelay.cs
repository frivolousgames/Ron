using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DripAnimDelay : MonoBehaviour
{
    Animator anim;
    //[SerializeField]
    float animDelay;
    [SerializeField]
    bool isDripping;
    [SerializeField]
    int dripAmount;
    int tempDripAmount;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        
    }

    private void Update()
    {
        anim.SetBool("isDripping", isDripping);
        //Debug.Log("TempDrip: " + tempDripAmount);
    }
    public void StartDrip()
    {
        tempDripAmount = dripAmount;
        isDripping = true;
    }

    public void SetRandomAnim()
    {
        if (tempDripAmount < 0)
        {
            //Debug.Log("Stop Drip");
            isDripping = false;
        }
        else
        {
            tempDripAmount--;
            anim.speed = 0f;
            StartCoroutine(AnimDelay(animDelay));
        }
    }
    IEnumerator AnimDelay(float animDelay)
    {
        animDelay = Random.Range(0f, .5f);
        yield return new WaitForSeconds(animDelay);
        anim.speed = Random.Range(.5f, 1f);
        yield break;
    }

    
}
