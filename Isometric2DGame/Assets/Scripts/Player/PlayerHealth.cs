using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
  [SerializeField]  private int health;
    private int maxHealth;
    public static event Action<int, int> OnHealthChanged;
    public void Start()
    {
        maxHealth = health;
    }
    public void ChangeHealth(int healthToAdd)
    {
        health = Mathf.Clamp(health + healthToAdd, 0, maxHealth);
        OnHealthChanged.Invoke(maxHealth, health);
    }
    public float GetHealthPerc()
    {
        return health /(float) maxHealth;
    }
}
