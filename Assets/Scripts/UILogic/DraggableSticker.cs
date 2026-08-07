using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class DraggableSticker : DraggableObject
{
    public override void CheckDropTarget(GameObject droppedOn)
    {
        if (droppedOn == null)
        {
            // If dropped in an invalid area, snap back to the sticker dispenser/tray
            transform.SetParent(originalParent);
            if (snapBack)
            {
                rectTransform.position = initialPosition;
            }
            return;
        }

        if (droppedOn.CompareTag("TrashBin"))
        {
            // Destroy the food item if dropped on the trash bin
            Destroy(gameObject);
            return;
        }

        foreach (var tag in allowedTags)
        {
            if (droppedOn == null) break;
            if (droppedOn.CompareTag(tag))
            {
                // Stick to the food item!
                if (droppedOn.TryGetComponent(out StampParent stampParent))
                {
                    // Snap to the food item's position
                    Transform food = stampParent.transform;
                    transform.SetParent(food);
                    return;
                }

                // Snap to the obj's position
                Transform obj = droppedOn.GetComponentInParent<RectTransform>().transform;
                transform.SetParent(obj);
                return;
            }
        }
    }
}