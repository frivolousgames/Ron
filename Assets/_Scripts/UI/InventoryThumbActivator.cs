using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryThumbActivator : MonoBehaviour
{
    [SerializeField]
    GameObject thumbPrefab;

    public InventoryItemList inventoryItemSO;
    ClothingScriptableObject[] clothingItemSO;
    ScriptableObject[] weaponItemSO;
    ScriptableObject[] healthItemSO;

    private void Awake()
    {
        clothingItemSO = inventoryItemSO.clothingItemsSO;
        //weaponItemSO = inventoryItemSO.weaponItemsSO;
        //healthItemSO = inventoryItemSO.healthItemsSO;

        PopulateClothingInventoryThumbs(clothingItemSO);
    }

    void PopulateClothingInventoryThumbs(ClothingScriptableObject[] so)
    {
        for(int i = 0; i < so.Length; i++)
        {
            if (so[i].isActivated)
            {
                var temp = Instantiate(thumbPrefab, transform);
                var inventoryThumbTemp = temp.GetComponent<InventoryThumb>();
                inventoryThumbTemp.clothingItemSO = so[i];
                inventoryThumbTemp.itemThumb.sprite = so[i].itemImage;
                if (clothingItemSO[i].hasVariations)
                {
                    inventoryThumbTemp.colorSwatchThumb.SetActive(true);
                }
                else
                {
                    inventoryThumbTemp.colorSwatchThumb.SetActive(false);
                }
                //Debug.Log(so[i].itemName);
            }
        }
    }
}
