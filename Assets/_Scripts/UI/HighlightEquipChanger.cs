using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HighlightEquipChanger : MonoBehaviour
{
    [SerializeField]
    TMP_Text text;

    Image image;
    [SerializeField]
    Color equipColor;
    [SerializeField]
    Color unEquipColor;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Update()// Changed the text and color of the box when InventoryFull.alreadySelected (thumb is active item)
    {
        if (InventoryFull.alreadySelected)
        {
            text.text = "UNEQUIP";
            image.color = new Color(unEquipColor.r, unEquipColor.g, unEquipColor.b, image.color.r);
        }
        else
        {
            text.text = "EQUIP";
            image.color = new Color(equipColor.r, equipColor.g, equipColor.b, image.color.r);
        }
    }
}
