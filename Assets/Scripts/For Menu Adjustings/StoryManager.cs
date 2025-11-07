using UnityEngine;
using TMPro;
using System.Collections;

public class StoryManager : MonoBehaviour
{
    public TMP_Text storyText;
    public string[] sentences;
    public float typingSpeed = 0.05f;
    public GameObject storyPanel;
    public static bool isStoryActive = false;

    void Start()
    {
        if (storyPanel != null)
        {
            StartCoroutine(PlayStory());
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        storyText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            storyText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    IEnumerator PlayStory()
    {
        for (int i = 0; i < sentences.Length; i++)
        {
            yield return StartCoroutine(TypeSentence(sentences[i]));
            yield return new WaitForSeconds(3f);
        }

        if (storyPanel != null)
            storyPanel.SetActive(false);

        isStoryActive = true;
    }
}
