using UnityEngine;

public class AnchorAssigner : MonoBehaviour
{
    [SerializeField] private GameObjectAnchorSO deskAnchor;
    private void OnEnable()
    {
        if (deskAnchor != null)
        {
            deskAnchor.value = gameObject;
        }
    }

    private void OnDisable()
    {
        if (deskAnchor != null && deskAnchor.value == gameObject)
        {
            deskAnchor.value = null;
        }
    }
}
