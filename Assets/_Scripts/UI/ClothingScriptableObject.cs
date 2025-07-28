using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Clothing Item", menuName = "Clothing Item")]
public class ClothingScriptableObject : InventoryItemScriptableObject
{
    [Tooltip("Bonus amount added to total armor in player health script")]
    public float itemArmorBonus;
    [Tooltip("Material(s) for item mesh")]
    public Material[] itemMats;
    [Tooltip("Whether or not the item has variations")]
    public bool hasVariations;
    [Tooltip("Variation Color(s) if any e.g. Red, Blue etc.")]
    public Color[] variationColors;
    [Tooltip("Variation Pic(s) if any")]
    public Sprite[] variationPics;
    [Tooltip("Variation Material(s) if any")]
    [SerializeField]
    public Material[] variationMats;
    [Tooltip("Whether or not the item has accessories")]
    public bool hasAccessories;
    [Tooltip("Accessory object(s) e. g. Belt, Wallet Chain etc.")]
    public GameObject[] accessoryObjects;
    [Tooltip("Whether or not the item is able to be tucked in (shirts only)")]
    public bool isTuckable;
}
