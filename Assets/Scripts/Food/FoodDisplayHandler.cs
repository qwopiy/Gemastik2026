using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class FoodDisplayHandler : MonoBehaviour
{
    [Header("Food Item Reference")]
    private FoodItem foodItem;

    [Header("Item Data")]
    [SerializeField] private FoodDisplaySO itemData;

    [Header("Child Renderers")]
    [SerializeField] private Image baseImage;
    [SerializeField] private Image brandImage;
    [SerializeField] private Image conditionImage;
    [SerializeField] private TextMeshProUGUI FoodName;
    [SerializeField] private TextMeshProUGUI ExpiredDate;
    [SerializeField] private TextMeshProUGUI[] BrandClaim;

    [Header("List Item Data")]
    [SerializeField] private FoodDisplaySO[] itemDataList;
    [SerializeField] private FoodDisplaySO[] DefectedDataList;

    private static readonly int PrimaryColorID = Shader.PropertyToID("_PrimaryColor");
    private static readonly int SecondaryColorID = Shader.PropertyToID("_SecondaryColor");
    private static readonly int TertiaryColorID = Shader.PropertyToID("_TertiaryColor");
    private static readonly int MaskTexID = Shader.PropertyToID("_MaskTex");


    private void Start()
    {
        foodItem = GetComponentInParent<FoodItem>();

        if(foodItem == null || foodItem.foodData == null)
        {
            return;
        }

        if(foodItem.foodData.IsDefect)
        {
            itemDataList = DefectedDataList;
        }

        for(int i = 0; i< BrandClaim.Length; i++)
        {
            BrandClaim[i].text = "";
        }

        if (itemDataList != null && itemDataList.Length > 0)
        {
            int randomIndex = Random.Range(0, itemDataList.Length);
            itemData = itemDataList[randomIndex];
            ApplyItemData(itemData);
        }

    }

    public void ApplyItemData(FoodDisplaySO newItemData)
    {
        itemData = newItemData;

        if (itemData == null) return;

        baseImage.sprite = itemData.baseSprite;

        Material mat = baseImage.material;
        if (mat == null)
        {
            var renderMat = baseImage.materialForRendering;
            mat = renderMat != null ? Instantiate(renderMat) : new Material(Shader.Find("UI/Default"));
            baseImage.material = mat;
        }
        else
        {
            baseImage.material = Instantiate(mat);
            mat = baseImage.material;
        }

        mat.SetColor(PrimaryColorID, itemData.primaryColor);
        mat.SetColor(SecondaryColorID, itemData.secondaryColor);
        mat.SetColor(TertiaryColorID, itemData.tertiaryColor);

        if (itemData.maskSprite != null)
        {
            mat.SetTexture(MaskTexID, itemData.maskSprite.texture);
        }
        else
        {
            mat.SetTexture(MaskTexID, null);
        }

        if (brandImage != null)
        {
            brandImage.sprite = itemData.brandSprite;
            brandImage.gameObject.SetActive(itemData.brandSprite != null);
        }

        if (conditionImage != null)
        {
            conditionImage.sprite = itemData.conditionSprite;
            conditionImage.gameObject.SetActive(itemData.conditionSprite != null);
        }

        if (foodItem.foodData.FoodId != null)
        {
            FoodName.text = foodItem.foodData.FoodId;
        }

        if(ExpiredDate != null)
        {
            ExpiredDate.text = foodItem.foodData.ExpiryDate.GetDateString();
        }

        if(BrandClaim != null)
        {
            for(int i = 0; i < foodItem.foodData.Claims.Count; i++)
            {
                if(BrandClaim[i] != null)
                {
                    BrandClaim[i].text = foodItem.foodData.Claims[i].claimDescription;
                }
            }
        }
    }
}
