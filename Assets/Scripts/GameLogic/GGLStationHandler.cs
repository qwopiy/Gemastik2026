using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGLStationHandler : MonoBehaviour
{
    private static readonly WaitForSeconds _waitForSeconds0_2 = new(0.2f);
    public List<Transform> GGLStampsParents;
    public List<GameObject> GGLObjects;

    private void Start()
    {
        for (int i = 0; i < GGLObjects.Count; i++)
        {
            StartCoroutine(CheckCoroutine(i));
        }
    }

    private void CheckStampAvailability(int index)
    {
        if (GGLStampsParents[index].childCount <= 1)
        {
            Instantiate(GGLObjects[index], GGLStampsParents[index]);
        }
    }

    private IEnumerator CheckCoroutine(int index)
    {
        while (true) 
        {
            yield return _waitForSeconds0_2;
            CheckStampAvailability(index);
        }
    }
}
