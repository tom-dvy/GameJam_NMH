using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InventoryGrid))]
public class GridVisualizer : MonoBehaviour
{
    public Color lineColor = new Color(1, 1, 1, 0.3f); // Blanc semi-transparent
    public float lineWidth = 2f;

    private InventoryGrid grid;
    private GameObject linesContainer;

    private void Start()
    {
        grid = GetComponent<InventoryGrid>();
        CreateGridLines();
    }

    private void CreateGridLines()
    {
        // Conteneur pour les lignes
        linesContainer = new GameObject("GridLines");
        linesContainer.transform.SetParent(transform);
        RectTransform containerRect = linesContainer.AddComponent<RectTransform>();
        containerRect.anchorMin = Vector2.zero;
        containerRect.anchorMax = Vector2.one;
        containerRect.sizeDelta = Vector2.zero;
        containerRect.anchoredPosition = Vector2.zero;

        // Lignes verticales
        for (int x = 0; x <= grid.gridSize.x; x++)
        {
            CreateLine(
                new Vector2(x * InventorySettings.slotSize.x, 0),
                new Vector2(x * InventorySettings.slotSize.x, grid.gridSize.y * InventorySettings.slotSize.y),
                true
            );
        }

        // Lignes horizontales
        for (int y = 0; y <= grid.gridSize.y; y++)
        {
            CreateLine(
                new Vector2(0, y * InventorySettings.slotSize.y),
                new Vector2(grid.gridSize.x * InventorySettings.slotSize.x, y * InventorySettings.slotSize.y),
                false
            );
        }
    }

    private void CreateLine(Vector2 start, Vector2 end, bool isVertical)
    {
        GameObject lineObj = new GameObject("Line");
        lineObj.transform.SetParent(linesContainer.transform);

        Image lineImage = lineObj.AddComponent<Image>();
        lineImage.color = lineColor;

        RectTransform rect = lineObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);

        if (isVertical)
        {
            rect.sizeDelta = new Vector2(lineWidth, end.y - start.y);
            rect.anchoredPosition = new Vector2(start.x, -start.y);
        }
        else
        {
            rect.sizeDelta = new Vector2(end.x - start.x, lineWidth);
            rect.anchoredPosition = new Vector2(start.x, -start.y);
        }
    }
}