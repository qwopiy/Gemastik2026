using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class HoverDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator animator;
    private bool isHovered = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isHovered)
        {
            SetHoverState(true);
            //Debug.Log("Real Enter: Pointer entered the parent region!");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Check if the pointer is moving to an object inside this parent hierarchy
        GameObject nextObject = eventData.pointerCurrentRaycast.gameObject;

        if (nextObject != null && nextObject.transform.IsChildOf(this.transform))
        {
            // The pointer is hovering over the child button inside us.
            // Do NOT trigger the exit logic!
            return;
        }

        // If it's not a child, the pointer has legitimately left the entire boundary
        SetHoverState(false);
        //Debug.Log("Real Exit: Pointer left the entire parent region!");
    }

    public void SetHoverState(bool hover)
    {
        isHovered = hover;
        animator.SetBool("isHovered", hover);
    }
}