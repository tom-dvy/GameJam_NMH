using UnityEngine;
using System.Collections.Generic;

public class PlayerStatsManager : MonoBehaviour
{
    [Header("References")]
    public Inventory inventory;
    public PlayerManager playerManager;

    [Header("Equipment Grids")]
    [Tooltip("Grille pour l'arme équipée")]
    public InventoryGrid weaponGrid;

    [Tooltip("Grille pour le bouclier équipé")]
    public InventoryGrid shieldGrid;

    [Tooltip("Grille pour le casque équipé")]
    public InventoryGrid helmetGrid;

    [Tooltip("Grille pour le plastron équipé")]
    public InventoryGrid chestplateGrid;

    [Tooltip("Grille pour les jambières équipées")]
    public InventoryGrid leggingsGrid;

    [Tooltip("Grille pour les bottes équipées")]
    public InventoryGrid bootsGrid;

    [Header("Consumable Grid")]
    [Tooltip("Grille où placer les consommables pour les utiliser")]
    public InventoryGrid consumableSlot;

    // Stats de base du joueur
    [Header("Base Stats")]
    public int baseHP = 100;
    public int baseDamage = 5;
    public float baseAccuracy = 50f;
    public float baseCritChance = 5f;

    // Stats temporaires (buffs des consommables)
    private float tempDamageBoost = 0f;
    private float tempAccuracyBoost = 0f;
    private float boostDuration = 0f;

    private void Update()
    {
        // Décrémenter la durée des buffs
        if (boostDuration > 0)
        {
            boostDuration -= Time.deltaTime;
            if (boostDuration <= 0)
            {
                tempDamageBoost = 0f;
                tempAccuracyBoost = 0f;
                if (playerManager != null)
                {
                    playerManager.AddLog("Les effets du consommable se sont dissipés.", "#888888");
                }
            }
        }
    }

    /// <summary>
    /// Calcule les dégâts totaux du joueur (base + équipement + buffs)
    /// </summary>
    public int GetTotalDamage()
    {
        float totalDamage = baseDamage;

        // Ajouter les dégâts de l'arme
        Item weapon = GetEquippedItem(weaponGrid);
        if (weapon != null)
        {
            totalDamage += GetStatValue(weapon, StatType.Damage);
        }

        // Appliquer les buffs temporaires
        totalDamage += totalDamage * (tempDamageBoost / 100f);

        return Mathf.RoundToInt(totalDamage);
    }

    /// <summary>
    /// Calcule l'armure totale du joueur
    /// </summary>
    public int GetTotalArmor()
    {
        int totalArmor = 0;

        totalArmor += GetArmorFromGrid(shieldGrid);
        totalArmor += GetArmorFromGrid(helmetGrid);
        totalArmor += GetArmorFromGrid(chestplateGrid);
        totalArmor += GetArmorFromGrid(leggingsGrid);
        totalArmor += GetArmorFromGrid(bootsGrid);

        return totalArmor;
    }

    /// <summary>
    /// Calcule les dégâts d'épines (thorns) totaux
    /// </summary>
    public int GetTotalThorns()
    {
        int totalThorns = 0;

        totalThorns += GetThornsFromGrid(shieldGrid);
        totalThorns += GetThornsFromGrid(helmetGrid);
        totalThorns += GetThornsFromGrid(chestplateGrid);
        totalThorns += GetThornsFromGrid(leggingsGrid);
        totalThorns += GetThornsFromGrid(bootsGrid);

        return totalThorns;
    }

    /// <summary>
    /// Calcule la précision totale du joueur
    /// </summary>
    public float GetTotalAccuracy()
    {
        float totalAccuracy = baseAccuracy;

        Item weapon = GetEquippedItem(weaponGrid);
        if (weapon != null)
        {
            totalAccuracy += GetStatValue(weapon, StatType.Accuracy) - baseAccuracy;
        }

        // Appliquer les buffs temporaires
        totalAccuracy += tempAccuracyBoost;

        return Mathf.Clamp(totalAccuracy, 0f, 100f);
    }

    /// <summary>
    /// Calcule les chances de coup critique totales
    /// </summary>
    public float GetTotalCritChance()
    {
        float totalCrit = baseCritChance;

        Item weapon = GetEquippedItem(weaponGrid);
        if (weapon != null)
        {
            totalCrit += GetStatValue(weapon, StatType.CriticalChance);
        }

        return Mathf.Clamp(totalCrit, 0f, 100f);
    }

    /// <summary>
    /// Réduit la durabilité de l'arme équipée
    /// </summary>
    public void DamageWeapon(int amount = 1)
    {
        Item weapon = GetEquippedItem(weaponGrid);
        if (weapon != null)
        {
            ReduceDurability(weapon, amount);
        }
    }

