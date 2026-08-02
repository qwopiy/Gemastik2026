using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableForm : DraggableObject, IBeginDragHandler
{
    [Header("Form References")]
    [SerializeField] private RectTransform baseForm;
    [SerializeField] private RectTransform compactedForm;
    public override void OnBeginDrag(PointerEventData eventData)
    {
        ApprovalResult approval = ApprovalResult.None;
        try 
        { 
            approval = GetComponent<StampsOnFood>().approvalResult;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }

        if (approval != ApprovalResult.None)
        {
            baseForm.gameObject.SetActive(false);
            compactedForm.gameObject.SetActive(true);

            if (!allowedTags.Contains("FoodItem"))
            {
                allowedTags.Add("FoodItem");
            }
        }

        base.OnBeginDrag(eventData);
    }

    public override void CheckDropTarget(GameObject droppedOn)
    {
        if (droppedOn.CompareTag("TrashBin"))
        {
            // Destroy the food item if dropped on the trash bin
            Destroy(gameObject);
            return;
        }
        base.CheckDropTarget(droppedOn);
    }
}
