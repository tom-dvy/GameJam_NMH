using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class Item : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// Data of the item referenced in the Item script
    /// </summary>
    public ItemData data { get; set; }

    /// <summary>
    /// Image element responsible for showing the item icon.
    /// </summary>
    public Image icon;

    /// <summary>
    /// Image element responsible for showing the item icon background.
    /// </summary>
    public Image background;

    /// <summary>
    /// Target rotation of the 
    /// </summary>
    private Vector3 rotateTarget;

    /// <summary>
    /// Boolean that indicates whether the item was rotated.
    /// </summary>
    public bool isRotated;

    /// <summary>
    /// Rotation index, used to tell the item when to rotate at the right time.
    /// </summary>
    public int rotateIndex;

    /// <summary>
    /// The indexed position is the position of the item relative to the grid in which the item is located.
    /// </summary>
    public Vector2Int indexPosition { get; set; }

    /// <summary>
    /// Reference of the main inventory to which the script communicates.
    /// </summary>
    public Inventory inventory { get; set; }

    /// <summary>
    /// Reference of the RectTransform.
    /// </summary>
    public RectTransform rectTransform { get; set; }

    /// <summary>
    /// Grid the item is currently in.
    /// </summary>
    public InventoryGrid inventoryGrid { get; set; }

    /// <summary>
    /// Correct position using the rotation ratio.
    /// </summary>
    public SizeInt correctedSize
    {
        get
        { return new(!isRotated ? data.size.width : data.size.height, !isRotated ? data.size.height : data.size.width); }
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        icon.sprite = data.icon;
        background.color = data.backgroundColor;
    }

    /// <summary>
    /// LateUpdate is called every frame, if the Behaviour is enabled.
    /// It is called after all Update functions have been called.
    /// </summary>
    private void LateUpdate()
    {
        UpdateRotateAnimation();
    }

    /// <summary>
    /// Rotates the item to the correct position the player needs.
    /// </summary>
    public void Rotate()
    {
        if (rotateIndex < 3)
        {
            rotateIndex++;
        }
        else if (rotateIndex >= 3)
        {
            rotateIndex = 0;
        }

        UpdateRotation();
    }

    /// <summary>
    /// Reset the rotate index.
    /// </summary>
    public void ResetRotate()
    {
        rotateIndex = 0;
        UpdateRotation();
    }

    /// <summary>
    /// Update rotation movement.
    /// </summary>
    private void UpdateRotation()
    {
        switch (rotateIndex)
        {
            case 0:
                rotateTarget = new(0, 0, 0);
                isRotated = false;
                break;

            case 1:
                rotateTarget = new(0, 0, -90);
                isRotated = true;
                break;

            case 2:
                rotateTarget = new(0, 0, -180);
                isRotated = false;
                break;

            case 3:
                rotateTarget = new(0, 0, -270);
                isRotated = true;
                break;
        }
    }

    /// <summary>
    /// Updates the item rotation animation.
    /// </summary>
    private void UpdateRotateAnimation()
    {
        Quaternion targetRotation = Quaternion.Euler(rotateTarget);

        if (rectTransform.localRotation != targetRotation)
        {
            rectTransform.localRotation = Quaternion.Slerp(
                rectTransform.localRotation,
                targetRotation,
                InventorySettings.rotationAnimationSpeed * Time.deltaTime
            );
        }
    }

    /// <summary>
    /// Called when the mouse enters the item
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Ne pas afficher le tooltip si l'item est sélectionné (en train d'être déplacé)
        if (inventory != null && inventory.selectedItem == this)
        {
            return;
        }

        // Afficher le tooltip
        if (TooltipManager.Instance != null && data != null)
        {
            TooltipManager.Instance.ShowTooltip(data);
        }
    }

    /// <summary>
    /// Called when the mouse exits the item
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        // Cacher le tooltip
        if (TooltipManager.Instance != null)
        {
            TooltipManager.Instance.HideTooltip();
        }
    }
}