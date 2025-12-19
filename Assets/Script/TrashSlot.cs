using UnityEngine;
using UnityEngine.EventSystems;

public class TrashSlot : MonoBehaviour, IDropHandler
{
    public Inventory inventory;
    public PlayerManager playerManager;

    public void OnDrop(PointerEventData eventData)
    {
        Item item = inventory.selectedItem;

        if (item != null)
        {
            playerManager.AddLog($"Vous avez jeté <color=gray>{item.data.itemName}</color> !", "gray");
            inventory.RemoveItem(item);
        }
    }
}