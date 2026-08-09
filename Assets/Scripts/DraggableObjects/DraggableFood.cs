using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(FoodUIController))]
public class DraggableFood : DraggableObject, IBeginDragHandler, IPointerClickHandler
{
    private bool _isDragging;
    [Header("Food Settings")]
    public bool isApproved = false; // Whether the food has been approved
    public override void OnBeginDrag(PointerEventData eventData)
    {
        ApprovalResult approval = ApprovalResult.None;
        isApproved = false; // so user wont exploit using same form for different foods
        try
        {
            approval = GetComponentInChildren<StampsOnFood>(true).approvalResult;
        }
        catch (System.Exception)
        {
            approval = ApprovalResult.None;
            Debug.Log("No approval found.");
        }

        if (approval != ApprovalResult.None)
        {
            isApproved = true;
        }

        _isDragging = true;

        base.OnBeginDrag(eventData);
    }

    public override void CheckDropTarget(GameObject droppedOn)
    {
        _isDragging = false;
        if (droppedOn == null)
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
        }

        if (droppedOn.CompareTag("CompactView") && isApproved)
        {
            FoodChecker stampChecker = droppedOn.GetComponent<FoodChecker>();
            stampChecker.AddPendingObject(gameObject);
            stampChecker.CheckFood();
            gameObject.SetActive(false); // Hide the food item after stamping
            return;
        }

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
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && _isDragging) 
        { 
            GetComponent<FoodUIController>().ToggleView();
        }
    }
}

