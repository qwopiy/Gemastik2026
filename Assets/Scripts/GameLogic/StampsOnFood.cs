using System.Collections;
using UnityEngine;
public class StampsOnFood : MonoBehaviour
{
    public StampResult stampResult = StampResult.None;

    public void SetStampResult(StampResult result)
    {
        if (stampResult == StampResult.None) stampResult = result;
        else stampResult = StampResult.Mixed;
    }
}