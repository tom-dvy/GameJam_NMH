using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InventoryGrid))]
public class GridVisualizer : MonoBehaviour
{
    [Header("Visual Settings")]
    public Color cellColor = new Color(0.2f, 0.2f, 0.2f, 1f); // Gris foncé
    public Color borderColor = new Color(0.4f, 0.4f, 0.4f, 1f); // Gris clair
    public float borderWidth = 2f;

    private InventoryGrid grid;
    private GameObject cellsContainer;

    private void Start()
    {
        grid = GetComponent<InventoryGrid>();
        CreateGridCells();
    }

    private void CreateGridCells()
    {
        // Conteneur pour les cellules
        cellsContainer = new GameObject("GridCells");
        cellsContainer.transform.SetParent(transform, false);

        RectTransform containerRect = cellsContainer.AddComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0, 1); // Ancre en haut à gauche
        containerRect.anchorMax = new Vector2(0, 1);
        containerRect.pivot = new Vector2(0, 1);
        containerRect.anchoredPosition = Vector2.zero;
        containerRect.sizeDelta = new Vector2(
            grid.gridSize.x * InventorySettings.slotSize.x,
            grid.gridSize.y * InventorySettings.slotSize.y
        );

        // Créer chaque cellule
        for (int y = 0; y < grid.gridSize.y; y++)
        {
            for (int x = 0; x < grid.gridSize.x; x++)
            {
                CreateCell(x, y);
            }
        }
    }

    private void CreateCell(int x, int y)
    {
        GameObject cellObj = new GameObject($"Cell_{x}_{y}");
        cellObj.transform.SetParent(cellsContainer.transform, false);

        // Ajouter l'image de fond
        Image cellImage = cellObj.AddComponent<Image>();
        cellImage.color = cellColor;

        // Configuration du RectTransform - identique à la logique de positionnement des items
        RectTransform rect = cellObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1); // Ancre en haut à gauche
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0.5f, 0.5f); // Pivot au centre

        // Position exactement comme IndexToInventoryPosition
        Vector2 position = new Vector2(
            x * InventorySettings.slotSize.x + InventorySettings.slotSize.x / 2f,
            -(y * InventorySettings.slotSize.y + InventorySettings.slotSize.y / 2f)
        );

        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(
            InventorySettings.slotSize.x - borderWidth,
            InventorySettings.slotSize.y - borderWidth
        );

        // Ajouter une bordure (optionnel)
        CreateCellBorder(cellObj);
    }

    private void CreateCellBorder(GameObject parent)
    {
        GameObject borderObj = new GameObject("Border");
        borderObj.transform.SetParent(parent.transform, false);

        Image borderImage = borderObj.AddComponent<Image>();
        borderImage.color = borderColor;

        RectTransform rect = borderObj.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        rect.anchoredPosition = Vector2.zero;

        // Créer un effet de bordure en mettant l'image de fond légèrement plus grande
        rect.offsetMin = new Vector2(-borderWidth / 2, -borderWidth / 2);
        rect.offsetMax = new Vector2(borderWidth / 2, borderWidth / 2);

        // Mettre la bordure derrière le fond
        borderObj.transform.SetAsFirstSibling();
    }
}