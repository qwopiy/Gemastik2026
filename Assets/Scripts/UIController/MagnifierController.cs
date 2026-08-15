using UnityEngine;

public class MagnifierController : MonoBehaviour
{
    private static readonly int EnabledHash = Animator.StringToHash("Enabled");
    public Camera zoomCamera;
    public GameObject magnifierCanvas;
    public GameObject redStripe;
    public Animator animator;
    private bool isMagnifierActive = false;
    public void ToggleMagnifier()
    {
        isMagnifierActive = !isMagnifierActive;
        redStripe.SetActive(isMagnifierActive);
        animator.SetBool(EnabledHash, isMagnifierActive);

        AudioManager.Instance.TriggerMagnifierSound();
    }
}
