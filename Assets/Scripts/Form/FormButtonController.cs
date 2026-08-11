using UnityEngine;
using UnityEngine.UI;

public class FormButtonController : MonoBehaviour
{
    public GameObject formPrefab;
    public RectTransform formParent;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnFormButtonClick);
    }
    public void OnFormButtonClick()
    {
        if (formPrefab != null && formParent != null)
        {
            GameObject newForm = Instantiate(formPrefab, formParent);
        }
        else
        {
            Debug.LogWarning("Form prefab or parent is not assigned.");
        }
    }
}
