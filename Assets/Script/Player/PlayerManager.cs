using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI textLog;
    public ScrollRect scrollRect;
    public Button nextButton;

    [Header("Settings")]
    public float typeSpeed = 0.02f;
    

    private void Start()
    {
        textLog.text = "";
        AddLog("L'aventure commence. Préparez l'équipement !");
    }

    // Add a log in the text area
    public void AddLog(string message, string colorHex = "white")
    {
        string newText = $"<color={colorHex}>{message}</color>\n\n";
        StartCoroutine(TypeFormattedText(newText));
    }

    // Required for displaying lisible text
    private IEnumerator TypeFormattedText(string message)
    {
        int startingIndex = textLog.text.Length;
        textLog.text += message;
        
        textLog.ForceMeshUpdate();

        int totalCharacters = textLog.textInfo.characterCount;

        textLog.maxVisibleCharacters = startingIndex;

        for (int i = startingIndex; i <= totalCharacters; i++)
        {
            textLog.maxVisibleCharacters = i;
            
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
            
            yield return new WaitForSeconds(typeSpeed);
        }
        
        textLog.maxVisibleCharacters = 99999;
    }

    /*
    public void TestNextEvent()
    {
        int rand = Random.Range(0, 3);
        
        if (rand == 0) 
            AddLog("Le héros trouve une <color=yellow>Épée Rouillée</color>. Rangez-la !", "#00FF00");
        else if (rand == 1)
            AddLog("Un loup attaque ! Le héros <color=red>saigne</color>.", "#FF5555");
        else
            AddLog("Le héros attend que vous rangiez son sac", "#AAAAAA");
    }
    */
}