using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    Weapon,
    Shield,
    Consumable,
    Helmet,
    Torso,
    Leg,
    Foot,
}

public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public enum StatType
{
    // Stats d'armes
    Damage,
    CriticalChance,
    Accuracy,
    
    // Stats d'armure
    Armor,
    Thorns,
    
    // Stats communes
    Durability,
    
    // Stats de consommables
    Heal,
    DamageBoost,
    AccuracyBoost
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Identification")]
    public ItemType itemType = ItemType.None;

    /// <summary>
    /// Size in width and height of the item.
    /// </summary>
    [Header("Main")]
    public SizeInt size = new();

    /// <summary>
    /// Item icon.
    /// </summary>
    [Header("Visual")]
    public Sprite icon;

    /// <summary>
    /// Background color of the item icon.
    /// </summary>
    public Color backgroundColor;

    [Header("Tooltip Information")]
    [Tooltip("Nom affiché dans le tooltip")]
    public string itemName = "Item";
    
    [Tooltip("Rareté de l'item (change la couleur du tooltip)")]
    public ItemRarity rarity = ItemRarity.Common;
    
    [Tooltip("Description de l'item")]
    [TextArea(3, 6)]
    public string description = "";
    
    [Header("Stats")]
    [Tooltip("Stats de l'item - Toutes les valeurs sont positives")]
    public ItemStat[] stats;

    /// <summary>
    /// Retourne la couleur associée à la rareté
    /// </summary>
    public Color GetRarityColor()
    {
        return rarity switch
        {
            ItemRarity.Common => new Color(0.7f, 0.7f, 0.7f), // Gris
            ItemRarity.Uncommon => new Color(0.2f, 0.8f, 0.2f), // Vert
            ItemRarity.Rare => new Color(0.2f, 0.5f, 1f), // Bleu
            ItemRarity.Epic => new Color(0.7f, 0.2f, 0.9f), // Violet
            ItemRarity.Legendary => new Color(1f, 0.6f, 0f), // Orange
            _ => Color.white
        };
    }
}

[System.Serializable]
public struct ItemStat
{
    [Tooltip("Type de stat")]
    public StatType statType;
    
    [Tooltip("Valeur de la stat (toujours positive)")]
    public float value;

    /// <summary>
    /// Retourne le nom formaté de la stat en français
    /// </summary>
    public string GetStatName()
    {
        return statType switch
        {
            StatType.Damage => "Dégâts",
            StatType.CriticalChance => "Critiques",
            StatType.Accuracy => "Précision",
            StatType.Armor => "Armure",
            StatType.Thorns => "Épines",
            StatType.Durability => "Durabilité",
            StatType.Heal => "Soin",
            StatType.DamageBoost => "Bonus Dégâts",
            StatType.AccuracyBoost => "Bonus Précision",
            _ => statType.ToString()
        };
    }

    /// <summary>
    /// Retourne le suffixe approprié pour la stat
    /// </summary>
    public string GetSuffix()
    {
        return statType switch
        {
            StatType.CriticalChance => "%",
            StatType.Accuracy => "%",
            StatType.Thorns => " pts",
            StatType.Armor => " pts",
            StatType.Durability => "",
            StatType.Heal => " PV",
            StatType.DamageBoost => "%",
            StatType.AccuracyBoost => "%",
            _ => ""
        };
    }

    /// <summary>
    /// Retourne la couleur de la stat selon son type
    /// </summary>
    public Color GetStatColor()
    {
        return statType switch
        {
            // Stats d'attaque en rouge
            StatType.Damage or StatType.CriticalChance or StatType.DamageBoost => new Color(1f, 0.4f, 0.4f),
            
            // Stats de défense en bleu
            StatType.Armor or StatType.Thorns => new Color(0.4f, 0.7f, 1f),
            
            // Stats de soin en vert
            StatType.Heal => new Color(0.4f, 1f, 0.4f),
            
            // Stats de précision en jaune
            StatType.Accuracy or StatType.AccuracyBoost => new Color(1f, 0.9f, 0.4f),
            
            // Durabilité en gris
            StatType.Durability => new Color(0.8f, 0.8f, 0.8f),
            
            _ => Color.white
        };
    }

    public override string ToString()
    {
        // Toutes les stats sont positives, donc on met toujours un +
        return $"+{value}{GetSuffix()} {GetStatName()}";
    }

    /// <summary>
    /// Version avec couleur HTML pour TextMeshPro
    /// </summary>
    public string ToColoredString()
    {
        Color color = GetStatColor();
        string hexColor = ColorUtility.ToHtmlStringRGB(color);
        return $"<color=#{hexColor}>+{value}{GetSuffix()} {GetStatName()}</color>";
    }
}