using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class SpeechBubbleController : MonoBehaviour, IPointerClickHandler
{
    private static readonly int HideHash = Animator.StringToHash("Hide");
    private static readonly int ShowHash = Animator.StringToHash("Show");
    public float delayBeforeDialogue = 1.0f; // Delay before starting the dialogue
    private Animator animator;


    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public IEnumerator ShowSpeechBubble()
    {
        // show the speech bubble
        animator.SetTrigger(ShowHash);

        yield return new WaitForSeconds(delayBeforeDialogue);
        // start dialogue
    }

    public IEnumerator HideSpeechBubble()
    {
        // hide the speech bubble
        animator.SetTrigger(HideHash);
        // end dialogue
        yield return new WaitForSeconds(delayBeforeDialogue);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DialogueEventManager.Instance.TriggerDialogueContinued();
    }
}
