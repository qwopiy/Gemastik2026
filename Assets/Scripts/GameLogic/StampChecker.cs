using System.Collections.Generic;
using UnityEngine;

public class StampChecker : MonoBehaviour
{
    public List<Transform>stampedObj;

    public void CheckStamps()
    {
        stampedObj = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            stampedObj.Add(transform.GetChild(i));
        }

        if (stampedObj.Count == 0) return;
        if (stampedObj.Count != LevelManager.Instance.FoodDataList[LevelManager.Instance.index - 1].FoodData.Count)
        {
            Debug.LogWarning($"Not all food items have been stamped. Stamped: {stampedObj.Count}, Expected: {LevelManager.Instance.FoodDataList[LevelManager.Instance.index - 1].FoodData.Count}");
            return;
        }

        foreach (var foodObj in stampedObj)
        {
            FoodItem foodInfo = foodObj.GetComponent<FoodItem>();
            StampsOnFood foodStamps = foodObj.GetComponent<StampsOnFood>();

            foodStamps.CheckForGGLStickers();
            
            if (foodStamps.approvalResult == foodInfo.GetFoodData().Approval)
            {
                ScoreManager.Instance.AddScore(1);
            }
            if (foodStamps.gglSticker == foodInfo.GetFoodData().GGLRating)
            {
                ScoreManager.Instance.AddScore(1);
            }

            // TEMP
            Destroy(foodObj.gameObject);
            LevelManager.Instance.SpawnNextFood();

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
    }
}
