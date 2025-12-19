using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class InventoryGrid : MonoBehaviour, IPointerEnterHandler
{
    /// <summary>
    /// Size of slots that the grid will have 1X1
    /// </summary>
    [Header("Grid Config")]
    public Vector2Int gridSize = new(5, 5);

    /// <summary>
    /// Optional: which item types this grid accepts. Empty or null = accepts all types.
    /// Configure this in the inspector per grid (e.g. equipment slots vs loot inventory).
    /// </summary>
    public ItemType[] acceptedTypes;

    /// <summary>
    /// Grid main rect transform reference.
    /// </summary>
    public RectTransform rectTransform;

    /// <summary>
    /// Item array.
    /// </summary>
    public Item[,] items { get; set; }

    /// <summary>
    /// Main inventory reference.
    /// </summary>
    public Inventory inventory { get; private set; }

    /// <summary>
    /// Returns whether this grid accepts the provided item type.
    /// </summary>
    public bool Accepts(ItemType type)
    {
        if (acceptedTypes == null || acceptedTypes.Length == 0) return true;
        for (int i = 0; i < acceptedTypes.Length; i++)
        {
            if (acceptedTypes[i] == type) return true;
        }
        return false;
    }

    private void Awake()
    {
        if (rectTransform != null)
        {
            inventory = GameObject.FindFirstObjectByType<Inventory>();
            InitializeGrid();
        }
        else
        {
            Debug.LogError("(InventoryGrid) RectTransform not found!");
        }
    }

    /// <summary>
    /// Initialize matrices and grid size.
    /// </summary>
    private void InitializeGrid()
    {
        // Set items matrices
        items = new Item[gridSize.x, gridSize.y];

        // Set grid size
        Vector2 size =
            new(
                gridSize.x * InventorySettings.slotSize.x,
                gridSize.y * InventorySettings.slotSize.y
            );
        rectTransform.sizeDelta = size;
    }

    /// <summary>
    /// Add grid as main mouse grid.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventory.gridOnMouse = this;
    }
}