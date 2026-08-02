using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public enum ObjectState
{
    ClientView,
    DeskView,
    ZoomView
}
[RequireComponent(typeof(CanvasGroup))]
public class DraggableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    protected RectTransform rectTransform;
    protected Canvas canvas;
    protected CanvasGroup canvasGroup;
    protected Transform originalParent;
    protected Vector3 initialPosition;
    protected Vector2 dragOffset;
    protected Vector2 minBounds;
    protected Vector2 maxBounds;
    protected ObjectState originalState;
    public ObjectState currentState;
    protected float thresholdY;
    protected float thresholdX;


    [Header("Desk Bounds & Storage")]
    [SerializeField] protected GameObjectAnchorSO deskArea; // ScriptableObject anchor containing desk RectTransform
    [SerializeField] protected GameObjectAnchorSO zoomArea; // ScriptableObject anchor containing zoom RectTransform
    [SerializeField] protected GameObjectAnchorSO clientArea; // ScriptableObject anchor containing client RectTransform
    [SerializeField] protected RectTransform ObjInClientView; // Client item visual
    [SerializeField] protected RectTransform ObjInDeskView; // Small/stowed visual

    [Header("Settings")]
    public List<string> allowedTags;
    public bool snapBack = true; // Whether to snap to the drop target or return to original position
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        initialPosition = rectTransform.position;
        originalParent = transform.parent;
        originalState = currentState;

        transform.SetParent(canvas.transform); // Keep world position when changing parent

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

        RectTransform deskRect = deskArea.value.GetComponent<RectTransform>();
        thresholdY = deskRect.rect.max.y + deskRect.anchoredPosition.y; // Default threshold is the top boundary of the allowed area
        thresholdX = deskRect.rect.max.x + deskRect.anchoredPosition.x; // Default threshold is the right boundary of the allowed area

        // Clamp the draggable object within the defined borders
        ClampToBorders();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // Check what UI object is under the center of dragged object when released
        GameObject droppedOn = eventData.pointerCurrentRaycast.gameObject;
        Debug.Log("Dropped on: " + (droppedOn != null ? droppedOn.name : "Nothing"));

        CheckDropTarget(droppedOn);
    }

    public virtual void CheckDropTarget(GameObject droppedOn)
    {
        if (droppedOn == null) return;

        foreach (var tag in allowedTags)
        {
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
        if (snapBack)
        {
            rectTransform.position = initialPosition;
            if (currentState != originalState)
            {
                SetVisualState(originalState); // Reset to original state when snapping back
            }
        }
    }

    private void GetBounds()
    {
        minBounds = new Vector2(float.MaxValue, float.MaxValue);
        maxBounds = new Vector2(float.MinValue, float.MinValue);
        if (canvas.TryGetComponent<RectTransform>(out var borderRect))
        {
            minBounds = Vector2.Min(minBounds, borderRect.rect.min);
            maxBounds = Vector2.Max(maxBounds, borderRect.rect.max);
        }
    }

    private void ClampToBorders()
    {
        if (canvas == null) return;
        Vector3 pos = rectTransform.localPosition;
        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);
        rectTransform.localPosition = pos;

        
        if (pos.y + dragOffset.y > thresholdY) // Client View
        {
            if (currentState != ObjectState.ClientView)
            {
                SetVisualState(ObjectState.ClientView);
            }
        }
        else if (pos.x + dragOffset.x > thresholdX) // Zoom View
        {
            if (currentState != ObjectState.ZoomView)
            {
                SetVisualState(ObjectState.ZoomView);
            }
        }
        else // Desk View
        {
            if (currentState != ObjectState.DeskView)
            {
                SetVisualState(ObjectState.DeskView);
            }
        }
    }

    protected void SetVisualState(ObjectState state)
    {
        ObjectState previousState = currentState;
        currentState = state;

        //if (CompactView != null) CompactView.gameObject.SetActive(state == ObjectState.Compact);
        //if (DeskView != null) DeskView.gameObject.SetActive(state == ObjectState.Desk);
        switch (currentState) 
        {
            case ObjectState.ClientView:
                if (ObjInClientView != null) ObjInClientView.gameObject.SetActive(true);
                if (ObjInDeskView != null) ObjInDeskView.gameObject.SetActive(false);

                
                break;
            case ObjectState.DeskView:
                if (ObjInClientView != null) ObjInClientView.gameObject.SetActive(false);
                if (ObjInDeskView != null) 
                { 
                    ObjInDeskView.gameObject.SetActive(true);
                    ObjInDeskView.transform.localScale = Vector3.one;
                }
                break;
            case ObjectState.ZoomView:
                if (ObjInClientView != null) ObjInClientView.gameObject.SetActive(false);
                if (ObjInDeskView != null)
                {
                    ObjInDeskView.gameObject.SetActive(true);
                    ObjInDeskView.transform.localScale = Vector3.one;
                }
                break;
        }
        CalculateDragOffset(previousState);
    }

    private void CalculateDragOffset(ObjectState prevState)
    {
        RectTransform prevObjRect;
        RectTransform currentObjRect;

        switch (prevState)
        {
            case ObjectState.ClientView:
                prevObjRect = ObjInClientView;
                break;
            case ObjectState.DeskView: 
                prevObjRect = ObjInDeskView;
                break; 
            case ObjectState.ZoomView:
                prevObjRect = ObjInDeskView;
                break;
            default:
                prevObjRect = ObjInClientView;
                break;
        }

        switch (currentState)
        {
            case ObjectState.ClientView:
                currentObjRect = ObjInClientView;
                break;
            case ObjectState.DeskView:
                currentObjRect = ObjInDeskView;
                break;
            case ObjectState.ZoomView:
                currentObjRect = ObjInDeskView;
                break;
            default: 
                currentObjRect = null;
                break;
        }

        Vector2 scaleDifference;
        scaleDifference = new Vector2(
            currentObjRect.rect.width / prevObjRect.rect.width,
            currentObjRect.rect.height / prevObjRect.rect.height
        );

        dragOffset *= scaleDifference;
    }
}