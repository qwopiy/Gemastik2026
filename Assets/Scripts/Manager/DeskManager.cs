using UnityEngine;

public class DeskManager : MonoBehaviour
{
    public static DeskManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Brings the specified RectTransform to the front of all sibling UI elements.
    /// </summary>
    public void BringToFront(RectTransform rectTransform)
    {
        rectTransform.SetSiblingIndex(3);
    }
}