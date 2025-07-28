using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[ExecuteAlways]
public class ClothingObjectActivator : MonoBehaviour
{
    [SerializeField]
    UnityEvent activate;

    [SerializeField]
    string[] posBlendShapeNames;
    [SerializeField]
    string[] negBlendShapeNames;

    private void OnEnable()
    {
        StartCoroutine(StartDelay());
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(.02f);
        SetClothingVariables();
        yield break;
    }

    void SetClothingVariables()//Sets this clothing item variables for "ClothingObjectActivationController" on enable.
    {
        ClothingObjectActivationController.type = gameObject.tag;
        ClothingObjectActivationController.item = gameObject.name;
        ClothingObjectActivationController.posBlendShapeNames.Clear();
        ClothingObjectActivationController.negBlendShapeNames.Clear();
        AddPosBlendShapeNames();
        AddNegBlendShapeNames();
        //Debug.Log(ClothingObjectActivationController.type);
        //Debug.Log(ClothingObjectActivationController.item);
        activate.Invoke();
    }

    void AddPosBlendShapeNames()//Adds items positive(100) blendshape name(s) to "ClothingObjectActivationController" list to be set on enable.
    {
        if(posBlendShapeNames.Length > 0)
        {
            foreach (var name in posBlendShapeNames)
            {
                ClothingObjectActivationController.posBlendShapeNames.Add(name);
            }
        }
    }

    void AddNegBlendShapeNames()//Adds items negative(0) blendshape name(s) to "ClothingObjectActivationController" list to be set on enable.
    {
        if (negBlendShapeNames.Length > 0)
        {
            foreach (var name in negBlendShapeNames)
            {
                ClothingObjectActivationController.negBlendShapeNames.Add(name);
            }
        }
    }
}
