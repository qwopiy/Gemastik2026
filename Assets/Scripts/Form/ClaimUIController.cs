using System.Security.Claims;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClaimUIController : MonoBehaviour
{
    public ClaimsOnFood claimsOnFood;
    public GameObject claimDropdownPrefab;
    public Button addClaimButton;
    public TMP_Dropdown.OptionDataList claimOptions;

    private void Start()
    {
        claimsOnFood = GetComponent<ClaimsOnFood>();
        addClaimButton.onClick.AddListener(AddClaim);

        InitializeOptions();
    }

    public void InitializeOptions()
    {
        claimOptions.options.Clear();
        //foreach (ClaimType claim in System.Enum.GetValues(typeof(ClaimType)))
        //{
        //    claimOptions.options.Add(new TMP_Dropdown.OptionData(claim.ToString()));
        //}

        for (int i = 1; i < 12; i++)
        {
            claimOptions.options.Add(new TMP_Dropdown.OptionData(GetClaimTypeStringInIndo(i)));
        }
    }

    public string GetClaimTypeStringInIndo(int index)
    {
        ClaimType claimType = (ClaimType)index;
        switch (claimType)
        {
            case ClaimType.CalorieFree:
                return "Bebas Kalori";
            case ClaimType.HighProtein:
                return "Tinggi Protein";
            case ClaimType.LowCarbohydrate:
                return "Rendah Karbohidrat";
            case ClaimType.SugarFree:
                return "Bebas Gula";
            case ClaimType.LowSugar:
                return "Rendah Gula";
            case ClaimType.LowSalt:
                return "Rendah Garam";
            case ClaimType.LowTotalFat:
                return "Rendah Lemak Total";
            case ClaimType.NutriLevel:
                return "NutriLevel";
            case ClaimType.Healthy:
                return "Sehat";
            case ClaimType.NoPreservative:
                return "Tanpa Pengawet";
            case ClaimType.Composition:
                return "Komposisi";
            default:
                return claimType.ToString();
        }
    }

    public void RemoveOption(ClaimType claimType)
    {
        claimOptions.options.RemoveAll(option => option.text == claimType.ToString());
    }

    public void AddClaim()
    {
        int index = addClaimButton.transform.GetSiblingIndex();

        GameObject newClaimDropdown = Instantiate(claimDropdownPrefab, addClaimButton.transform.parent);
        newClaimDropdown.transform.SetSiblingIndex(index - 1);
    }
}
