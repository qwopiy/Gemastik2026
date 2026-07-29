using System.Collections;
using UnityEngine;
public class StampsOnFood : MonoBehaviour
{
    public ApprovalResult approvalResult = ApprovalResult.None;
    public GGLSticker gglSticker = GGLSticker.None;

    public void SetStampResult(ApprovalResult result)
    {
        if (approvalResult == ApprovalResult.None) approvalResult = result;
        else approvalResult = ApprovalResult.Mixed;
    }

    public void SetGGLStamp(GGLSticker stamp)
    {
        if (gglSticker == GGLSticker.None) gglSticker = stamp;
        else gglSticker = GGLSticker.Mixed;
    }

    public void ResetGGL()
    {
        gglSticker = GGLSticker.None;
    }

    public void CheckForGGLStickers()
    {
        ResetGGL();
        Transform foodChild = transform.GetChild(0);
        for (int i = 0; i < foodChild.childCount; i++)
        {
            if (foodChild.GetChild(i).TryGetComponent(out GGLStickerItem sticker))
            {
                SetGGLStamp(sticker.stamp);
            }
        }
    }
}