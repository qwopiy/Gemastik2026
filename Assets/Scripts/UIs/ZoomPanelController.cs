using UnityEngine;

public class ZoomPanelController : MonoBehaviour
{
    public void FlipFood()
    {
        if (transform.GetChild(0).TryGetComponent<FoodUIController>(out FoodUIController foodUI)) // skipped the first child because of Button
        {
            foodUI.ToggleView();
        }
    }
}
