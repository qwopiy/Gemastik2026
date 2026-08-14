using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    [Header("Canvas Reference")]
    [Tooltip("Drag your main UI Canvas here")]
    public Canvas targetCanvas;

    [Header("Cursor Textures")]
    public Texture2D defaultCursor;
    public Texture2D pointerCursor;
    public Texture2D handCursor;
    public Texture2D dragCursor;

    [Header("Hotspot Settings")]
    public Vector2 topLeftHotSpot = Vector2.zero;
    public Vector2 centerHotSpot = new(16,16);

    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    void Awake()
    {
        // 1. Singleton pattern: Destroy duplicates if they exist when returning to a scene
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // 2. Instruct Unity to keep this GameObject alive when changing scenes
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        // Subscribe to Unity's scene loaded event loop
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks when the game shuts down
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 3. Automatically runs every time a new scene finishes loading
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindActiveCanvas();
        eventSystem = EventSystem.current;
        SetCustomCursor(defaultCursor, topLeftHotSpot);
    }

    void FindActiveCanvas()
    {
        // Finds the primary active canvas in the current scene
        targetCanvas = GameObject.FindGameObjectWithTag("GameCanvas").GetComponent<Canvas>();

        if (targetCanvas != null)
        {
            raycaster = targetCanvas.GetComponent<GraphicRaycaster>();

            // If the canvas lacks a raycaster for some reason, add it dynamically
            if (raycaster == null)
            {
                raycaster = targetCanvas.gameObject.AddComponent<GraphicRaycaster>();
            }
        }
        else
        {
            raycaster = null;
            Debug.LogWarning("[CentralizedUICursor] No Canvas found in the current scene!");
        }
    }

    void Update()
    {
        if (raycaster == null || eventSystem == null) return;

        // 1. Set up the pointer position data
        pointerEventData = new PointerEventData(eventSystem)
        {
            position = Mouse.current.position.ReadValue()
        };

        // 2. Create a list to store all UI elements hit under the mouse
        List<RaycastResult> results = new List<RaycastResult>();

        // 3. Raycast against the Canvas
        raycaster.Raycast(pointerEventData, results);

        // 4. If we hit UI elements, evaluate the topmost one
        if (results.Count > 0)
        {
            GameObject hitUIObject = results[0].gameObject;

            // 1. Check if the object (or its parents) is a Button component
            Button btn = hitUIObject.GetComponentInParent<Button>();
            DraggableObject draggableObject = hitUIObject.GetComponentInParent<DraggableObject>();

            if (btn != null && btn.interactable)
            {
                // Automatically switches to button cursor if the button is active
                SetCustomCursor(pointerCursor, topLeftHotSpot);
            }
            else if (draggableObject != null)
            {
                // Switch to drag cursor if the object is draggable
                if (!Mouse.current.leftButton.isPressed)
                {
                    SetCustomCursor(handCursor, centerHotSpot);
                }
                else
                {
                    SetCustomCursor(dragCursor, centerHotSpot);
                }
            }
            else
            {
                // 2. Fallback to Tag checking if it's not an interactable button
                EvaluateUITag(hitUIObject.tag);
            }
        }
        else
        {
            // Reset to default if the mouse is over empty screen space
            SetCustomCursor(defaultCursor, topLeftHotSpot);
        }
    }

    void EvaluateUITag(string objectTag)
    {
        switch (objectTag)
        {
            case "Interactible":
                SetCustomCursor(pointerCursor, topLeftHotSpot);
                break;
            default:
                SetCustomCursor(defaultCursor, topLeftHotSpot);
                break;
        }
    }

    void SetCustomCursor(Texture2D cursorTexture, Vector2 hotspot)
    {
        Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
    }
}
