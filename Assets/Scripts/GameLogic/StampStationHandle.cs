using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public enum ApprovalResult { None, Approved, Mixed }
public enum GGLSticker {None, A, B, C, D, Mixed  }

public class StampStationHandle : MonoBehaviour
{

    [Header("Stamp Configuration")]
    public ApprovalResult stampType;
    public GameObject markPrefab;         // Green Approved or Red Denied mark UI prefab
    public RectTransform strikePoint;     // Empty UI object showing where the ink hits the desk

    [Header("Animation & Sound")]
    public RectTransform stampArm;        // The moving physical handle graphic
    public float pressDistance = 25f;     // How far down the arm moves on click
    public float pressSpeed = 0.08f;      // Down/Up animation duration
    public AudioSource audioSource;
    public AudioClip stampThudSound;

    private bool isStamping = false;
    private Vector2 originalArmPos;

    private void Start()
    {
        if (stampArm != null)
            originalArmPos = stampArm.anchoredPosition;
    }

    public void OnStampClicked()
    {
        if (isStamping) return;
        StartCoroutine(PressStampSequence());
    }

    private IEnumerator PressStampSequence()
    {
        isStamping = true;

        // 1. Animate Stamp Moving Down
        if (stampArm != null)
        {
            Vector2 pressedPos = originalArmPos - new Vector2(0, pressDistance);
            stampArm.anchoredPosition = pressedPos;
        }

        // 2. Play Sound Effect
        //if (audioSource && stampThudSound)
        //    audioSource.PlayOneShot(stampThudSound);

        // 3. Detect Food Item Directly Underneath the Strike Point
        CheckAndApplyStamp();

        // Pause briefly for impact feel
        yield return new WaitForSeconds(pressSpeed);

        // 4. Animate Stamp Returning Up
        if (stampArm != null)
        {
            stampArm.anchoredPosition = originalArmPos;
        }

        isStamping = false;
    }

    private void CheckAndApplyStamp()
    {
        // Get the screen point where the stamp physically strikes
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, strikePoint.position);

        // Raycast down through UI layers at that exact point
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = screenPoint
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            // Make sure your Food UI object has the "Form" tag
            if (result.gameObject.CompareTag("Form"))
            {
                GameObject food = result.gameObject.GetComponentInChildren<StampParent>().gameObject;
                result.gameObject.GetComponentInParent<StampsOnFood>().SetStampResult(stampType);
                ApplyMarkToFood(food, screenPoint);
                break; // Stamp the top-most food item found
            }
        }
    }

    private void ApplyMarkToFood(GameObject foodObject, Vector2 screenPoint)
    {
        // Instantiate the stamp mark as a child of the food item
        GameObject newMark = Instantiate(markPrefab, foodObject.transform);
        RectTransform markRect = newMark.GetComponent<RectTransform>();
        RectTransform foodRect = foodObject.GetComponent<RectTransform>();

        // Convert the strike point screen position into the food item's local coordinates
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            foodRect,
            screenPoint,
            Camera.main,
            out Vector2 localPoint
        );

        markRect.anchoredPosition = localPoint;

        // Slight human error rotation (-6 to +6 degrees)
        markRect.localRotation = Quaternion.Euler(0, 0, Random.Range(-6f, 6f));

        // Pass judgment to the food item's script
        // foodObject.GetComponent<FoodItem>()?.SetStatus(stampType);
    }
}