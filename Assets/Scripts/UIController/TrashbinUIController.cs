using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class TrashbinUIController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static readonly int EnabledHash = Animator.StringToHash("Enabled");
    private static readonly int IsHoveredHash = Animator.StringToHash("isHovered");

    public GameObject redStripe;
    public Animator trashAnimator;
    public bool isHovered = false;
    public bool isMouseDown = false;

    private Animator animator;
    private bool isTrashActive = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isHovered)
            return;

        if (Mouse.current.leftButton.isPressed)
        {
            isMouseDown = true;
        } else
        {
            isMouseDown = false;
        }
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

        animator.SetBool(IsHoveredHash, hover);
    }

    public void ToggleTrash()
    {
        isTrashActive = !isTrashActive;
        redStripe.SetActive(isTrashActive);
        trashAnimator.SetBool(EnabledHash, isTrashActive);

        AudioManager.Instance.TriggerMagnifierSound();
    }
}
