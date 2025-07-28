using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteAlways]
public class ClothingObjectActivationController : MonoBehaviour
{
    [SerializeField]
    GameObject[] shoes;
    [SerializeField]
    GameObject[] pants;
    [SerializeField]
    GameObject[] shirts;
    [SerializeField]
    GameObject[] hats;
    [SerializeField]
    GameObject[] glasses;
    [SerializeField]
    GameObject[] masks;
    [SerializeField]
    GameObject[] socks;

    [SerializeField]
    SkinnedMeshRenderer bodyMR;
    [SerializeField]
    SkinnedMeshRenderer socksMR;
    [SerializeField]
    SkinnedMeshRenderer longPantsMR;
    //[SerializeField]
    //SkinnedMeshRenderer longShirtMR;
    //[SerializeField]
    //SkinnedMeshRenderer tshirtMR;

    public static List<GameObject[]> objectList =  new List<GameObject[]>();
    
    public static List<string> posBlendShapeNames = new List<string>();
    public static List<string> negBlendShapeNames = new List<string>();
    int currentBlendShape;

    public static string chosenShoes;
    public static string chosenPants;
    public static string chosenShirt;
    public static string chosenGlasses;
    public static string chosenHat;
    public static string chosenMask;

    public static string type;
    public static string item;


    private void Awake()
    { //change if adding more types of clothing e.g hairstyles, gloves etc.
        //AddClothingTypes
        objectList.Add(shoes);
        objectList.Add(pants);
        objectList.Add(shirts);
        objectList.Add(hats);
        objectList.Add(glasses);
        objectList.Add(masks);
        objectList.Add(socks);

    }

    public void ItemSelection() //Sets clothing items inactive when same type is enabled. Add "ClothingObjectActivator" script to any new items created and Link  event to this method.
    {
        switch (type)
        {
            case "Shoes":
                chosenShoes = item;
                SetInactive(shoes, chosenShoes);
                SetBlendShapes(posBlendShapeNames, 100, socksMR);
                SetBlendShapes(posBlendShapeNames, 100, longPantsMR);
                SetBlendShapes(negBlendShapeNames, 0, socksMR);
                SetBlendShapes(negBlendShapeNames, 0, longPantsMR);
                break;
            case "Pants":
                chosenPants = item;
                SetInactive(pants, chosenPants);;
                SetBlendShapes(posBlendShapeNames, 100, bodyMR);
                SetBlendShapes(negBlendShapeNames, 0, bodyMR);
                break;
            case "Shirt":
                chosenShirt = item;
                SetInactive(shirts, chosenShirt);
                SetBlendShapes(posBlendShapeNames, 100, bodyMR);
                SetBlendShapes(negBlendShapeNames, 0, bodyMR);
                break;
            case "Hat":
                chosenHat = item;
                SetInactive(hats, chosenHat);
                SetInactive(masks, chosenHat);
                SetBlendShapes(posBlendShapeNames, 100, bodyMR);
                SetBlendShapes(negBlendShapeNames, 0, bodyMR);
                break;
            case "Glasses":
                chosenGlasses = item;
                SetInactive(glasses, chosenGlasses);
                SetInactive(masks, chosenGlasses);
                break;
            case "Mask":
                chosenMask = item;
                SetInactive(masks, chosenMask);
                SetInactive(glasses, chosenMask);
                SetInactive(hats, chosenMask);
                SetBlendShapes(posBlendShapeNames, 100, bodyMR);
                SetBlendShapes(negBlendShapeNames, 0, bodyMR);
                break;
        }
    }
    void SetInactive(GameObject[] type, string chosenItem)
    {
        foreach (GameObject go in type)
        {
            if(go.name != chosenItem && go.activeInHierarchy)
            {
                //Debug.Log(go.name + " Deactivated");
                go.SetActive(false);
            }
        }
    }

    void SetBlendShapes(List<string> shapeNames, float amount, SkinnedMeshRenderer mr)
    {
        foreach(string shape in shapeNames)
        {
            if(mr.sharedMesh.GetBlendShapeIndex(shape) >= 0)
            {
                currentBlendShape = mr.sharedMesh.GetBlendShapeIndex(shape);
                mr.SetBlendShapeWeight(currentBlendShape, amount);
                //Debug.Log("Set BlendShape");
            }
            else
            {
                //Debug.Log("No Such BlendShape");
                continue;
            }
        }
    }

    void CheckCargoPants()
    {

    }
}
