using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormStationHandler : MonoBehaviour
{
    private static readonly WaitForSeconds _waitForSeconds0_2 = new(0.2f);
    public Transform FormContainer;
    public GameObject formPrefab;

    private void Start()
    {
        StartCoroutine(CheckCoroutine());
    }

    private void CheckStampAvailability()
    {
        if (FormContainer.childCount <= 1)
        {
            Instantiate(formPrefab, FormContainer.position, Quaternion.identity, FormContainer);
        }
    }

    private IEnumerator CheckCoroutine()
    {
        while (true)
        {
            yield return _waitForSeconds0_2;
            CheckStampAvailability();
        }
    }
}
