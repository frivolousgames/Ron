using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class InventoryThumb : MonoBehaviour
{
    //AddClothingTypes
    public static InventoryThumb Instance;
    public static InventoryThumb SelectedShoes;
    public static InventoryThumb SelectedPants;
    public static InventoryThumb SelectedShirt;
    public static InventoryThumb SelectedHat;
    public static InventoryThumb SelectedGlasses;
    public static InventoryThumb SelectedMask;
    public static InventoryThumb SelectedSocks;
    [HideInInspector]
    public int selectedIndex;

    [HideInInspector]
    public ClothingScriptableObject clothingItemSO;
    //public WeaponScriptableObject weaponItemSO;
    //public HealthScriptableObject healthItemSO;
    [HideInInspector]
    public bool isClothing;
    [HideInInspector]
    public bool isWeapon;
    [HideInInspector]
    public bool isHealth;

    [HideInInspector]
    public Image itemThumb;

    [SerializeField]
    Image outline;
    Color originalOutlineColor;

    Image bgImage;
    [SerializeField]
    Color clickedColor;
    Color originalBgColor;
    [SerializeField]
    Color selectedColor;

    public GameObject colorSwatchThumb;

    private void Awake()
    {
        bgImage = GetComponent<Image>();
        originalBgColor = bgImage.color;
        originalOutlineColor = outline.color;
        SetScriptableObjectType();
    }

    private void Start()
    {
        if (Instance == null)
        {
            OnPointerClick();
        }
    }
    private void OnEnable()
    {
        //itemThumb.sprite = itemThumbSprite;
    }

    private void Update() //sets bg image for thumb. If thumb SO is selected in InventoryFull it is "Selected" instance and bg color becomes selected color. If clicked it becomes "Instance" and clicked color. Add clothing types if creating more types.
    {
        //AddClothingTypes
        if (this == Instance)
        {
            if 
            (
            this != SelectedShoes ||
            this != SelectedPants ||
            this != SelectedShirt ||
            this != SelectedHat ||
            this != SelectedGlasses ||
            this != SelectedMask ||
            this != SelectedSocks
            )
            {
                bgImage.color = clickedColor;
            }
            else
            {
                bgImage.color = selectedColor;
            }
        }
        else if(this == SelectedShoes ||
                this == SelectedPants ||
                this == SelectedShirt ||
                this == SelectedHat ||
                this == SelectedGlasses ||
                this == SelectedMask ||
                this == SelectedSocks
               )
        {
            bgImage.color = selectedColor;
            //Debug.Log("Selected");
        }
        else
        {
            bgImage.color = originalBgColor;
        }
    }

    public void OnPointerEnter()
    {
        outline.color = Color.white;
    }
    public void OnPointerExit()
    {
        outline.color = originalOutlineColor;
    }

    public void OnPointerClick()
    {
        Instance = this;
        SetFullSizePicInfo();
    }

    void SetScriptableObjectType()
    {
        //if(!string.IsNullOrEmpty(clothingItemSO.name))//temp until other SOs are created
        //{
            isClothing = true;
        //}
        //Debug.Log("ClothingSO: " + clothingItemSO.name);
        //else if(weaponItemSo != null)
        //{
        //    isWeapon = true;
        //}
        //else
        //{
        //    isHealth = true;
        //}
    }
    void SetFullSizePicInfo()
    {
        //Debug.Log("Clicked");
        //Debug.Log("isClothing: " + isClothing);
        //Debug.Log("ClothingSO: " + clothingItemSO.name);
        GameObject fullSize = GameObject.FindGameObjectWithTag("Respawn");
        if (fullSize != null)
        {
            var fullSizeScript = fullSize.GetComponent<InventoryFull>();
            if (isClothing)
            {
                
                fullSizeScript.itemImage.sprite = clothingItemSO.itemImage;
                fullSizeScript.titleText.text = clothingItemSO.itemName;
                fullSizeScript.subtitleText.text = clothingItemSO.itemSubType;
                fullSizeScript.descriptionText.text = clothingItemSO.itemDescription;
                fullSizeScript.clothingSO = clothingItemSO;
                if (fullSizeScript.colorMenu.activeInHierarchy)
                {
                    fullSizeScript.colorMenu.SetActive(false);
                    int childCount = fullSizeScript.colorMenu.transform.childCount;
                    for(int i = childCount - 1; i >= 0; i--)
                    {
                        DestroyImmediate(fullSizeScript.colorMenu.transform.GetChild(i).gameObject);
                    }
                }
                if (clothingItemSO.hasVariations)
                {
                    int i = 0;
                    foreach(Color c in clothingItemSO.variationColors)
                    {
                        GameObject tempSwatch = Instantiate(fullSizeScript.colorSwatch, fullSizeScript.colorMenu.transform);
                        tempSwatch.GetComponent<Image>().color = c;
                        var tempSwatchScript = tempSwatch.GetComponent<ColorSwatchController>();
                        tempSwatchScript.itemSprite = clothingItemSO.variationPics[i];
                        tempSwatchScript.variationIndex = i;
                        i++;
                        //add other info like materials and sprites
                    }
                    fullSizeScript.colorMenu.SetActive(true);
                }
                else
                {
                    fullSizeScript.colorMenu.SetActive(false);
                }
            }
            else if(isWeapon)
            {

            }
            else
            {

            }
        }
        SetSelectedSO();
    }
    void SetSelectedSO()
    {
        if (InventoryFull.selectedClothingSO != clothingItemSO.name)
        {
            InventoryFull.variationIndex = 0;
            InventoryFull.selectedClothingSO = clothingItemSO.name;
        }
    }
}
