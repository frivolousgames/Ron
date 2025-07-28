using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TuckedShirtButton : MonoBehaviour
{
    public static bool isTucked; //Save this in serialization system so tucked/untucked shirt is saved
    Image buttonBG;
    [SerializeField]
    Color untuckedColor;
    [SerializeField]
    Color tuckedColor;
    [SerializeField]
    Text tuckedText;
    [SerializeField]
    Color textColorTucked;
    [SerializeField]
    Color textColorUnTucked;

    private void Awake()
    {
        //isTucked = serialized system.isTucked
        buttonBG = GetComponent<Image>();
    }

    public void Tuck()
    {
        if(isTucked)
        {
            isTucked = false;
            buttonBG.color = untuckedColor;
            tuckedText.color = textColorUnTucked;
        }
        else
        {
            isTucked = true;
            buttonBG.color = tuckedColor;
            tuckedText.color = textColorTucked;
        }
    }
}
