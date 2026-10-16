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
        if (pendingObjects.Count != LevelManager.Instance.ClientDataList[LevelManager.Instance.randomIndex].AmountToSpawn)
        {
            Debug.LogWarning($"Not all food items have been stamped. Stamped: {pendingObjects.Count}, Expected: {LevelManager.Instance.ClientDataList[LevelManager.Instance.randomIndex].AmountToSpawn}");
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

            CheckFood(foodObj, foodInfo, out bool isCorrect);

            if (isCorrect)
            {
                EndingManager.Instance.AddCorrect();
            }
            else
            {
                EndingManager.Instance.AddMistake();
            }

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

    private void CheckFood(Transform tr, FoodItem foodInfo, out bool isCorrect)
    {
        bool isGGLCorrect = false;
        bool isTableCorrect = false;
        bool isClaimsCorrect = false;

        if (LevelManager.Instance.Level >= 0)
        {
            CompareFoodGGL(tr, foodInfo, out isGGLCorrect);
        }
        if (LevelManager.Instance.Level >= 1)
        {
            CompareFoodTable(tr, foodInfo, out isTableCorrect);
        }
        if (LevelManager.Instance.Level >= 2)
        {
            CompareFoodClaims(tr, foodInfo, out isClaimsCorrect);
        }

        isCorrect = LevelManager.Instance.Level switch 
        { 
            0 => isGGLCorrect,
            1 => isGGLCorrect && isTableCorrect,
            2 => isGGLCorrect && isTableCorrect && isClaimsCorrect,
            _ => isGGLCorrect && isTableCorrect && isClaimsCorrect
        };
    }

    private void CompareFoodGGL(Transform tr, FoodItem foodInfo, out bool isCorrect)
    {
        StampsOnFood foodStamps = tr.GetComponentInChildren<StampsOnFood>(true);
        foodStamps.CheckForGGLStickers();

        Debug.Log($"Food: {foodInfo.GetFoodData().FoodId}, GGL Sticker: {foodStamps.gglSticker}, Expected GGL: {foodInfo.GetFoodData().GGLRating}");

        if (foodStamps.gglSticker != foodInfo.GetFoodData().GGLRating)
        {
            isCorrect = false;
            foreach (var reason in foodInfo.GetFoodData().GGLReasons)
            {
                switch (reason)
                {
                    case GGLReason.Sugar:
                        EndingManager.Instance.AddMistakeType(MistakeType.Sugar);
                        break;
                    case GGLReason.Salt:
                        EndingManager.Instance.AddMistakeType(MistakeType.Salt);
                        break;
                    case GGLReason.Fat:
                        EndingManager.Instance.AddMistakeType(MistakeType.Fat);
                        break;
                }
            }
        } 
        else
        {
            isCorrect = true;
        }
    }

    private void CompareFoodTable(Transform tr, FoodItem foodInfo, out bool isCorrect)
    {
        TableResult tableResult = tr.GetComponentInChildren<TableResult>(true);

        bool isKadaluarsaCorrect = tableResult.isKadaluarsa == foodInfo.GetFoodData().IsExpired;
        bool isDefectCorrect = tableResult.isDefect == foodInfo.GetFoodData().IsDefect;

        if (isKadaluarsaCorrect && isDefectCorrect)
        {
            isCorrect = true;
            return;
        } else
        {
            isCorrect = false;
        }

        if (!isKadaluarsaCorrect)
        {
            EndingManager.Instance.AddMistakeType(MistakeType.Expired);
        }
        if (!isDefectCorrect)
        {
            EndingManager.Instance.AddMistakeType(MistakeType.Defect);
        }
    }

    private void CompareFoodClaims(Transform tr, FoodItem foodInfo, out bool isCorrect)
    {
        ClaimsOnFood claimsOnFood = tr.GetComponentInChildren<ClaimsOnFood>(true);
        List<Claim> foodClaims = foodInfo.GetFoodData().Claims;

        bool allClaimsMatch = claimsOnFood.CompareClaims(foodInfo);
        if (allClaimsMatch) 
        {
            isCorrect = true;
            return;
        } else
        {
            isCorrect = false;
        }

        foreach (var claim in claimsOnFood.claims)
        {
            if (!claim.ContainsClaimInList(foodClaims))
            {
                switch (claim.claimType)
                {
                    case ClaimType.CalorieFree:
                    case ClaimType.LowTotalFat:
                    case ClaimType.HighProtein:
                    case ClaimType.LowCarbohydrate:
                    case ClaimType.LowSugar:
                    case ClaimType.LowSalt:
                    case ClaimType.Healthy:
                    case ClaimType.NutriLevel:
                        EndingManager.Instance.AddMistakeType(MistakeType.WrongNutritionClaim);
                        break;
                    case ClaimType.NoPreservative:
                    case ClaimType.Composition:
                        EndingManager.Instance.AddMistakeType(MistakeType.WrongCompositionClaim);
                        break;
                }
            }
        }
    }
}
