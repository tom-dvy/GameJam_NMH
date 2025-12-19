using System;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class InventoryController : MonoBehaviour
{
    public Inventory inventory { get; private set; }

    [Header("Grid References")]
    [Tooltip("Inventaire temporaire où les items apparaissent")]
    public InventoryGrid temporaryInventoryGrid;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // DEBUG: Afficher la grille sous la souris
            if (inventory.gridOnMouse == null)
            {
                Debug.LogWarning("Aucune grille détectée sous la souris ! Vérifie que les grilles ont un component Image avec Raycast Target activé.");
                return;
            }

            Debug.Log($"Grille sous la souris: {inventory.gridOnMouse.gameObject.name}");

            // Check if mouse is inside a any grid.
            if (!inventory.ReachedBoundary(inventory.GetSlotAtMouseCoords(), inventory.gridOnMouse))
            {
                if (inventory.selectedItem)
                {
                    Debug.Log($"Tentative de placement de l'item type: {inventory.selectedItem.data.itemType}");

                    Item oldSelectedItem = inventory.selectedItem;
                    Item overlapItem = inventory.GetItemAtMouseCoords();

                    if (overlapItem != null)
                    {
                        inventory.SwapItem(overlapItem, oldSelectedItem);
                    }
                    else
                    {
                        inventory.MoveItem(oldSelectedItem);
                    }
                }
                else
                {
                    SelectItemWithMouse();
                }
            }
            else
            {
                Debug.LogWarning("Position hors limites de la grille");
            }
        }

        // Remove an item from the inventory
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            RemoveItemWithMouse();
        }

        // Generates a random item in the inventory
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Ajoute l'item dans l'inventaire temporaire spécifique
            if (temporaryInventoryGrid != null)
            {
                inventory.AddItemToGrid(
                    inventory.itemsData[UnityEngine.Random.Range(0, inventory.itemsData.Length)],
                    temporaryInventoryGrid
                );
            }
            else
            {
                Debug.LogWarning("Temporary Inventory Grid non assigné ! Glisse la grille temporaire dans l'Inspector.");
                // Comportement par défaut si pas configuré
                inventory.AddItem(inventory.itemsData[UnityEngine.Random.Range(0, inventory.itemsData.Length)]);
            }
        }

        if (inventory.selectedItem != null)
        {
            MoveSelectedItemToMouse();

            if (Input.GetKeyDown(KeyCode.R))
            {
                inventory.selectedItem.Rotate();
            }
        }
    }

    /// <summary>
    /// Select a new item in the inventory.
    /// </summary>
    private void SelectItemWithMouse()
    {
        Item item = inventory.GetItemAtMouseCoords();

        if (item != null)
        {
            inventory.SelectItem(item);
        }
    }

    /// <summary>
    /// Removes the item from the inventory that the mouse is hovering over.
    /// </summary>
    private void RemoveItemWithMouse()
    {
        Item item = inventory.GetItemAtMouseCoords();

        if (item != null)
        {
            inventory.RemoveItem(item);
        }
    }

    /// <summary>
    /// Moves the currently selected object to the mouse.
    /// </summary>
    private void MoveSelectedItemToMouse()
    {
        // Position simplifié - l'item suit directement la souris
        inventory.selectedItem.rectTransform.position = Input.mousePosition;
    }
}