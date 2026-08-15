using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Endings CurrentEnding;

    // Global Events
    public event Action EscapePressedEvent;

    public void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            EscapePressedEvent?.Invoke();
        }
    }
}
