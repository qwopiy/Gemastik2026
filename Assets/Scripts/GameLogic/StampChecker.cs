using System.Collections.Generic;
using UnityEngine;

public class StampChecker : MonoBehaviour
{
    //public List<GameObject> stampedObjs; // List of stamp GameObjects
    public Transform stampedObj;

    public void CheckStamps()
    {
        if (stampedObj == null) return;

        switch (stampedObj.GetComponent<StampsOnFood>().stampResult)
        {
            case StampResult.Approved:
                Debug.Log("Approved");
                break;
            case StampResult.Denied:
                Debug.Log("Denied");
                break;
            case StampResult.Mixed:
                Debug.Log("Mixed");
                break;
            default:
                Debug.Log("No stamp detected.");
                break;
        }
    }
}
