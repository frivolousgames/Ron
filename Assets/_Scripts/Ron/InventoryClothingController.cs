using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryClothingController : MonoBehaviour
{
    [SerializeField]
    SkinnedMeshRenderer mr;
    Mesh sharedMesh;

    GameObject selectedClothing;

    ///BlendShapes
    int tankTopShape;
    int shortSleevesShape;
    int longSleevesShape;
    int shortPantsShape;
    int longPantsShape;
    int hatShape;
    int visorShape;
    int bucketHatShape;

    ///Clothing Mesh
    [SerializeField]
    GameObject tankTop;
    [SerializeField]
    GameObject belt;
    [SerializeField]
    GameObject shirtCollar;
    [SerializeField]
    GameObject tShirtLarge;
    [SerializeField]
    GameObject tShirt2Sided;
    [SerializeField]
    GameObject longSleeve;
    [SerializeField]
    GameObject longSleeveCollar;
    [SerializeField]
    GameObject shortPants;
    [SerializeField]
    GameObject longPants;
    [SerializeField]
    GameObject sneakers;
    [SerializeField]
    GameObject flipFlops;
    [SerializeField]
    GameObject boots;
    [SerializeField]
    GameObject sandals;
    [SerializeField]
    GameObject baseballHat;
    [SerializeField]
    GameObject bucketHat;
    [SerializeField]
    GameObject visor;
    [SerializeField]
    GameObject gasMask;
    [SerializeField]
    GameObject aviatorGlasses;
    [SerializeField]
    GameObject rectGlasses;
    [SerializeField]
    GameObject neoGlasses;
    [SerializeField]
    GameObject terminatorGlasses;
    [SerializeField]
    GameObject razorGlasses;
    [SerializeField]
    GameObject roundGlasses;
    ///Clothing

    ///T-shirt Large


    private void Awake()
    {
        //sharedMesh = mr.sharedMesh;
        //tankTopShape = sharedMesh.GetBlendShapeIndex("Tank Shrink");
        //shortSleevesShape = sharedMesh.GetBlendShapeIndex("Short Sleeve Shrink");
        //longSleevesShape = sharedMesh.GetBlendShapeIndex("Long Sleeve Shrink");
        //shortSleevesShape = sharedMesh.GetBlendShapeIndex("Short Sleeve Shrink");
        //shortPantsShape = sharedMesh.GetBlendShapeIndex("Shorts Shrink");
        //longPantsShape = sharedMesh.GetBlendShapeIndex("Pants Shrink");
        //hatShape = sharedMesh.GetBlendShapeIndex("Hat Shrink");
        //visorShape = sharedMesh.GetBlendShapeIndex("Visor Shrink");
        //bucketHatShape = sharedMesh.GetBlendShapeIndex("Bucket Hat Shrink");
    }

    void SetClothingVariables(GameObject selectedClothing)
    {

    }
}