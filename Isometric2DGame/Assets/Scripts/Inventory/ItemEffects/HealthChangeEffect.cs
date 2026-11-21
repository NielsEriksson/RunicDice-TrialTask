
using UnityEngine;

[CreateAssetMenu(fileName = "HealthChange", menuName = "Inventory/Effects/Change Health")]
public class HealthChangeEffect : ItemEffect
{
    [SerializeField] private int healthChanged = 25;

    public override bool CanApply()
    {
        PlayerHealth health = FindAnyObjectByType<PlayerHealth>();

        if (health == null)
            return false;

        if (healthChanged > 0)
        {
            // Can only restore health if not at maximum.
            return health.GetHealthPerc() < 1f;
        }

        if (healthChanged < 0)
        {
            // Can only remove health if the player is alive.
            return health.GetHealthPerc() > 0f;
        }

        return false;
    }

    public override void Apply()
    {
        PlayerHealth health = FindAnyObjectByType<PlayerHealth>();

        if (health == null)
            return;

        health.ChangeHealth(healthChanged);

        if (healthChanged > 0)
        {
            Debug.Log("Restored " + healthChanged + " health!");
        }
        else if (healthChanged < 0)
        {
            Debug.Log("Removed " + Mathf.Abs(healthChanged) + " health!");
        }
    }
}
