using UnityEngine;

public class MagnifierController : MonoBehaviour
{
    private static readonly int EnabledHash = Animator.StringToHash("Enabled");
    public Camera zoomCamera;
    public GameObject magnifierCanvas;
    public GameObject RedStripe;
    public Animator animator;
    private bool isMagnifierActive = false;
    public void ToggleMagnifier()
    {
        isMagnifierActive = !isMagnifierActive;
        RedStripe.SetActive(isMagnifierActive);
        animator.SetBool(EnabledHash, isMagnifierActive);
        zoomCamera.enabled = isMagnifierActive;

        AudioManager.Instance.TriggerMagnifierSound();
    }
}
