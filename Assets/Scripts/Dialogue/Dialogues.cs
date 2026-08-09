using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogues 
{
    [Header("Speaker Settings")]
    public string speakersName;
    public Sprite speakersSprite;
    public AudioClip voiceClip;

    [Header("Dialogue Lines")]
    public List<string> startingLines;
    public List<string> endingLines;
    public List<string> randomChatter;

    public bool isFirstDialogue = true;

    public string GetRandomChatter()
    {
        if (randomChatter.Count == 0)
            return string.Empty;
        int randomIndex = Random.Range(0, randomChatter.Count);
        return randomChatter[randomIndex];
    }

    public string GetLine(int index)
    {
        if (index < 0)
            return string.Empty;

        if (isFirstDialogue)
        {
            if (index >= startingLines.Count)
                return string.Empty;
            return startingLines[index];
        }
        else
        {
            if (index >= endingLines.Count)
                return string.Empty;
            return endingLines[index];
        }
    }

    public int GetLineCount()
    {
        return isFirstDialogue ? startingLines.Count : endingLines.Count;
    }

    public Sprite GetSprite()
    {
        return speakersSprite;
    }
}