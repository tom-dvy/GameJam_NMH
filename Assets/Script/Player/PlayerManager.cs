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
        AddLog("Système : L'aventure commence. Préparez l'équipement !");
    }

    public void AddLog(string message, string colorHex = "white")
    {
        string formattedMessage = $"<color={colorHex}>{message}</color>\n\n";
        StartCoroutine(TypeText(formattedMessage));
    }

    private IEnumerator TypeText(string message)
    {
        foreach (char c in message)
        {
            textLog.text += c;
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
            yield return new WaitForSeconds(typeSpeed);
        }
    }
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
}