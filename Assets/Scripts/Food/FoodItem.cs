using UnityEngine;


public class FoodItem : MonoBehaviour
{
    public FoodDataSO foodData;

    public void SetFoodData(FoodDataSO data)
    {
        foodData = data;
    }

    public FoodDataSO GetFoodData()
    {
        return foodData;
    }
}
