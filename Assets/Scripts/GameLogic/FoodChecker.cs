using System.Collections.Generic;
using UnityEngine;

public class FoodChecker : MonoBehaviour
{
    public List<GameObject> pendingObjects;
    public List<Transform> stampedObj;

    public void AddPendingObject(GameObject obj)
    {
        if (!pendingObjects.Contains(obj))
        {
            pendingObjects.Add(obj);
        }
    }
    public void CheckFood()
    {
        if (pendingObjects.Count != LevelManager.Instance.FoodDataList[LevelManager.Instance.index - 1].FoodData.Count)
        {
            Debug.LogWarning($"Not all food items have been stamped. Stamped: {pendingObjects.Count}, Expected: {LevelManager.Instance.FoodDataList[LevelManager.Instance.index - 1].FoodData.Count}");
            return;
        }

        stampedObj = new List<Transform>();
        for (int i = 0; i < pendingObjects.Count; i++)
        {
            stampedObj.Add(pendingObjects[i].transform);
        }

        if (stampedObj.Count == 0) return;
        

        foreach (var foodObj in stampedObj)
        {
            FoodItem foodInfo = foodObj.GetComponent<FoodItem>();
            
            CheckLevel1(foodObj, foodInfo);
            CheckLevel2(foodObj, foodInfo);
            CheckLevel3(foodObj, foodInfo);

            // TEMP
            Destroy(foodObj.gameObject);
        }

        // Should only run when all checks are done, and all food items have been stamped
        LevelManager.Instance.SpawnNextFood();
        ClearObjs();
    }

    public void ClearObjs()
    {
        pendingObjects.Clear();
        stampedObj.Clear();
    }

    private void CheckLevel1(Transform tr, FoodItem foodInfo)
    {
        if (tr == null || LevelManager.Instance.Level < 0)
            return;

        StampsOnFood foodStamps = tr.GetComponentInChildren<StampsOnFood>(true);

        foodStamps.CheckForGGLStickers();
        if (foodStamps.approvalResult == ApprovalResult.Approved)
        {
            ScoreManager.Instance.AddScore(1);
        }
        if (foodStamps.gglSticker == foodInfo.GetFoodData().GGLRating)
        {
            ScoreManager.Instance.AddScore(1);
        }
    }

    private void CheckLevel2(Transform tr, FoodItem foodInfo)
    {
        if (tr == null || LevelManager.Instance.Level < 1)
            return;

        TableResult tableResult = tr.GetComponentInChildren<TableResult>(true);
        if (tableResult.isKadaluarsa == foodInfo.GetFoodData().IsKadaluarsa)
        {
            ScoreManager.Instance.AddScore(1);
        }
        if (tableResult.isDefect == foodInfo.GetFoodData().IsDefect)
        {
            ScoreManager.Instance.AddScore(1);
        }
    }

    private void CheckLevel3(Transform tr, FoodItem foodInfo)
    {
        if (tr == null || LevelManager.Instance.Level < 2)
            return;

        ClaimsOnFood claimsOnFood = tr.GetComponentInChildren<ClaimsOnFood>(true);
        if (claimsOnFood.CompareClaims(foodInfo))
        {
            ScoreManager.Instance.AddScore(1);
        }
    }
}
