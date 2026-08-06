using UnityEngine;

public class ZoomPanelController : MonoBehaviour
{
    public void FlipFood()
    {
        if (transform.GetChild(0).TryGetComponent<FoodUIController>(out FoodUIController foodUI))
        {
            foodUI.ToggleView();
        }
    }
}
