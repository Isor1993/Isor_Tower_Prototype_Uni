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
using UnityEngine;

/// <summary>
/// Controls the health values of a sheep and provides methods for taking damage and healing.
/// </summary>
public class SheepHealth : MonoBehaviour
{
    [Tooltip("ScriptableObject that contains the base health configuration for this sheep.")]
    [SerializeField] SheepSettings settings;

    private int _maxHealth;
    private int _currentHealth;

    /// <summary>
    /// Gets the current health value of the sheep.
    /// </summary>
    public int CurrentHealth => _currentHealth;

    /// <summary>
    /// Gets the maximum health value of the sheep.
    /// </summary>
    public int MaxHealth=>_maxHealth;

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
        _maxHealth = settings.MaxHealth;
        _currentHealth = _maxHealth;      
    }

    /// <summary>
    /// Applies damage to the sheep and clamps the current health value to a minimum of zero.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    public void TakeDamage(int damage)
    {
        _currentHealth-=damage;

        if(_currentHealth<=0)
        {
            _currentHealth = 0;            
        }
    }

    /// <summary>
    /// Restores health to the sheep if it is alive and clamps the value to the maximum health.
    /// </summary>
    /// <param name="heal">The amount of health to restore.</param>
    public void Heal(int heal)
    {
        if (!IsAlive) return;

        _currentHealth += heal;

        if (_currentHealth>_maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }
}