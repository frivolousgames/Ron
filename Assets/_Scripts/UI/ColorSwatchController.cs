using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwatchController : MonoBehaviour
{
    //public ClothingScriptableObject clothingSO;
    [HideInInspector]
    public Sprite itemSprite;
    [HideInInspector]
    public int variationIndex;
    GameObject fullSizeItem;

    private void Awake()
    {
        fullSizeItem = GameObject.FindGameObjectWithTag("Respawn");
    }
    private void OnEnable()
    {
        //Debug.Log(itemSprite.name + " " + variationIndex);
    }
    private void OnDisable()
    {
        GameObject.Destroy(gameObject);
    }

    public void OnClick()
    {
        if (itemSprite != null)
        {
            var fullSizeItemScript = fullSizeItem.GetComponent<InventoryFull>();
            if (fullSizeItemScript != null)
            {
                fullSizeItemScript.itemImage.sprite = itemSprite;
                InventoryFull.variationIndex = variationIndex;
            }
        }
    }
}
