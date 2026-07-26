using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class DraggableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector3 initialPosition;
    private Vector2 dragOffset;
    private Vector2 minBounds;
    private Vector2 maxBounds;

    [Header("Settings")]
    public List<string> allowedTags;
    public List<GameObject> borders;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPosition = rectTransform.position;
        originalParent = transform.parent;

        transform.parent = canvas.transform; // Move to top-level canvas to avoid clipping issues

        // Block raycasts through this object so eventData.pointerCurrentRaycast 
        // can detect whatever is underneath the mouse (like the Food Item)
        canvasGroup.blocksRaycasts = false;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out dragOffset
        );

        // Bring to front while dragging
        DeskManager.Instance.BringToFront(rectTransform);

        // Calculate the bounds of the allowed drop area based on the borders
        GetBounds();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Follow the mouse position
        if (canvas == null) return;

        // Convert screen coordinates to canvas local coordinates
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint))
        {
            rectTransform.localPosition = localPoint - dragOffset;
        }

        // Clamp the draggable object within the defined borders
        ClampToBorders();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // Check what UI object is under the cursor when released
        GameObject droppedOn = eventData.pointerCurrentRaycast.gameObject;
        Debug.Log("Dropped on: " + (droppedOn != null ? droppedOn.name : "Nothing"));

        CheckDropTarget(droppedOn);
    }

    private void CheckDropTarget(GameObject droppedOn)
    {

        foreach (var tag in allowedTags)
        {
            if (droppedOn == null) break;
            if (droppedOn.CompareTag(tag))
            {
                // Snap to the obj's position
                Transform obj = droppedOn.GetComponentInParent<RectTransform>().transform;
                transform.SetParent(obj);
                return;
            }
        }
        
        // If dropped in an invalid area, snap back to the sticker dispenser/tray
        transform.SetParent(originalParent);
        rectTransform.position = initialPosition;
    }

    private void GetBounds()
    {
        if (borders != null && borders.Count > 0)
        {
            minBounds = new Vector2(float.MaxValue, float.MaxValue);
            maxBounds = new Vector2(float.MinValue, float.MinValue);
            foreach (var obj in borders)
            {
                if (obj.TryGetComponent<RectTransform>(out var borderRect))
                {
                    minBounds = Vector2.Min(minBounds, borderRect.rect.min);
                    maxBounds = Vector2.Max(maxBounds, borderRect.rect.max);
                }
            }
        }
    }

    private void ClampToBorders()
    {
        if (borders == null || borders.Count == 0) return;
        Vector3 pos = rectTransform.localPosition;
        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);
        rectTransform.localPosition = pos;
    }
}