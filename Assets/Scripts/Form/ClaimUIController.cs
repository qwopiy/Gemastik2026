using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClaimUIController : MonoBehaviour
{
    public ClaimsOnFood claimsOnFood;
    public GameObject claimDropdownPrefab;
    public Button addClaimButton;
    public TMP_Dropdown.OptionDataList claimOptions;

    private int claimCount = 0;
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

        for (int i = 0; i < 11; i++)
        {
            claimOptions.options.Add(new TMP_Dropdown.OptionData(GetClaimTypeStringInIndo(i)));
        }
    }

    public string GetClaimTypeStringInIndo(int index)
    {
        ClaimType claimType = (ClaimType)index;
        return claimType switch
        {
            ClaimType.CalorieFree => "Bebas Kalori",
            ClaimType.HighProtein => "Tinggi Protein",
            ClaimType.LowCarbohydrate => "Rendah Karbohidrat",
            ClaimType.LowSugar => "Rendah Gula",
            ClaimType.LowSalt => "Rendah Garam",
            ClaimType.LowTotalFat => "Rendah Lemak Total",
            ClaimType.NutriLevel => "NutriLevel",
            ClaimType.Healthy => "Sehat",
            ClaimType.NoPreservative => "Tanpa Pengawet",
            ClaimType.Composition => "Komposisi",
            _ => claimType.ToString(),
        };
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
        claimCount++;
        if (claimCount >= 3)
        {
            addClaimButton.gameObject.SetActive(false);
        }
    }
}
