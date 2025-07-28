using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryRonInfo : MonoBehaviour
{
    public ClothingScriptableObject[] clothingItemSOs;
    public GameObject[] itemObjects;

    Dictionary<GameObject, ClothingScriptableObject> clothingItems;


    void SetShirtBlendShape(int blendShapeWeight)
    {
        int i = 0;
        foreach(var so in clothingItemSOs)
        {
            if(so.itemType == "Shirt" && so.isTuckable)
            {
                var index = itemObjects[i].GetComponent<SkinnedMeshRenderer>().sharedMesh.GetBlendShapeIndex("Untucked");
                itemObjects[i].GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(index, blendShapeWeight);
                break;
            }
            i++;
        }
    }

    public void SetShirtIsTucked()
    {
        if(TuckedShirtButton.isTucked)
        {
            SetShirtBlendShape(100);
        }
        else
        {
            SetShirtBlendShape(0);
        }
    }
}
