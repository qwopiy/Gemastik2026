using UnityEngine;

public class MagnifierController : MonoBehaviour
{
    public Camera zoomCamera;
    public GameObject magnifierCanvas;
    public Animator animator;
    private bool isMagnifierActive = false;
    public void ToggleMagnifier()
    {
        isMagnifierActive = !isMagnifierActive;
        animator.SetBool("Enabled", isMagnifierActive);
        zoomCamera.enabled = isMagnifierActive;
    }
}
