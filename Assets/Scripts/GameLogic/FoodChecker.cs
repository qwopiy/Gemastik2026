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
        DialogueEventManager.Instance.TriggerFoodSubmitted();
        EndingManager.Instance.CalculateEnding();
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

        CompareFoodGGL(tr, foodInfo);
    }

    private void CheckLevel2(Transform tr, FoodItem foodInfo)
    {
        if (tr == null || LevelManager.Instance.Level < 1)
            return;

        CompareFoodTable(tr, foodInfo);
    }

    private void CheckLevel3(Transform tr, FoodItem foodInfo)
    {
        if (tr == null || LevelManager.Instance.Level < 2)
            return;

        CompareFoodClaims(tr, foodInfo);
    }

    private void CompareFoodGGL(Transform tr, FoodItem foodInfo)
    {
        StampsOnFood foodStamps = tr.GetComponentInChildren<StampsOnFood>(true);
        foodStamps.CheckForGGLStickers();

        if (foodStamps.gglSticker != foodInfo.GetFoodData().GGLRating)
        {
            foreach (var reason in foodInfo.GetFoodData().GGLReasons)
            {
                switch (reason)
                {
                    case GGLReason.Sugar:
                        EndingManager.Instance.AddMistake(MistakeType.Sugar);
                        break;
                    case GGLReason.Salt:
                        EndingManager.Instance.AddMistake(MistakeType.Salt);
                        break;
                    case GGLReason.Fat:
                        EndingManager.Instance.AddMistake(MistakeType.Fat);
                        break;
                }
            }
        } 
        else
        {
            EndingManager.Instance.AddCorrect();
        }
    }

    private void CompareFoodTable(Transform tr, FoodItem foodInfo)
    {
        TableResult tableResult = tr.GetComponentInChildren<TableResult>(true);

        bool isKadaluarsaCorrect = tableResult.isKadaluarsa == foodInfo.GetFoodData().IsKadaluarsa;
        bool isDefectCorrect = tableResult.isDefect == foodInfo.GetFoodData().IsDefect;

        if (isKadaluarsaCorrect && isDefectCorrect)
        {
            EndingManager.Instance.AddCorrect();
            return;
        }

        if (!isKadaluarsaCorrect)
        {
            EndingManager.Instance.AddMistake(MistakeType.Expired);
        }
        if (!isDefectCorrect)
        {
            EndingManager.Instance.AddMistake(MistakeType.Defect);
        }
    }

    private void CompareFoodClaims(Transform tr, FoodItem foodInfo)
    {
        ClaimsOnFood claimsOnFood = tr.GetComponentInChildren<ClaimsOnFood>(true);
        List<Claim> foodClaims = foodInfo.GetFoodData().Claims;

        bool allClaimsMatch = claimsOnFood.CompareClaims(foodInfo);
        if (allClaimsMatch) 
        {
            EndingManager.Instance.AddCorrect();
            return;
        }

        foreach (var claim in claimsOnFood.claims)
        {
            if (!claim.ContainsClaimInList(foodClaims))
            {
                switch (claim.claimType)
                {
                    case ClaimType.GGL:
                        EndingManager.Instance.AddMistake(MistakeType.WrongNutritionClaim);
                        break;
                    case ClaimType.Kadaluarsa:
                        EndingManager.Instance.AddMistake(MistakeType.WrongCompositionClaim);
                        break;
                    case ClaimType.Defect:
                        EndingManager.Instance.AddMistake(MistakeType.WrongCompositionClaim);
                        break;
                    case ClaimType.Halal:
                        EndingManager.Instance.AddMistake(MistakeType.WrongCompositionClaim);
                        break;
                }
            }
        }
    }
}
