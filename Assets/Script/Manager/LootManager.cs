using UnityEngine;

public class LootManager : MonoBehaviour
{
    [Header("Références")]
    public Inventory inventory;
    public InventoryGrid tempoGrid;
    public PlayerManager playerManager;

    [Header("Loot par Rareté")]
    [Tooltip("Items Cuivre")]
    public ItemData[] copperItems;

    [Tooltip("Items Or")]
    public ItemData[] goldItems;

    [Tooltip("Items Diamant")]
    public ItemData[] diamondItems;

    [Tooltip("Items Netherite")]
    public ItemData[] netheriteItems;

    [Header("Loot de Base (Salles Normales)")]
    [Tooltip("Items Common (Cuir/Fer)")]
    public ItemData[] commonLoot;

    [Tooltip("Items Uncommon (Cuivre)")]
    public ItemData[] uncommonLoot;

    [Tooltip("Items Rare (Or)")]
    public ItemData[] rareLoot;

    [Tooltip("Items Epic (Diamant)")]
    public ItemData[] epicLoot;

    [Tooltip("Items Legendary (Netherite)")]
    public ItemData[] legendaryLoot;

    [Tooltip("Consommables")]
    public ItemData[] consumables;

    /// <summary>
    /// Génère du loot adapté au niveau de la salle (ancienne méthode pour compatibilité)
    /// </summary>
    public void GenerateLoot(int roomNumber = 1, bool isBoss = false)
    {
        ItemData itemToAdd = null;

        if (isBoss)
        {
            // Cette méthode est maintenant remplacée par GenerateBossLoot
            return;
        }

        // Salles 1-2 : Principalement Common
        if (roomNumber <= 2)
        {
            float roll = Random.Range(0f, 100f);

            if (roll < 10f && uncommonLoot.Length > 0)
            {
                itemToAdd = uncommonLoot[Random.Range(0, uncommonLoot.Length)];
            }
            else if (roll < 15f && consumables.Length > 0)
            {
                itemToAdd = consumables[Random.Range(0, consumables.Length)];
            }
            else if (commonLoot.Length > 0)
            {
                itemToAdd = commonLoot[Random.Range(0, commonLoot.Length)];
            }
        }
        // Salles 3-4 : Mélange
        else
        {
            float roll = Random.Range(0f, 100f);

            if (roll < 15f && epicLoot.Length > 0)
            {
                itemToAdd = epicLoot[Random.Range(0, epicLoot.Length)];
            }
            else if (roll < 50f && rareLoot.Length > 0)
            {
                itemToAdd = rareLoot[Random.Range(0, rareLoot.Length)];
            }
            else if (roll < 70f && uncommonLoot.Length > 0)
            {
                itemToAdd = uncommonLoot[Random.Range(0, uncommonLoot.Length)];
            }
            else if (consumables.Length > 0)
            {
                itemToAdd = consumables[Random.Range(0, consumables.Length)];
            }
        }

        if (itemToAdd != null)
        {
            inventory.AddItemToGrid(itemToAdd, tempoGrid);
            LogLoot(itemToAdd);
        }
    }

    /// <summary>
    /// Génère du loot de boss (2-4 items de qualité)
    /// </summary>
    public void GenerateBossLoot(int bossesDefeated)
    {
        // Boss drop toujours des items de haute qualité
        float roll = Random.Range(0f, 100f);
        ItemData itemToAdd = null;

        // 30% Legendary, 70% Epic
        if (roll < 30f && legendaryLoot.Length > 0)
        {
            itemToAdd = legendaryLoot[Random.Range(0, legendaryLoot.Length)];
            LogLoot(itemToAdd, "⭐ LÉGENDAIRE ! ⭐");
        }
        else if (epicLoot.Length > 0)
        {
            itemToAdd = epicLoot[Random.Range(0, epicLoot.Length)];
            LogLoot(itemToAdd, "💎 Épique !");
        }

        if (itemToAdd != null)
        {
            inventory.AddItemToGrid(itemToAdd, tempoGrid);
        }
    }

