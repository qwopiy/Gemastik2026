using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class ClientController : MonoBehaviour
{
    private static readonly int ExitHash = Animator.StringToHash("Exit");
    private static readonly int EnterHash = Animator.StringToHash("Enter");
    private Image clientImage;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        DialogueEventManager.Instance.StartGameEvent += OnClientEnter;

        DialogueEventManager.Instance.OnClientEntered += OnClientEnter;
        DialogueEventManager.Instance.OnClientExited += OnClientExit;
    }

    private void OnDisable()
    {
        DialogueEventManager.Instance.StartGameEvent -= OnClientEnter;

        DialogueEventManager.Instance.OnClientEntered -= OnClientEnter;
        DialogueEventManager.Instance.OnClientExited -= OnClientExit;
    }

    public void SetClientImage()
    {
        Sprite sprite = LevelManager.Instance.ClientDataList[LevelManager.Instance.index].Dialogue.speakersSprite;
        if (clientImage == null)
        {
            clientImage = GetComponent<Image>();
        }
        clientImage.sprite = sprite;
    }

    public void OnClientEnter()
    {
        animator.SetTrigger(EnterHash);
    }
    public void OnClientExit()
    {
        animator.SetTrigger(ExitHash);
        TriggerNextClient();
    }

    public void TriggerNextClient()
    {
        StartCoroutine(DelayBetweenClientsCoroutine());
    }

    public IEnumerator DelayBetweenClientsCoroutine()
    {
        yield return new WaitForSeconds(LevelManager.Instance.delayBetweenClients);

        Debug.Log("Started Next Client Dialogue");
        if (LevelManager.Instance.index < LevelManager.Instance.ClientDataList.Count)
        {
            LevelManager.Instance.TriggerDialogue(LevelManager.Instance.index);
            DialogueEventManager.Instance.TriggerClientEnter();
        }
        else
        {
            LevelManager.Instance.TriggerLevelCompletedEvent();
        }
    }

    public void StartCurrentDialogue()
    {
        Debug.Log("Dialogue Started");
        if (DialogueEventManager.Instance != null)
        {
            LevelManager.Instance.TriggerDialogue(LevelManager.Instance.index);
            DialogueEventManager.Instance.TriggerDialogueStart();
        }
    }
}
