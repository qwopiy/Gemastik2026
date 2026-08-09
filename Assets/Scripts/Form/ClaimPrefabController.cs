using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClaimPrefabController : MonoBehaviour
{
    public TextMeshProUGUI claimText;
    public TMP_Dropdown dropdown;
    public GameObject TCheck;
    public Button TButton;
    public GameObject FCheck;
    public Button FButton;

    private ClaimType selectedClaim;

    private ClaimUIController claimUIController;
    private ClaimsOnFood claimsOnFood;
    private void Start()
    {
        claimUIController = GetComponentInParent<ClaimUIController>();
        claimsOnFood = claimUIController.claimsOnFood;

        InitializeDropdown();
        InitializeButtons();
    }

    public void InitializeDropdown()
    {
        dropdown.onValueChanged.AddListener(ConfirmClaim);
        dropdown.ClearOptions();
        dropdown.AddOptions(claimUIController.claimOptions.options);
    }

    public void InitializeButtons()
    {
        TButton.onClick.AddListener(() => ConfirmValidity(true));
        FButton.onClick.AddListener(() => ConfirmValidity(false));
    }

    public void ConfirmValidity(bool isValid)
    {
        TButton.gameObject.SetActive(false);
        TCheck.SetActive(isValid);
        FButton.gameObject.SetActive(false);
        FCheck.SetActive(!isValid);

        claimsOnFood.AddClaim(selectedClaim, isValid);
    }

    public void ConfirmClaim(int value)
    {
        selectedClaim = (ClaimType)value;
        Debug.Log("Selected Claim: " + selectedClaim);

        // Remove the selected option from the dropdown options in ClaimUIController
        claimText.text = "Selected Claim: " + selectedClaim.ToString();
        //claimUIController.RemoveOption(selectedClaim);

        // Destroy this prefab after confirming the choice
        dropdown.gameObject.SetActive(false);
    }
}