    /// <summary>
    /// Génère du loot pour la salle au trésor selon la progression
    /// </summary>
    public void GenerateTreasureRoomLoot(int bossesDefeated)
    {
        ItemData itemToAdd = null;
        float roll = Random.Range(0f, 100f);

        // Avant le premier boss
        if (bossesDefeated == 0)
        {
            // 1% Netherite, 10% Diamant, 50% Cuivre, 39% Or
            if (roll < 1f && netheriteItems.Length > 0)
            {
                itemToAdd = netheriteItems[Random.Range(0, netheriteItems.Length)];
                LogLoot(itemToAdd, " NETHERITE ! ");
            }
            else if (roll < 11f && diamondItems.Length > 0)
            {
                itemToAdd = diamondItems[Random.Range(0, diamondItems.Length)];
                LogLoot(itemToAdd, " DIAMANT !");
            }
            else if (roll < 61f && copperItems.Length > 0)
            {
                itemToAdd = copperItems[Random.Range(0, copperItems.Length)];
            }
            else if (goldItems.Length > 0)
            {
                itemToAdd = goldItems[Random.Range(0, goldItems.Length)];
            }
        }
        // Après 1 boss : 50% Or, 20% Diamant, 5% Netherite, 25% Cuivre
        else if (bossesDefeated == 1)
        {
            if (roll < 5f && netheriteItems.Length > 0)
            {
                itemToAdd = netheriteItems[Random.Range(0, netheriteItems.Length)];
                LogLoot(itemToAdd, " NETHERITE ! ");
            }
            else if (roll < 25f && diamondItems.Length > 0)
            {
                itemToAdd = diamondItems[Random.Range(0, diamondItems.Length)];
                LogLoot(itemToAdd, " DIAMANT !");
            }
            else if (roll < 75f && goldItems.Length > 0)
            {
                itemToAdd = goldItems[Random.Range(0, goldItems.Length)];
            }
            else if (copperItems.Length > 0)
            {
                itemToAdd = copperItems[Random.Range(0, copperItems.Length)];
            }
        }
        // Après 2 boss : 30% Diamant, 10% Netherite, 40% Or, 20% Cuivre
        else if (bossesDefeated == 2)
        {
            if (roll < 10f && netheriteItems.Length > 0)
            {
                itemToAdd = netheriteItems[Random.Range(0, netheriteItems.Length)];
                LogLoot(itemToAdd, " NETHERITE ! ");
            }
            else if (roll < 40f && diamondItems.Length > 0)
            {
                itemToAdd = diamondItems[Random.Range(0, diamondItems.Length)];
                LogLoot(itemToAdd, " DIAMANT !");
            }
            else if (roll < 80f && goldItems.Length > 0)
            {
                itemToAdd = goldItems[Random.Range(0, goldItems.Length)];
            }
            else if (copperItems.Length > 0)
            {
                itemToAdd = copperItems[Random.Range(0, copperItems.Length)];
            }
        }
        // Après 3 boss : 40% Diamant, 25% Netherite, 25% Or, 10% Cuivre
        else if (bossesDefeated == 3)
        {
            if (roll < 25f && netheriteItems.Length > 0)
            {
                itemToAdd = netheriteItems[Random.Range(0, netheriteItems.Length)];
                LogLoot(itemToAdd, " NETHERITE ! ");
            }
            else if (roll < 65f && diamondItems.Length > 0)
            {
                itemToAdd = diamondItems[Random.Range(0, diamondItems.Length)];
                LogLoot(itemToAdd, " DIAMANT !");
            }
            else if (roll < 90f && goldItems.Length > 0)
            {
                itemToAdd = goldItems[Random.Range(0, goldItems.Length)];
            }
            else if (copperItems.Length > 0)
            {
                itemToAdd = copperItems[Random.Range(0, copperItems.Length)];
            }
        }
        // Après 4 boss : 50% Netherite, 30% Diamant, 20% Or
        else if (bossesDefeated == 4)
        {
            if (roll < 50f && netheriteItems.Length > 0)
            {
                itemToAdd = netheriteItems[Random.Range(0, netheriteItems.Length)];
                LogLoot(itemToAdd, " NETHERITE ! ");
            }
            else if (roll < 80f && diamondItems.Length > 0)
            {
                itemToAdd = diamondItems[Random.Range(0, diamondItems.Length)];
                LogLoot(itemToAdd, " DIAMANT !");
            }
            else if (goldItems.Length > 0)
            {
                itemToAdd = goldItems[Random.Range(0, goldItems.Length)];
            }
        }
        // Après 5+ boss : 60% Netherite, 25% Diamant, 15% Or
        else
        {
            if (roll < 60f && netheriteItems.Length > 0)
            {
                itemToAdd = netheriteItems[Random.Range(0, netheriteItems.Length)];
                LogLoot(itemToAdd, " NETHERITE ! ");
            }
            else if (roll < 85f && diamondItems.Length > 0)
            {
                itemToAdd = diamondItems[Random.Range(0, diamondItems.Length)];
                LogLoot(itemToAdd, " DIAMANT !");
            }
            else if (goldItems.Length > 0)
            {
                itemToAdd = goldItems[Random.Range(0, goldItems.Length)];
            }
        }

        if (itemToAdd != null)
        {
            inventory.AddItemToGrid(itemToAdd, tempoGrid);
        }
        else
        {
            Debug.LogWarning("Aucun loot disponible pour cette configuration de salle au trésor!");
        }
    }

    /// <summary>
    /// Affiche un message de loot dans le log
    /// </summary>
    private void LogLoot(ItemData item, string suffix = "")
    {
        if (playerManager == null || item == null) return;

        string colorHex = ColorUtility.ToHtmlStringRGB(item.GetRarityColor());
        string message = $" <color=#{colorHex}>{item.itemName}</color> {suffix}";

        playerManager.AddLog(message, "white");
    }
}