using UnityEngine;
using UnityEngine.EventSystems;

public class TrashSlot : MonoBehaviour, IDropHandler
{
    public Inventory inventory;
    public PlayerManager uiManager;

    public void OnDrop(PointerEventData eventData)
    {
        Item item = inventory.selectedItem;

        if (item != null)
        {
            uiManager.AddLog($"Vous avez jeté {item.data.name} ! ", "gray");
            inventory.RemoveItem(item);
        }
    }
}