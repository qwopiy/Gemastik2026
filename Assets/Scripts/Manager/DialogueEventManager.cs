using System;
using UnityEngine;

public class DialogueEventManager : MonoBehaviour
{
    public static DialogueEventManager Instance;
    public event Action StartGameEvent;
    public event Action OnClientEntered;
    public event Action OnClientExited;
    public event Action OnDialogueStarted;
    public event Action OnDialogueCompleted;
    public event Action OnDialogueContinued;
    public event Action OnFoodSubmitted;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void TriggerClientEnter()
    {
        OnClientEntered?.Invoke();
    }

    public void TriggerClientExit()
    {
        OnClientExited?.Invoke();
    }

    public void TriggerDialogueStart()
    {
        OnDialogueStarted?.Invoke();
    }

    public void TriggerDialogueComplete()
    {
        OnDialogueCompleted?.Invoke();
    }

    public void TriggerDialogueContinued()
    {
        OnDialogueContinued?.Invoke();
    }

    public void TriggerFoodSubmitted()
    {
        OnFoodSubmitted?.Invoke();
    }

    public void TriggerStartEvent()
    {
        StartGameEvent?.Invoke();
    }
}