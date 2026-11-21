
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerPickUp : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference interactAction;

    [Header("Pickup UI")]
    [SerializeField] private TMP_Text pickupText;

    private List<InventoryItem> nearbyItems = new List<InventoryItem>();

    private void OnEnable()
    {
        interactAction.action.performed += OnInteract;
    }

    private void OnDisable()
    {
        interactAction.action.performed -= OnInteract;
    }

    private void Start()
    {
        UpdatePickupText();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("PickUpItem"))
            return;

        InventoryItem item = collision.GetComponent<InventoryItem>();

        if (item != null && !nearbyItems.Contains(item))
        {
            nearbyItems.Add(item);
            UpdatePickupText();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("PickUpItem"))
            return;

        InventoryItem item = collision.GetComponent<InventoryItem>();

        if (item != null)
        {
            nearbyItems.Remove(item);
            UpdatePickupText();
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        PickUpClosestItem();
    }

    private InventoryItem GetClosestItem()
    {
        nearbyItems.RemoveAll(item => item == null);

        InventoryItem closestItem = null;
        float closestDistance = Mathf.Infinity;

        foreach (InventoryItem item in nearbyItems)
        {
            float distance = (item.transform.position - transform.position).sqrMagnitude;

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestItem = item;
            }
        }

        return closestItem;
    }

    private void UpdatePickupText()
    {
        InventoryItem closestItem = GetClosestItem();

        if (closestItem == null)
        {
            pickupText.gameObject.SetActive(false);
            return;
        }

        pickupText.text = "Pick up " + closestItem.Name + " (E)";
        pickupText.gameObject.SetActive(true);
    }

    private void PickUpClosestItem()
    {
        InventoryItem closestItem = GetClosestItem();

        if (closestItem == null)
            return;

        if (closestItem.PickUpItem())
        {
            nearbyItems.Remove(closestItem);
            UpdatePickupText();
        }
    }
}
