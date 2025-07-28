using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Inventory List", menuName = "Inventory List")]
public class InventoryItemList : ScriptableObject
{
    public ClothingScriptableObject[] clothingItemsSO;
    public ScriptableObject[] weaponItemsSO;//temp until so created
    public ScriptableObject[] healthItemsSO;//temp unitl so created
}
