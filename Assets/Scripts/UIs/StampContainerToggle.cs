using UnityEngine;

public class StampContainerToggle : MonoBehaviour
{
    private static readonly int IsOpenHash = Animator.StringToHash("IsOpen");
    public Animator animator;
    public void Toggle()
    {
        animator.SetBool(IsOpenHash, !animator.GetBool(IsOpenHash));
    }
}
