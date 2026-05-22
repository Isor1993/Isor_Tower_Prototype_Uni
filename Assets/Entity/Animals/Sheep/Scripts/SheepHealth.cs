/*****************************************************************************
* Project : Isors Tower Prototype
* File    : SheepHealth.cs
* Date    : 20.02.2026
* Author  : Eric Rosenberg
*
* Description :
* Manages the health system for a sheep entity.
* Loads health-related values from SheepSettings, provides access to the
* current and maximum health values, handles typed damage and healing, and
* raises events when the sheep takes damage or dies.
*
* History :
* 20.02.2026 ER Created
******************************************************************************/
using System;
using UnityEngine;

/// <summary>
/// Controls the health values of a sheep and provides damage, healing,
/// full restoration, and death notification functionality.
/// </summary>
public class SheepHealth : MonoBehaviour,IDamagable
{
    [Tooltip("ScriptableObject that contains the base health configuration for this sheep.")]
    [SerializeField] SheepSettings settings;

    [Tooltip("Maximum health value this sheep can have."),Range(1,100)]
    [SerializeField] private int _maxHealth;

    [Tooltip("Current health value of this sheep.")]
    [SerializeField] private int _currentHealth;

    /// <summary>
    /// Raised once when the sheep's health reaches zero.
    /// </summary>
    public event Action OnDied;

    /// <summary>
    /// Raised whenever the sheep receives valid damage.
    /// Provides the applied damage amount and the damage type.
    /// </summary>
    public event Action<int,DamageType> OnDamaged;

    /// <summary>
    /// Gets the current health value of the sheep.
    /// </summary>
    public int CurrentHealth => _currentHealth;

    /// <summary>
    /// Gets the maximum health value of the sheep.
    /// </summary>
    public int MaxHealth => _maxHealth;

    /// <summary>
    /// Indicates whether the sheep is currently alive.
    /// Returns true when the current health value is greater than zero.
    /// </summary>
    public bool IsAlive => _currentHealth > 0;

    private void Awake()
    {
        SetBaseValues();
    }

    /// <summary>
    /// Loads the initial health configuration from the SheepSettings ScriptableObject.
    /// Sets maximum health and initializes current health.
    /// </summary>
    private void SetBaseValues()
    {
        if(settings==null)
        {
            Debug.LogError($"{name}: No SheepSettings assigned.");
            return;
        }
        _maxHealth = settings.MaxHealth;
        _currentHealth = _maxHealth;
    }

    /// <summary>
    /// Applies typed damage to the sheep if it is alive and the damage amount is valid.
    /// Raises the damage event and triggers death when health reaches zero.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    /// <param name="damageType">The type of damage being applied.</param>
    public void TakeDamage(int damage, DamageType damageType)
    {
        if (!IsAlive)
            return;

        if (damage <= 0)
            return;

        _currentHealth -= damage;
        OnDamaged?.Invoke(damage,damageType);

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
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

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