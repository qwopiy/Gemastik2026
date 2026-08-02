using System.Collections.Generic;
using UnityEngine;

public class StampChecker : MonoBehaviour
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
    public void CheckStamps()
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
            StampsOnFood foodStamps = foodObj.GetComponentInChildren<StampsOnFood>(true);

            foodStamps.CheckForGGLStickers();
            
            if (foodStamps.approvalResult == ApprovalResult.Approved)
            {
                ScoreManager.Instance.AddScore(1);
            }
            if (foodStamps.gglSticker == foodInfo.GetFoodData().GGLRating)
            {
                ScoreManager.Instance.AddScore(1);
            }

            // TEMP
            Destroy(foodObj.gameObject);

            //// Debug Test Stamp and GGL
            //switch (foodStamps.approvalResult)
            //{
            //    case ApprovalResult.Approved:
            //        Debug.Log("Approved");
            //        break;
            //    case ApprovalResult.Denied:
            //        Debug.Log("Denied");
            //        break;
            //    case ApprovalResult.Mixed:
            //        Debug.Log("Mixed");
            //        break;
            //    default:
            //        Debug.Log("No stamp detected.");
            //        break;
            //}
            //switch (foodStamps.gglStamp)
            //{
            //    case GGLSticker.A:
            //        Debug.Log("A");
            //        break;
            //    case GGLSticker.B:
            //        Debug.Log("B");
            //        break;
            //    case GGLSticker.C:
            //        Debug.Log("C");
            //        break;
            //    case GGLSticker.D:
            //        Debug.Log("D");
            //        break;
            //    case GGLSticker.Mixed:
            //        Debug.Log("Mixed");
            //        break;
            //    default:
            //        Debug.Log("No stamp detected.");
            //        break;
            //}
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
}
