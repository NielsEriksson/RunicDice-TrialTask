
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private TMP_Text amountTxt;
    [SerializeField] private Image Icon;

    private int amount;
    private ItemData item;

    public bool occupied;

    public ItemData Item => item;
    public int Amount => amount;

    private void Awake()
    {
        Icon.enabled = false;
        amountTxt.text = "";
    }

    public bool ContainsItem(ItemData newItem)
    {
        return occupied && item == newItem;
    }

    public bool CanStack(ItemData newItem)
    {
        return ContainsItem(newItem) &&
               item.isStackable &&
               amount < item.maxStackSize;
    }

    public void AddItem(ItemData newItem)
    {
        occupied = true;
        item = newItem;
        amount = 1;

        Icon.sprite = item.icon;
        Icon.enabled = true;

        UpdateUI();
    }

    public void AddItemStack()
    {
        if (!occupied || !item.isStackable)
            return;

        if (amount >= item.maxStackSize)
            return;

        amount++;
        UpdateUI();
    }

    public void RemoveItem()
    {
        occupied = false;
        item = null;
        amount = 0;

        Icon.sprite = null;
        Icon.enabled = false;

        UpdateUI();
    }

    public void RemoveItemStack()
    {
        if (!occupied)
            return;

        amount--;

        if (amount <= 0)
        {
            RemoveItem();
            return;
        }

        UpdateUI();
    }

    public void UseItem()
    {
        if (!occupied || item == null)
            return;

        if (item.UseItem())
        {
            RemoveItemStack();
        }
    }

    private void UpdateUI()
    {
        amountTxt.text = amount >0  ? amount.ToString() : "";
    }
}
