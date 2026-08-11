using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableForm : DraggableObject, IBeginDragHandler
{
    [Header("Form References")]
    [SerializeField] private RectTransform baseForm;
    [SerializeField] private RectTransform compactedForm;
    [SerializeField] private RectTransform compactedApprovedForm;
    [SerializeField] private bool hasBeenDragged = false;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (!hasBeenDragged) 
        { 
            compactedForm.gameObject.SetActive(false);
            baseForm.gameObject.SetActive(true);
            hasBeenDragged = true;
        }
        ApprovalResult approval = ApprovalResult.None;
        try 
        { 
            approval = GetComponent<StampsOnFood>().approvalResult;
        }
        catch (System.Exception)
        {
            Debug.Log("No approval found.");
        }

        if (approval != ApprovalResult.None)
        {
            baseForm.gameObject.SetActive(false);
            compactedApprovedForm.gameObject.SetActive(true);

            if (!allowedTags.Contains("FoodItem"))
            {
                allowedTags.Add("FoodItem");
            }
        }

        base.OnBeginDrag(eventData);
    }
}
