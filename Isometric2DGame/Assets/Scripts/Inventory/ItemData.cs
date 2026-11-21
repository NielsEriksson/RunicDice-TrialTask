
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Information")]
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;

    [Header("Item Settings")]
    public bool isStackable = true;
    public int maxStackSize = 99;

    [Header("Use Effects")]
    public List<ItemEffect> useEffects = new List<ItemEffect>();

    public bool CanUse()
    {
        if (useEffects.Count == 0)
            return false;

        foreach (ItemEffect effect in useEffects)
        {
            if (effect == null || !effect.CanApply())
                return false;
        }

        return true;
    }

    public bool UseItem()
    {
        if (!CanUse())
            return false;

        foreach (ItemEffect effect in useEffects)
        {
            effect.Apply();
        }

        return true;
    }
}
