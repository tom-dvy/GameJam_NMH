using UnityEngine;

public class LootManager : MonoBehaviour
{
    [Header("Références")]
    public Inventory inventory;
    public InventoryGrid tempoGrid;

    [Header("Configuration du Loot")]
    public ItemData[] possibleLoots;

    public void GenerateLoot()
    {
        if (possibleLoots.Length == 0) return;

        ItemData randomItemData = possibleLoots[Random.Range(0, possibleLoots.Length)];
        inventory.AddItemToGrid(randomItemData, tempoGrid);
    }
}