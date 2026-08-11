using UnityEngine;
using UnityEngine.UI;

public class TableResult : MonoBehaviour
{
    public bool isKadaluarsa;
    public bool isDefect;

    [Header("Button References")]
    public Button kadaluarsaButtonY;
    public Button kadaluarsaButtonN;
    public Button defectButtonY;
    public Button defectButtonN;

    [Header("Checkmarks")]
    public GameObject kadaluarsaYCheckmark;
    public GameObject kadaluarsaNCheckmark;
    public GameObject defectYCheckmark;
    public GameObject defectNCheckmark;

    private void Start()
    {
        kadaluarsaButtonY.onClick.AddListener(() => SetKadaluarsa(true));
        kadaluarsaButtonN.onClick.AddListener(() => SetKadaluarsa(false));
        defectButtonY.onClick.AddListener(() => SetDefect(true));
        defectButtonN.onClick.AddListener(() => SetDefect(false));
    }
    public void SetKadaluarsa(bool value)
    {
        isKadaluarsa = value;
        kadaluarsaButtonY.gameObject.SetActive(false);
        kadaluarsaButtonN.gameObject.SetActive(false);
        kadaluarsaYCheckmark.SetActive(value);
        kadaluarsaNCheckmark.SetActive(!value);
    }

    public void SetDefect(bool value)
    {
        isDefect = value;
        defectButtonY.gameObject.SetActive(false);
        defectButtonN.gameObject.SetActive(false);
        defectYCheckmark.SetActive(value);
        defectNCheckmark.SetActive(!value);
    }

    //public bool CheckTableResult(int index)
    //{
    //    if (isKadaluarsa == LevelManager.Instance.FoodToSpawn[LevelManager.Instance.index - 1].IsKadaluarsa &&
    //        isDefect == LevelManager.Instance.FoodToSpawn[LevelManager.Instance.index - 1].IsDefect)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
}
