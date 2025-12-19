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

    [Header("Stats (Optional)")]
    [Tooltip("Stats à afficher (ex: Dégâts, Défense, etc.)")]
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
    [Tooltip("Nom du stat (ex: Dégâts, Défense, Vitesse)")]
    public string statName;

    [Tooltip("Valeur du stat")]
    public float value;

    [Tooltip("Affiche un + devant les valeurs positives")]
    public bool showPlusSign;

    public override string ToString()
    {
        string prefix = showPlusSign && value > 0 ? "+" : "";
        return $"{statName}: {prefix}{value}";
    }
}