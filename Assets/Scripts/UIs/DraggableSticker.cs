using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class DraggableSticker : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector3 initialPosition;
    private Vector2 dragOffset;

    [Header("Settings")]
    public string foodTag = "FoodItem"; // Make sure your Food UI object has this tag or component

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
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // Check what UI object is under the cursor when released
        GameObject droppedOn = eventData.pointerCurrentRaycast.gameObject;
        Debug.Log("Dropped on: " + (droppedOn != null ? droppedOn.name : "Nothing"));

        if (droppedOn != null)
        {
            // Stick to the food item!
            if (droppedOn.CompareTag(foodTag))
            {
                // Snap to the food item's position
                Transform food = droppedOn.GetComponentInParent<DraggableFood>().transform;
                transform.SetParent(food);
            }
            else
            {
                Transform droppedOnParent = droppedOn.GetComponentInParent<RectTransform>().transform;
                transform.SetParent(droppedOnParent);
            }


            // Optional: Play a "slap/stick" sound effect here
        }
        else
        {
            // If dropped in an invalid area, snap back to the sticker dispenser/tray
            transform.SetParent(originalParent);
            rectTransform.position = initialPosition;
        }
    }
}