using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableFood : DraggableObject, IBeginDragHandler
{
    [Header("Food Settings")]
    public bool isApproved = false; // Whether the food has been approved
    public override void OnBeginDrag(PointerEventData eventData)
    {
        ApprovalResult approval = ApprovalResult.None;
        try
        {
            approval = GetComponentInChildren<StampsOnFood>().approvalResult;
        }
        catch (System.Exception e)
        {
            approval = ApprovalResult.None;
            Debug.LogError(e);
        }

        if (approval != ApprovalResult.None)
        {
            isApproved = true;
        }

        base.OnBeginDrag(eventData);
    }

    public override void CheckDropTarget(GameObject droppedOn)
    {
        if (droppedOn == null) return;

        if (droppedOn.CompareTag("TrashBin"))
        {
            // Destroy the food item if dropped on the trash bin
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
                return;
            }
        }

        if (droppedOn.CompareTag("ClientView") && isApproved)
        {
            StampChecker stampChecker = droppedOn.GetComponent<StampChecker>();
            stampChecker.AddPendingObject(gameObject);
            stampChecker.CheckStamps();
            gameObject.SetActive(false); // Hide the food item after stamping
            return;
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
}

