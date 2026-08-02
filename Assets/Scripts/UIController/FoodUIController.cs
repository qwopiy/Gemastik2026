using UnityEngine;

public class FoodUIController : MonoBehaviour
{
    public RectTransform frontView;
    public RectTransform backView;

    public bool isFrontViewActive;

    private void Start()
    {
        ShowFrontView();
    }
    public void ToggleView()
    {
        isFrontViewActive = !isFrontViewActive;
        frontView.gameObject.SetActive(isFrontViewActive);
        backView.gameObject.SetActive(!isFrontViewActive);
    }

    public void ShowFrontView()
    {
        isFrontViewActive = true;
        frontView.gameObject.SetActive(true);
        backView.gameObject.SetActive(false);
    }
}
