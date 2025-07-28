using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class InventoryFull : MonoBehaviour
{
    [HideInInspector]
    public ClothingScriptableObject clothingSO;
    public Image itemImage;
    public TMP_Text titleText;
    public TMP_Text subtitleText;
    public TMP_Text descriptionText;
    public GameObject colorMenu;
    public GameObject colorSwatch;
    [HideInInspector]
    public static int variationIndex;
    public static string selectedClothingSO;
    public static int selectedIndex;
    public static bool alreadySelected;

    [SerializeField]
    Animator equipAnim;
    public bool isActive;
    GameObject selectedObject;

    //blendshapes// AddClothingtypes 
    [SerializeField]
    string[] shoeBlendShapes;
    [SerializeField]
    string[] pantsBlendShapes;
    [SerializeField]
    string[] shirtsBlendShapes;
    [SerializeField]
    string[] glassesBlendShapes;
    [SerializeField]
    string[] hatsBlendShapes;
    [SerializeField]
    string[] masksBlendShapes;
    [SerializeField]
    string[] socksBlendShapes;
    //Add new mesh renderers for new blend shapes used by clothing objects.
    [SerializeField]
    SkinnedMeshRenderer bodyMR;
    [SerializeField]
    SkinnedMeshRenderer socksMR;
    [SerializeField]
    SkinnedMeshRenderer pantsMR;

    //Tuck
    [SerializeField]
    GameObject[] tuckedShirtObjects;
    //Pants
    public UnityEvent jeanCheck;//Temp
    public UnityEvent cargoCheck;//Temp

    string[] objectsNames = new string[7] //change if adding more types of clothing e.g hairstyles, gloves etc.
    {
        //AddClothingTypes
        "Shoes",
        "Pants",
        "Shirts",
        "Hats",
        "Glasses",
        "Masks",
        "Socks"
    };

    private void Update()
    {
        //IsThumbActive();

        CheckIfAlreadyEquipped();
        equipAnim.SetBool("isActive", isActive);

        //Debug.Log("alreadySelected: " + alreadySelected);
    }

    public void OnPointerEnter()
    {
        isActive = true;
    }

    public void OnPointerExit() 
    {  
        isActive = false; 
    }

    public void OnPointerClick()
    {
        //if object is clothing
        SetClothingItemActive();
        

        
    }

    void SetClothingItemActive()
    {
        if (!alreadySelected)
        {
            SetItemObjectActive(); //Temp set in OnClick event trigger. Change when adding weapons and health items
            SetItemObjectMaterials();
            //Thumb
            SetSelectedThumb(InventoryThumb.Instance.clothingItemSO.itemType);
            ////pants
            //jeanCheck.Invoke();//Temp
            //cargoCheck.Invoke();//Temp
        }
        else
        {
            SetItemObjectInactive();
            ResetObjectAndThumb(clothingSO.itemType);
        }

    }

    ///CLOTHING ITEM///
    
    //Set Active
    void SetItemObjectActive()
    {
        for (int i = 0; i < objectsNames.Length; i++)
        {
            if (objectsNames[i] == clothingSO.itemType)
            {
                foreach (GameObject objItem in ClothingObjectActivationController.objectList[i])
                {
                    if (objItem.name == clothingSO.itemObjectName)
                    {
                        selectedObject = objItem;
                        objItem.SetActive(true);
                        if(clothingSO.itemType == "Shirts")
                        {
                            var mr = objItem.GetComponent<SkinnedMeshRenderer>();
                            if (TuckedShirtButton.isTucked)
                            {
                                if (clothingSO.isTuckable)
                                {
                                    SetBlendShapes("Untucked", 0, mr);
                                    Debug.Log("Tucked");
                                }
                                else
                                {
                                    SetBlendShapes("Untucked", 100, mr);
                                    Debug.Log("UnTucked");
                                }
                            }
                            else
                            {
                                SetBlendShapes("Untucked", 100, mr);
                                Debug.Log("Tucked");
                            }
                        }
                        break;
                    }
                }
                break;
            }
        }
    }

    void SetItemObjectMaterials()
    {
        if (clothingSO.hasVariations)
        {
            selectedIndex = variationIndex;
            GetMaterials(clothingSO.variationMats, variationIndex);
        }
        else
        {
            selectedIndex = 0;
            GetMaterials(clothingSO.itemMats, 0);
        }
    }

    void SetSelectedThumb(string name) //AddClothingTypes
    {                                  //Sets the thumb to "selected" statis in InventoryThumb which turns it a different color until another item of the same type is selected
        switch (name)
        {
            case "Shoes":
                InventoryThumb.SelectedShoes = InventoryThumb.Instance;
                break;
            case "Pants":
                InventoryThumb.SelectedPants = InventoryThumb.Instance;
                break;
            case "Shirts":
                InventoryThumb.SelectedShirt = InventoryThumb.Instance;
                //Debug.Log("Shirt");
                break;
            case "Hats":
                InventoryThumb.SelectedHat = InventoryThumb.Instance;
                InventoryThumb.SelectedMask = null;
                break;
            case "Glasses":
                InventoryThumb.SelectedGlasses = InventoryThumb.Instance;
                InventoryThumb.SelectedMask = null;
                break;
            case "Masks":
                InventoryThumb.SelectedMask = InventoryThumb.Instance;
                InventoryThumb.SelectedHat = null;
                InventoryThumb.SelectedGlasses = null;
                break;
            case "Socks":
                InventoryThumb.SelectedSocks = InventoryThumb.Instance;
                break;
        }
        if (clothingSO.hasVariations)
        {
            InventoryThumb.Instance.selectedIndex = variationIndex;
        }
        else
        {
            InventoryThumb.Instance.selectedIndex = 0;
        }
    }

    void GetMaterials(Material[] mats, int index)
    {
        if(clothingSO.itemMats.Length == 0)
        {
            return;
        }
        else if (selectedObject.TryGetComponent<SkinnedMeshRenderer>(out var skinnedMR))
        {
            var mrMats = skinnedMR.sharedMaterials;
            int adjustedIndex = index * skinnedMR.materials.Length;
            for (int i = adjustedIndex, j = 0; i < skinnedMR.materials.Length + adjustedIndex; i++, j++)
            {
                mrMats[j] = mats[i];
                //Debug.Log("Material: " + mats[i].name);
            }
            skinnedMR.sharedMaterials = mrMats;
        }
        else
        {
            
            var mr = selectedObject.GetComponent<MeshRenderer>();
            var mrMats = mr.sharedMaterials;
            int adjustedIndex = index * mr.materials.Length;
            for (int i = adjustedIndex, j = 0; i < mr.materials.Length + adjustedIndex; i++, j++)
            {
                mrMats[j] = mats[i];
                //Debug.Log("Material: " + mats[i].name);
            }
            mr.sharedMaterials = mrMats;
        } 
    }

    //Set Inactive
    void SetItemObjectInactive()
    {
        if(clothingSO.itemType == "Socks")
        {
            return;
        }
        for (int i = 0; i < objectsNames.Length; i++)
        {
            if (objectsNames[i] == clothingSO.itemType)
            {
                foreach (GameObject objItem in ClothingObjectActivationController.objectList[i])
                {
                    objItem.SetActive(false);
                }
                break;
            }
        }
    }

    void ResetObjectAndThumb(string type)
    {
        switch (type)
        {
            case "Shoes":
                InventoryThumb.SelectedShoes = null;
                SetBlendShapes(shoeBlendShapes, 100, pantsMR);
                SetBlendShapes(socksBlendShapes, 0, socksMR);
                break;
            case "Pants":
                InventoryThumb.SelectedPants = null;
                SetBlendShapes(pantsBlendShapes, 0, bodyMR);
                break;
            case "Shirts":
                InventoryThumb.SelectedShirt = null;
                SetBlendShapes(shirtsBlendShapes, 0, bodyMR);
                break;
            case "Hats":
                InventoryThumb.SelectedHat = null;
                SetBlendShapes(hatsBlendShapes, 0, bodyMR);
                break;
            case "Glasses":
                InventoryThumb.SelectedGlasses = null;
                //Add if necessary
                break;
            case "Masks":
                InventoryThumb.SelectedMask = null;
                SetBlendShapes(masksBlendShapes, 0, bodyMR);
                break;
            case "Socks":
                break;
        }
    }

    void SetBlendShapes(string[] shapeNames, float amount, SkinnedMeshRenderer mr)
    {
        int currentBlendShape = 0;
        foreach (string shape in shapeNames)
        {
            if (mr.sharedMesh.GetBlendShapeIndex(shape) >= 0)
            {
                currentBlendShape = mr.sharedMesh.GetBlendShapeIndex(shape);
                mr.SetBlendShapeWeight(currentBlendShape, amount);
                //Debug.Log("Set BlendShape");
            }
            else
            {
                //Debug.Log("No Such BlendShape");
                continue;
            }
        }
    }

    void SetBlendShapes(string shapeName, float amount, SkinnedMeshRenderer mr)
    {
        int currentBlendShape = 0;
        if (mr.sharedMesh.GetBlendShapeIndex(shapeName) >= 0)
        {
            currentBlendShape = mr.sharedMesh.GetBlendShapeIndex(shapeName);
            mr.SetBlendShapeWeight(currentBlendShape, amount);
            //Debug.Log("Set BlendShape");
        }
    }

    //Check if already equipped
    void CheckIfAlreadyEquipped()
    {
        if (clothingSO.hasVariations)
        {
            if (IsThumbActive() && variationIndex == selectedIndex)
            {
                alreadySelected = true;
            }
            else
            {
                alreadySelected = false;
            }
        }
        else
        {
            if (IsThumbActive())
            {
                alreadySelected = true;
            }
            else
            {
                alreadySelected = false;
            }
        }
    }

    bool IsThumbActive()
    {
        if(InventoryThumb.Instance == InventoryThumb.SelectedShoes ||
           InventoryThumb.Instance == InventoryThumb.SelectedPants ||
           InventoryThumb.Instance == InventoryThumb.SelectedShirt ||
           InventoryThumb.Instance == InventoryThumb.SelectedHat ||
           InventoryThumb.Instance == InventoryThumb.SelectedGlasses ||
           InventoryThumb.Instance == InventoryThumb.SelectedMask ||
           InventoryThumb.Instance == InventoryThumb.SelectedSocks)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetObjectIsTucked()
    {
        if(InventoryThumb.SelectedShirt != null)
        {
            if (InventoryThumb.SelectedShirt.clothingItemSO.isTuckable)
            {
                SkinnedMeshRenderer mr = null;
                foreach (GameObject shirt in tuckedShirtObjects)
                {
                    if(shirt.name == InventoryThumb.SelectedShirt.clothingItemSO.itemObjectName)
                    {
                        mr = shirt.GetComponent<SkinnedMeshRenderer>();
                        Debug.Log("Shirt Object: " + shirt.name);
                    }
                }
                
                if(TuckedShirtButton.isTucked)
                {
                    { 
                        SetBlendShapes("Untucked", 0, mr);
                        Debug.Log("UnTucked");
                    }
                }
                else
                {
                    SetBlendShapes("Untucked", 100, mr);
                    Debug.Log("Tucked");
                }
            }
        }
    }
   
}
