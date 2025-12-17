using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogBoxManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI textDisplay;
    public Button nextButton;

    [Header("Game Data")]
    private Queue<string> sentences = new Queue<string>(); // Queue for messages that will been put on the screen

    public void AddEvent(string description)
    {
        sentences.Enqueue(description);
        
        if (sentences.Count == 1)
        {
            DisplayNextSentence();
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            textDisplay.text = "En attente d'actions...";
            nextButton.interactable = false;
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence)); // Progressive writting 
        
        nextButton.interactable = sentences.Count > 0;
    }

    // The text is printed letter by letter
    System.Collections.IEnumerator TypeSentence(string sentence)
    {
        textDisplay.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(0.02f); 
        }
    }
}