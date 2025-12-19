using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    public class Player
    {
        public string name = "Héros";
        public int hp = 100;
        public int maxHP = 100;
        public string color = "#33ca1cff";
        public bool isDead = false;
    }

    [Header("UI Components")]
    public TextMeshProUGUI textLog;
    public ScrollRect scrollRect;
    public Button nextButton;

    [Header("Player Stats Display (Optional)")]
    public TextMeshProUGUI statsDisplay;

    [Header("Settings")]
    public float typeSpeed = 0.02f;

    [HideInInspector]
    public Player player;

    [HideInInspector]
    public PlayerStatsManager statsManager;

    private void Awake()
    {
        player = new Player();
        statsManager = GetComponent<PlayerStatsManager>();

        if (statsManager == null)
        {
            Debug.LogError("PlayerStatsManager not found! Add it to the same GameObject as PlayerManager.");
        }
    }

    private void Start()
    {
        textLog.text = "";
        AddLog("L'aventure commence. Préparez l'équipement !");
        UpdateStatsDisplay();
    }

    private void Update()
    {
        UpdateStatsDisplay();
    }

    // Add a log in the text area
    public void AddLog(string message, string colorHex = "white")
{
    if (textLog.text.Length > 500) 
    {
        textLog.text = "... (suite) ...\n\n"; 
    }

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

    /// <summary>
    /// Met à jour l'affichage des stats du joueur (optionnel)
    /// </summary>
    private void UpdateStatsDisplay()
    {
        if (statsDisplay == null || statsManager == null) return;

        int damage = statsManager.GetTotalDamage();
        int armor = statsManager.GetTotalArmor();
        int thorns = statsManager.GetTotalThorns();
        float accuracy = statsManager.GetTotalAccuracy();
        float crit = statsManager.GetTotalCritChance();

        statsDisplay.text = $"<b>STATS DU HÉROS</b>\n" +
                           $"<color=#FF6666>PV:</color> {player.hp}/{player.maxHP}\n" +
                           $"<color=#FF4444>Dégâts:</color> {damage}\n" +
                           $"<color=#4444FF>Armure:</color> {armor}\n" +
                           $"<color=#44FFFF>Épines:</color> {thorns}\n" +
                           $"<color=#FFFF44>Précision:</color> {accuracy:F0}%\n" +
                           $"<color=#B823B0>Critiques:</color> {crit:F0}%";
    }
}