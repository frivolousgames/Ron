using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItemScriptableObject : ScriptableObject
{
    [Tooltip("Whether the item has been unlocked/added to the active inventory")]
    public bool isActivated;
    [Tooltip("Name of item")]
    public string itemName;
    [Tooltip("Genre of item e.g. Clothing, Weapon, Food, etc.")]
    public string itemGenre;
    [Tooltip("Type of item e.g. Shorts, Shirt, Hat, etc.")]
    public string itemType;
    [Tooltip("Subtype of item e.g. Baseball, Cargo, Long Sleeve, etc.")]
    public string itemSubType;
    [Tooltip("Special group e.g. Budidas, Blue Jean, etc.")]
    public string itemCollection;
    [Tooltip("Short fun description of item")]
    [TextArea(3, 10)]
    public string itemDescription;
    [Tooltip("Bonus amount added to alcohol potency")]
    public float itemAlcoholBonus;
    [Tooltip("Bonus amount added to drug potency")]
    public float itemDrugsBonus;
    [Tooltip("Display pic of item")]
    public Sprite itemImage;
    [Tooltip("Gameobject name associated with item e.g. Baseball Hat, Boots, Jorts, etc.")]
    public string itemObjectName;
    [Tooltip("Tag of gameobject associated with item")]
    public string itemObjectTag;
}
