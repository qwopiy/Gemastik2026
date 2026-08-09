using TMPro;
using UnityEngine;

public class CalendarOnTableController : MonoBehaviour
{
    public TextMeshProUGUI dateText;
    public string currentDate;

    private void Start()
    {
        currentDate = Calendar.Instance.GetDateString();
        dateText.text = currentDate;
    }
}
