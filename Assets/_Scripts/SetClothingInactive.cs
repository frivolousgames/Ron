using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteAlways]
public class SetClothingInactive : MonoBehaviour
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

    public static string chosenShoes;
    public static string chosenPants;
    public static string chosenShirt;
    public static string chosenGlasses;
    public static string chosenHat;
    public static string chosenMask;

}
