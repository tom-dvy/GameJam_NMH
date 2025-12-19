using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI rarityText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI statsText;
    public Image borderImage;
    public Image backgroundImage; // Optionnel : pour changer la couleur du fond

    [Header("Settings")]
    public Vector2 offset = new Vector2(15, -15);
    public float fadeSpeed = 10f;
    public bool useRarityBackground = false; // Coloriser le fond selon la rareté

    private RectTransform tooltipRect;
    private CanvasGroup canvasGroup;
    private bool isVisible = false;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        tooltipRect = tooltipPanel.GetComponent<RectTransform>();
        canvasGroup = tooltipPanel.GetComponent<CanvasGroup>();
        
        if (canvasGroup == null)
        {
            canvasGroup = tooltipPanel.AddComponent<CanvasGroup>();
        }

        HideTooltip();
    }

    private void Update()
    {
        if (isVisible)
        {
            // Suivre la position de la souris
            UpdatePosition();
            
            // Fade in
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, fadeSpeed * Time.deltaTime);
        }
        else
        {
            // Fade out
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, fadeSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Affiche le tooltip avec les informations de l'item
    /// </summary>
    public void ShowTooltip(ItemData itemData)
    {
        if (itemData == null) return;

        // Nom de l'item
        if (itemNameText != null)
        {
            itemNameText.text = itemData.itemName;
        }

        // Rareté
        if (rarityText != null)
        {
            rarityText.text = itemData.rarity.ToString();
            rarityText.color = itemData.GetRarityColor();
        }

        // Bordure colorée selon la rareté
        if (borderImage != null)
        {
            borderImage.color = itemData.GetRarityColor();
        }

        // Fond coloré (optionnel)
        if (useRarityBackground && backgroundImage != null)
        {
            Color bgColor = itemData.GetRarityColor();
            bgColor.a = 0.2f; // Semi-transparent
            backgroundImage.color = bgColor;
        }

        // Description
        if (descriptionText != null)
        {
            descriptionText.text = itemData.description;
            descriptionText.gameObject.SetActive(!string.IsNullOrEmpty(itemData.description));
        }

        // Stats
        if (statsText != null)
        {
            if (itemData.stats != null && itemData.stats.Length > 0)
            {
                string statsString = "";
                foreach (var stat in itemData.stats)
                {
                    if (!string.IsNullOrEmpty(statsString))
                    {
                        statsString += "\n";
                    }
                    // Utiliser la version colorée des stats
                    statsString += stat.ToColoredString();
                }
                statsText.text = statsString;
                statsText.gameObject.SetActive(true);
            }
            else
            {
                statsText.gameObject.SetActive(false);
            }
        }

        isVisible = true;
        tooltipPanel.SetActive(true);
        UpdatePosition();
    }

    /// <summary>
    /// Cache le tooltip
    /// </summary>
    public void HideTooltip()
    {
        isVisible = false;
        // On garde le panel actif pour le fade out, il sera désactivé une fois l'alpha à 0
        if (canvasGroup.alpha <= 0.01f)
        {
            tooltipPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Met à jour la position du tooltip pour suivre la souris
    /// </summary>
    private void UpdatePosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        
        // Ajouter l'offset
        Vector2 targetPosition = mousePosition + offset;

        // Empêcher le tooltip de sortir de l'écran
        RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        
        // Ajuster horizontalement
        if (targetPosition.x + tooltipRect.rect.width > canvasRect.rect.width)
        {
            targetPosition.x = mousePosition.x - tooltipRect.rect.width - offset.x;
        }

        // Ajuster verticalement
        if (targetPosition.y - tooltipRect.rect.height < 0)
        {
            targetPosition.y = mousePosition.y + tooltipRect.rect.height - offset.y;
        }

        tooltipRect.position = targetPosition;
    }
}