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
        base.OnBeginDrag(eventData);

        // If this is the first drag, snap the object to the cursor position
        if (!hasBeenDragged)
        {
            compactedForm.gameObject.SetActive(false);
            baseForm.gameObject.SetActive(true);
            hasBeenDragged = true;

            Vector2 scaleDifference;
            scaleDifference = new Vector2(
                baseForm.rect.width / compactedForm.rect.width,
                baseForm.rect.height / compactedForm.rect.height
            );

            dragOffset *= scaleDifference;
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

            Vector2 scaleDifference;
            scaleDifference = new Vector2(
                compactedApprovedForm.rect.width / baseForm.rect.width,
                compactedApprovedForm.rect.height / baseForm.rect.height
            );

            dragOffset *= scaleDifference;
        }


    }
}