    /// <summary>
    /// Réduit la durabilité de toutes les pièces d'armure
    /// </summary>
    public void DamageArmor(int amount = 1)
    {
        DamageArmorPiece(shieldGrid, amount);
        DamageArmorPiece(helmetGrid, amount);
        DamageArmorPiece(chestplateGrid, amount);
        DamageArmorPiece(leggingsGrid, amount);
        DamageArmorPiece(bootsGrid, amount);
    }

    /// <summary>
    /// Calcule les dégâts réduits par l'armure
    /// </summary>
    public int CalculateDamageReduction(int incomingDamage)
    {
        int armor = GetTotalArmor();

        // Formule de réduction : chaque point d'armure réduit 1% des dégâts
        // Maximum 75% de réduction
        float reductionPercent = Mathf.Min(armor * 1f, 75f);
        int reducedDamage = Mathf.RoundToInt(incomingDamage * (1f - reductionPercent / 100f));

        return Mathf.Max(1, reducedDamage); // Minimum 1 dégât
    }

    /// <summary>
    /// Utilise le consommable dans le slot de consommable
    /// </summary>
    public void UseConsumable()
    {
        Item consumable = GetEquippedItem(consumableSlot);

        if (consumable == null || consumable.data.itemType != ItemType.Consumable)
        {
            if (playerManager != null)
            {
                playerManager.AddLog("Aucun consommable à utiliser !", "#FF5555");
            }
            return;
        }

        // Appliquer les effets
        bool effectApplied = false;

        foreach (var stat in consumable.data.stats)
        {
            switch (stat.statType)
            {
                case StatType.Heal:
                    playerManager.player.hp += Mathf.RoundToInt(stat.value);
                    playerManager.player.hp = Mathf.Min(playerManager.player.hp, baseHP);
                    playerManager.AddLog($"Vous récupérez <color=green>{stat.value} PV</color> !", "white");
                    effectApplied = true;
                    break;

                case StatType.DamageBoost:
                    tempDamageBoost = stat.value;
                    boostDuration = 30f; // 30 secondes de buff
                    playerManager.AddLog($"Vos dégâts augmentent de <color=red>{stat.value}%</color> pour 30 secondes !", "white");
                    effectApplied = true;
                    break;

                case StatType.AccuracyBoost:
                    tempAccuracyBoost = stat.value;
                    boostDuration = 30f;
                    playerManager.AddLog($"Votre précision augmente de <color=yellow>{stat.value}%</color> pour 30 secondes !", "white");
                    effectApplied = true;
                    break;
            }
        }

        if (effectApplied)
        {
            // Détruire le consommable après utilisation
            inventory.RemoveItem(consumable);
        }
    }

    // ========== MÉTHODES PRIVÉES ==========

    private Item GetEquippedItem(InventoryGrid grid)
    {
        if (grid == null || grid.items == null) return null;

        for (int x = 0; x < grid.gridSize.x; x++)
        {
            for (int y = 0; y < grid.gridSize.y; y++)
            {
                if (grid.items[x, y] != null)
                {
                    return grid.items[x, y];
                }
            }
        }
        return null;
    }

    private float GetStatValue(Item item, StatType statType)
    {
        if (item == null || item.data == null || item.data.stats == null) return 0f;

        foreach (var stat in item.data.stats)
        {
            if (stat.statType == statType)
            {
                return stat.value;
            }
        }
        return 0f;
    }

    private int GetArmorFromGrid(InventoryGrid grid)
    {
        Item item = GetEquippedItem(grid);
        return item != null ? Mathf.RoundToInt(GetStatValue(item, StatType.Armor)) : 0;
    }

    private int GetThornsFromGrid(InventoryGrid grid)
    {
        Item item = GetEquippedItem(grid);
        return item != null ? Mathf.RoundToInt(GetStatValue(item, StatType.Thorns)) : 0;
    }

    private void DamageArmorPiece(InventoryGrid grid, int amount)
    {
        Item item = GetEquippedItem(grid);
        if (item != null)
        {
            ReduceDurability(item, amount);
        }
    }

    private void ReduceDurability(Item item, int amount)
    {
        if (item == null || item.data == null || item.data.stats == null) return;

        // Trouver et réduire la durabilité
        for (int i = 0; i < item.data.stats.Length; i++)
        {
            if (item.data.stats[i].statType == StatType.Durability)
            {
                item.data.stats[i].value -= amount;

                if (item.data.stats[i].value <= 0)
                {
                    // L'item est cassé
                    if (playerManager != null)
                    {
                        playerManager.AddLog($"<color=red>{item.data.itemName}</color> est détruit !", "#FF5555");
                    }
                    inventory.RemoveItem(item);
                }
                else if (item.data.stats[i].value <= 20)
                {
                    // Avertissement de durabilité faible
                    if (playerManager != null && Random.value < 0.3f) // 30% de chance d'afficher l'avertissement
                    {
                        playerManager.AddLog($"<color=orange>{item.data.itemName}</color> est presque cassé ! (Durabilité: {item.data.stats[i].value})", "#FFA500");
                    }
                }
                break;
            }
        }
    }
}