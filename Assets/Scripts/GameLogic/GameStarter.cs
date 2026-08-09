using System.Collections;
using UnityEngine; 
public class GameStarter : MonoBehaviour
{
    public void StartGame()
    {
        DialogueEventManager.Instance.TriggerStartEvent();
    }
}