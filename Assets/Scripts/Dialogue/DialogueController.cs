using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogueController : MonoBehaviour
{
    [Header("Text Settings")]
    public float textSpeed;
    public GameObject speechBubble;
    public TextMeshProUGUI textbox;
    public Dialogues currentDialogue;

    [Header("Sound Settings")]
    public AudioClip typingSound;
    public AudioSource audioSource;

    private int dialogueIndex;
    private ClientController clientController;
    private SpeechBubbleController speechBubbleController;
    private float lastInputTime = 0f;
    private readonly float inputCooldown = 0.2f; // 200ms cooldown

    private void Start()
    {
        clientController = GetComponentInChildren<ClientController>(true);
        speechBubbleController = GetComponentInChildren<SpeechBubbleController>(true);

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = typingSound;

        LevelManager.Instance.OnDialogueTriggered += SetDialogue;
        DialogueEventManager.Instance.StartGameEvent += SetDialogue;
        DialogueEventManager.Instance.OnDialogueStarted += StartDialogue;
        DialogueEventManager.Instance.OnDialogueCompleted += EndDialogue;
        DialogueEventManager.Instance.OnDialogueContinued += ContinueDialogue;

        DialogueEventManager.Instance.OnFoodSubmitted += SetEndingDialogue;
    }

    private void OnDisable()
    {
        LevelManager.Instance.OnDialogueTriggered -= SetDialogue;
        DialogueEventManager.Instance.StartGameEvent -= SetDialogue;
        DialogueEventManager.Instance.OnDialogueStarted -= StartDialogue;
        DialogueEventManager.Instance.OnDialogueCompleted -= EndDialogue;
        DialogueEventManager.Instance.OnDialogueContinued -= ContinueDialogue;

        DialogueEventManager.Instance.OnFoodSubmitted -= SetEndingDialogue;
    }

    public void SetDialogue()
    {
        if (LevelManager.Instance.index < LevelManager.Instance.ClientDataList.Count)
        {
            SetDialogue(LevelManager.Instance.ClientDataList[LevelManager.Instance.index].Dialogue);
        }
    }
    private void SetDialogue(Dialogues dialogue)
    {
        currentDialogue = dialogue;
        dialogueIndex = 0;
        typingSound = currentDialogue.voiceClip;
    }

    public void StartDialogue()
    {
        if (currentDialogue != null)
        {
            StartCoroutine(ShowDialogue());
        }
    }

    public IEnumerator ShowDialogue()
    {
        ClearTextbox();
        yield return speechBubbleController.ShowSpeechBubble();
        yield return TypeLinesCoroutine(currentDialogue.GetLine(dialogueIndex));
    }

    public void EndDialogue()
    {
        StartCoroutine(HideDialogue());

        if (currentDialogue.isFirstDialogue)
        {
            currentDialogue.isFirstDialogue = false;
            LevelManager.Instance.TriggerSendFoodEvent();
        } 
        else
        {
            DialogueEventManager.Instance.TriggerClientExit();
        }
    }

    public IEnumerator HideDialogue()
    {
        ClearTextbox();
        yield return speechBubbleController.HideSpeechBubble();
    }

    private void TypeLines(string text)
    {
        StartCoroutine(TypeLinesCoroutine(text));
    }
    private IEnumerator TypeLinesCoroutine(string text)
    {
        StartVoiceClip();
        foreach (char c in text.ToCharArray())
        {
            textbox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        StopVoiceClip();
    }

    public void ContinueDialogue()
    {
        if (Time.time - lastInputTime < inputCooldown)
            return; // Ignore rapid input

        lastInputTime = Time.time;


        if (textbox.text == currentDialogue.GetLine(dialogueIndex))
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            StopVoiceClip();
            textbox.text = currentDialogue.GetLine(dialogueIndex);
        }
    }

    private void NextLine()
    {
        if (dialogueIndex < currentDialogue.GetLineCount() - 1)
        {
            dialogueIndex++;
            ClearTextbox();
            TypeLines(currentDialogue.GetLine(dialogueIndex));
        }
        else
        {
            EndDialogue();
        }
    }

    private void ClearTextbox()
    {
        textbox.text = string.Empty;
    }

    private void SetEndingDialogue()
    {
        currentDialogue.isFirstDialogue = false;
        dialogueIndex = 0;

        DialogueEventManager.Instance.TriggerDialogueStart();
    }

    private void StartVoiceClip()
    {
        if (audioSource != null && typingSound != null)
        {
            audioSource.clip = typingSound;
            audioSource.Play();
        }
    }

    private void StopVoiceClip()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}
