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
        foreach (ClaimType claim in System.Enum.GetValues(typeof(ClaimType)))
        {
            claimOptions.options.Add(new TMP_Dropdown.OptionData(claim.ToString()));
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
