using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlcoholMeterController : MonoBehaviour
{
    [SerializeField]
    Slider slider;
    [SerializeField]
    GameObject head;

    private void Update()
    {
        ActivateHead();
    }

    void ActivateHead()
    {
        if(slider.value >= slider.maxValue)
        {
            head.SetActive(true);
        }
        else
        {
            head.SetActive(false);
        }
    }
}
