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


}