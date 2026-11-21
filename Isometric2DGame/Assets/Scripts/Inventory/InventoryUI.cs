
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private List<GameObject> InventorySlots = new List<GameObject>();
    [SerializeField] private CanvasGroup inventoryCanvasGroup;

    [Header("Player")]
    [SerializeField] private GameObject player;

    [Header("Input")]
    [SerializeField] private InputActionReference inventoryToggleAction;

    private bool inventoryOpen = false;

    private void Awake()
    {
        SetInventoryVisibility(false);
    }

    private void OnEnable()
    {
        if (inventoryToggleAction != null)
        {
            inventoryToggleAction.action.performed += OnInventoryToggle;
            inventoryToggleAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (inventoryToggleAction != null)
        {
            inventoryToggleAction.action.performed -= OnInventoryToggle;
            inventoryToggleAction.action.Disable();
        }
    }

    private void OnInventoryToggle(InputAction.CallbackContext context)
    {
        inventoryOpen = !inventoryOpen;
        SetInventoryVisibility(inventoryOpen);
    }

    private void SetInventoryVisibility(bool visible)
    {
        if (inventoryCanvasGroup == null)
            return;

        inventoryCanvasGroup.alpha = visible ? 1f : 0f;
        inventoryCanvasGroup.interactable = visible;
        inventoryCanvasGroup.blocksRaycasts = visible;
    }

    public int GetNextAvailableSlotIndex()
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            InventorySlotUI slot = InventorySlots[i].GetComponent<InventorySlotUI>();

            if (!slot.occupied)
                return i;
        }

        return -1;
    }

    public bool AddItem(ItemData item)
    {
        if (item == null)
            return false;

        // Try stacking first.
        if (item.isStackable)
        {
            foreach (GameObject slotObject in InventorySlots)
            {
                InventorySlotUI slot = slotObject.GetComponent<InventorySlotUI>();

                if (slot.CanStack(item))
                {
                    slot.AddItemStack();
                    return true;
                }
            }
        }

        // Find an empty slot.
        int index = GetNextAvailableSlotIndex();

        if (index == -1)
        {
            Debug.Log("Inventory is full!");
            return false;
        }

        InventorySlots[index].GetComponent<InventorySlotUI>().AddItem(item);

        return true;
    }

    public void RemoveItem(int index)
    {
        if (index < 0 || index >= InventorySlots.Count)
            return;

        InventorySlots[index].GetComponent<InventorySlotUI>().RemoveItem();
    }

    public void UseItem(int index)
    {
        if (index < 0 || index >= InventorySlots.Count)
            return;

        InventorySlots[index].GetComponent<InventorySlotUI>().UseItem();
    }
}
