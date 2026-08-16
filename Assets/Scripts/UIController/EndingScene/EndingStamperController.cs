using UnityEngine;
using UnityEngine.UI;

public class EndingStamperController : MonoBehaviour
{
    public Image stampImage;

    public Sprite GoodStamp;
    public Sprite BadStamp;
    public void EnableStamp()
    {
        AudioManager.Instance.TriggerStamp();
        bool isGoodEnding = GlobalManager.Instance.CurrentEnding == Endings.AllCorrect || GlobalManager.Instance.CurrentEnding == Endings.PerfectSpeedrunner;

        if (isGoodEnding)
        {
            stampImage.sprite = GoodStamp;
        }
        else
        {
            stampImage.sprite = BadStamp;
        }

        stampImage.enabled = true;
    }
}
