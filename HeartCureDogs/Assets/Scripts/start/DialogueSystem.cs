using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public GameObject nextIndicator;
    private Queue<string> sentences = new Queue<string>();
    private bool isTyping;

    public void StartDialogue(List<string> dialogues)
    {
        sentences.Clear();
        foreach (string line in dialogues) sentences.Enqueue(line);
        ShowNextSentence();
    }

    public void ShowNextSentence()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogText.text = sentences.Peek();
            isTyping = false;
            return;
        }

        if (sentences.Count == 0) return;

        string sentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        nextIndicator.SetActive(false);
        dialogText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        isTyping = false;
        nextIndicator.SetActive(sentences.Count > 0);
    }
}
