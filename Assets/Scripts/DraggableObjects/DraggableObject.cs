using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public enum ObjectState
{
    CompactView,
    DeskView,
    ZoomView
}
[RequireComponent(typeof(CanvasGroup))]
public class DraggableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Debug Vars")]
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
    public GameObject ObjCurrentlyOn; // The object currently being dragged over
    protected float maxDistance = 10f;
    protected float snapduration = 0.2f; // Duration of the snap-back animation

    [Header("References")]
    [SerializeField] protected RectTransform ObjInClientView; // Client item visual
    [SerializeField] protected RectTransform ObjInDeskView; // Small/stowed visual

    [Header("Settings")]
    public List<string> allowedTags;
    public bool snapBack = true; // Whether to snap to the drop target or return to original position
    public bool isTrashable = false; // Whether the object can be trashed
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
        //canvasGroup.blocksRaycasts = false;

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

        AudioManager.Instance.TriggerDrag();
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

        // Check for objects behind the draggable object
        GetObjectBehind(eventData);
        SetObjectState();
        // Clamp the draggable object within the defined borders
        ClampToBorders();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        Debug.Log("Dropped on: " + (ObjCurrentlyOn != null ? ObjCurrentlyOn.name : "Nothing"));

        CheckDropTarget(ObjCurrentlyOn);

        AudioManager.Instance.TriggerDrop();
    }

    public virtual void CheckDropTarget(GameObject droppedOn)
    {
        if (droppedOn == null)
        {
            // If dropped in an invalid area, snap back to the sticker dispenser/tray
            transform.SetParent(originalParent);
            if (snapBack)
            {
                //rectTransform.position = initialPosition;
                StartCoroutine(MoveRoutine(initialPosition, snapduration));
            }
            return;
        }

        if (isTrashable && droppedOn.CompareTag("TrashBin"))
        {
            // Destroy the food item if dropped on the trash bin
            AudioManager.Instance.TriggerTrashbin();
            Destroy(gameObject);
            return;
        }

        foreach (var tag in allowedTags)
        {
            if (droppedOn.CompareTag(tag))
            {
                // Snap to the obj's position
                Transform obj = droppedOn.GetComponentInParent<RectTransform>().transform;
                transform.SetParent(obj);
                return; // Exit after snapping to the first valid object
            }
            else
            {
                // If dropped in an invalid area, snap back to the sticker dispenser/tray
                transform.SetParent(originalParent);
                if (snapBack)
                {
                    //rectTransform.position = initialPosition;
                    StartCoroutine(MoveRoutine(initialPosition, snapduration));
                    if (currentState != originalState)
                    {
                        SetVisualState(originalState); // Reset to original state when snapping back
                    }
                }
                return;
            }
        }
    }

    protected IEnumerator MoveRoutine(Vector3 target, float duration)
    {
        Vector3 startPosition = rectTransform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Calculates the percentage of completion (0.0 to 1.0)
            float percentageComplete = elapsedTime / duration;

            // Smoothly interpolates between start and target
            rectTransform.position = Vector3.Lerp(startPosition, target, percentageComplete);

            // Waits until the next frame
            yield return null;
        }

        // Ensures the object snaps precisely to the final position
        rectTransform.position = target;
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

    private void GetObjectBehind(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        ObjCurrentlyOn = null;

        // Skip the first result (which is this draggable object) and look for allowed targets
        for (int i = 1; i < results.Count; i++)
        {
            GameObject hitObject = results[i].gameObject;
            Debug.Log("Hit UI Object: " + hitObject.name);

            foreach (string tag in allowedTags)
            {
                if (hitObject.CompareTag(tag))
                {
                    ObjCurrentlyOn = hitObject;
                    Debug.Log("Found object behind: " + hitObject.name);
                    return; // Exit after finding the first valid object
                }
            }
        }

        Debug.Log("Currently on: " + (ObjCurrentlyOn != null ? ObjCurrentlyOn.name : "Nothing"));
    }

    private void ClampToBorders()
    {
        if (canvas == null) return;
        Vector3 pos = rectTransform.localPosition;
        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);
        rectTransform.localPosition = pos;
    }

    public void SetObjectState()
    {
        if (ObjCurrentlyOn == null) 
        {
            SetVisualState(currentState);
            return;
        }

        switch (ObjCurrentlyOn.tag)
        {
            case "CompactView":
                SetVisualState(ObjectState.CompactView);
                break;
            case "DeskView":
                SetVisualState(ObjectState.DeskView);
                break;
            case "ZoomView":
                SetVisualState(ObjectState.ZoomView);
                break;
            default:
                Debug.LogWarning("Unknown tag: " + ObjCurrentlyOn.tag);
                break;
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
            case ObjectState.CompactView:
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
        //CalculateDragOffset(previousState);
    }

    //private void CalculateDragOffset(ObjectState prevState)
    //{
    //    if (ObjInClientView == null || ObjInDeskView == null) return;

    //    RectTransform prevObjRect;
    //    RectTransform currentObjRect;

    //    switch (prevState)
    //    {
    //        case ObjectState.CompactView:
    //            prevObjRect = ObjInClientView;
    //            break;
    //        case ObjectState.DeskView: 
    //            prevObjRect = ObjInDeskView;
    //            break; 
    //        case ObjectState.ZoomView:
    //            prevObjRect = ObjInDeskView;
    //            break;
    //        default:
    //            prevObjRect = ObjInClientView;
    //            break;
    //    }

    //    switch (currentState)
    //    {
    //        case ObjectState.CompactView:
    //            currentObjRect = ObjInClientView;
    //            break;
    //        case ObjectState.DeskView:
    //            currentObjRect = ObjInDeskView;
    //            break;
    //        case ObjectState.ZoomView:
    //            currentObjRect = ObjInDeskView;
    //            break;
    //        default: 
    //            currentObjRect = null;
    //            break;
    //    }

    //    Vector2 scaleDifference;
    //    scaleDifference = new Vector2(
    //        currentObjRect.rect.width / prevObjRect.rect.width,
    //        currentObjRect.rect.height / prevObjRect.rect.height
    //    );

    //    dragOffset *= scaleDifference;
    //}
}