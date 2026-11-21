
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private bool destroyOnPickup = true;

    public ItemData Data => itemData;

    public string Name => itemData != null ? itemData.itemName : "Unknown";

    public bool PickUpItem()
    {
        if (itemData == null)
        {
            Debug.LogError("[Pickup] No ItemData assigned!");
            return false;
        }

        InventoryUI inventory = FindAnyObjectByType<InventoryUI>();

        if (inventory == null)
        {
            Debug.LogError("[Pickup] No InventoryUI found!");
            return false;
        }

        if (!inventory.AddItem(itemData))
            return false;

        Debug.Log("Picked up " + itemData.itemName);

        if (destroyOnPickup)
            Destroy(gameObject);

        return true;
    }
}
