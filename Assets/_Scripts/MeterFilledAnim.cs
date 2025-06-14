using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeterFilledAnim : MonoBehaviour
{
    [SerializeField]
    Slider slider;
    Animator anim;
    public bool isFilled;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("isFilled", isFilled);
        SetFilled();
    }

    void SetFilled()
    {
        if(slider.value >= slider.maxValue)
        {
            isFilled = true;
        }
        else
        {
            isFilled = false;
        }
    }
}
