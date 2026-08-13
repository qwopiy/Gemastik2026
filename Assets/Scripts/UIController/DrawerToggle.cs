using UnityEngine;

public class DrawerToggle : MonoBehaviour
{
    private static readonly int IsOpenHash = Animator.StringToHash("IsOpen");
    private bool isOpen = false;
    public Animator animator;
    public void Toggle()
    {
        isOpen = !isOpen;
        animator.SetBool(IsOpenHash, isOpen);
        if (isOpen)
        {
            AudioManager.Instance.TriggerDrawerOpen();
        }
        else
        {
            AudioManager.Instance.TriggerDrawerClose();
        }
    }
}
