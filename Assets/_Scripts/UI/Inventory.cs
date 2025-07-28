using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    List<ClothingScriptableObject> clothingInventoryItems;
    [SerializeField]
    List<ScriptableObject> weaponInventoryItems;
    [SerializeField]
    List<ScriptableObject> energyInventoryItems;

    private void Awake()
    {
        //Add items from storage when the system is set up
        clothingInventoryItems = new List<ClothingScriptableObject>();
        weaponInventoryItems = new List<ScriptableObject>();
        energyInventoryItems = new List<ScriptableObject>();

    }
}
