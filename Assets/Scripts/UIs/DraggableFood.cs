using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class DraggableFood : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Desk Bounds & Storage")]
    [SerializeField] private GameObjectAnchorSO deskArea; // ScriptableObject anchor containing desk RectTransform
    [SerializeField] private RectTransform CompactView; // Primary item visual
    [SerializeField] private RectTransform DeskView; // Small/stowed visual

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 dragOffset;

    private bool isCompact = false;
    private float leftThresholdX;

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
        float minX = deskRect.rect.min.x + deskRect.anchoredPosition.x;
        float maxX = deskRect.rect.max.x - rectTransform.rect.max.x + deskRect.anchoredPosition.x;
        float minY = deskRect.rect.min.y - rectTransform.rect.min.y;
        float maxY = deskRect.rect.max.y - rectTransform.rect.max.y;

        leftThresholdX = minX;

        if (pos.x < leftThresholdX)
        {
            // Past left edge: Switch to compact visual, allow free drag past left bound
            if (!isCompact) SetVisualState(true);
        }
        else
        {
            // Inside desk area: Full visual state
            if (isCompact) SetVisualState(false);

            // Clamp Left boundary ONLY when on desk
            pos.x = Mathf.Max(pos.x, minX);
        }

        // 2. Clamp Right, Top, and Bottom strictly
        pos.x = Mathf.Min(pos.x, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        rectTransform.localPosition = pos;
    }

    private void SetVisualState(bool compact)
    {
        isCompact = compact;

        if (CompactView != null) CompactView.gameObject.SetActive(compact);
        if (DeskView != null) DeskView.gameObject.SetActive(!compact);
    }
}