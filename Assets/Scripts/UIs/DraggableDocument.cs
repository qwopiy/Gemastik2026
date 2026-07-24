using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class DraggableDocument : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Desk Boundaries (Optional)")]
    [SerializeField] private GameObjectAnchorSO deskArea; // Assign parent workspace bounds

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 dragOffset;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    // 1. Bring to front instantly when touched/clicked
    public void OnPointerDown(PointerEventData eventData)
    {
        DeskManager.Instance.BringToFront(rectTransform);
    }

    // 2. Calculate cursor offset so the paper doesn't snap its center to the mouse
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false; // Allows drop zones/underlying elements to detect pointer if needed

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out dragOffset
        );
    }

    // 3. Smooth dragging across different screen resolutions
    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;

        // Convert screen coordinates to canvas local coordinates
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint))
        {
            rectTransform.localPosition = localPoint - dragOffset;
            ClampToDeskArea();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }

    // Prevents papers from being dragged completely off-screen
    private void ClampToDeskArea()
    {
        if (deskArea.value == null) return;

        Vector3 pos = rectTransform.localPosition;
        RectTransform deskRect = deskArea.value.GetComponent<RectTransform>();
        Vector3 minBounds = deskRect.rect.min - rectTransform.rect.min;
        Vector3 maxBounds = deskRect.rect.max - rectTransform.rect.max;

        pos.x = Mathf.Clamp(pos.x, minBounds.x + deskRect.anchoredPosition.x, maxBounds.x + deskRect.anchoredPosition.x);
        pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);

        rectTransform.localPosition = pos;
    }
}