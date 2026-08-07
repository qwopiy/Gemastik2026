using UnityEngine;

public class MagnifierController : MonoBehaviour
{
    public Camera zoomCamera;
    public GameObject magnifierCanvas;

    public void ToggleMagnifier()
    {
        bool isActive = !magnifierCanvas.activeSelf;
        magnifierCanvas.SetActive(isActive);
        zoomCamera.enabled = isActive;
    }
}
