/*****************************************************************************
* Project : Isors Tower Prototype
* File    : SheepHealth.cs
* Date    : 20.02.2026
* Author  : Eric Rosenberg
*
* Description :
* Manages the health system for a sheep entity.
* Loads health-related values from the SheepSettings ScriptableObject,
* provides access to the current and maximum health values, and handles
* damage, healing, and life state checks.
*
* History :
* 20.02.2026 ER Created
******************************************************************************/
using System;
using UnityEngine;

/// <summary>
/// Controls the health values of a sheep and provides methods for taking damage,
/// healing, restoring health, and notifying other systems when the sheep dies.
/// </summary>
public class SheepHealth : MonoBehaviour
{
    [Tooltip("ScriptableObject that contains the base health configuration for this sheep.")]
    [SerializeField] SheepSettings settings;

    private int _maxHealth;
    private int _currentHealth;

    /// <summary>
    /// Raised once when the sheep's health reaches zero.
    /// </summary>
    public event Action OnDied;    

    /// <summary>
    /// Gets the current health value of the sheep.
    /// </summary>
    public int CurrentHealth => _currentHealth;

    /// <summary>
    /// Gets the maximum health value of the sheep.
    /// </summary>
    public int MaxHealth => _maxHealth;

    /// <summary>
    /// Indicates whether the sheep is alive.
    /// Returns true if current health is greater than zero.
    /// </summary>
    public bool IsAlive => _currentHealth > 0;

    private void Awake()
    {
        SetBaseValues();
    }

    /// <summary>
    /// Loads initial health configuration from the SheepSettings ScriptableObject.
    /// Sets maximum health and initializes current health.
    /// </summary>
    private void SetBaseValues()
    {
        if(settings=null)
        {
            Debug.LogError($"{name}: No SheepSettings assigned.");
            return;
        }
        _maxHealth = settings.MaxHealth;
        _currentHealth = _maxHealth;
    }

    /// <summary>
    /// Applies damage to the sheep if it is alive and the damage amount is valid.
    /// Clamps the current health value to zero and triggers death when health is depleted.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    public void TakeDamage(int damage)
    {
        if (!IsAlive)
            return;
        if (damage <= 0)
            return;
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    /// <summary>
    /// Restores health to the sheep if it is alive and the healing amount is valid.
    /// Clamps the current health value to the maximum health value.
    /// </summary>
    /// <param name="heal">The amount of health to restore.</param>
    public void Heal(int heal)
    {
        if (!IsAlive)
            return;
        if (heal <= 0)
            return;

        _currentHealth += heal;

        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }

    /// <summary>
    /// Restores the sheep's current health value to its maximum health value.
    /// </summary>
    public void RestoreFullHealth()
    {
        _currentHealth = _maxHealth;
    }

    /// <summary>
    /// Invokes the death event to notify subscribed systems that this sheep has died.
    /// </summary>
    private void Die()
    {
        OnDied?.Invoke();
    }
}