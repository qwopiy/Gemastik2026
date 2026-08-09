using UnityEngine;

[System.Serializable]
public struct Date
{
    public int day;
    public int month;
    public int year;
    public Date(int day, int month, int year)
    {
        this.day = day;
        this.month = month;
        this.year = year;
    }
}
public class Calendar : MonoBehaviour
{
    public static Calendar Instance;
    public Date date;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SetDate();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public string GetDateString()
    {
        return $"{date.day}/{date.month}/{date.year}";
    }

    public void SetDate()
    {
        date = new(LevelManager.Instance.Level + 1, 12, 2026);
    }

    public Date GetRandomDate(bool isExpired)
    {
        int randomDay;
        int randomMonth;
        int randomYear;

        if (isExpired) 
        {
            randomDay = Random.Range(1, 29);
            randomMonth = Random.Range(1, 13);
            randomYear = Random.Range(2020, 2026);
        }

        else
        {
            randomDay = Random.Range(1, 29);
            randomMonth = Random.Range(1, 13);
            randomYear = Random.Range(2026, 2031);
        }

        return new Date(randomDay, randomMonth, randomYear);
    }
}