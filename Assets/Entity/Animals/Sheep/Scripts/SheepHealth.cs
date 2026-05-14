/*****************************************************************************
* Project : Spielprojekt (K1, S1, S2, S3)
* File    : SheepHealth.cs
* Date    : 20.02.2026
* Author  : Eric Rosenberg
*
* Description :
* Manages the health system for a sheep entity.
* Handles damage, healing, and life state checks
* based on values defined in SheepSettings.
*
* History :
* 20.02.2026 ER Created
******************************************************************************/
using UnityEngine;

public class SheepHealth : MonoBehaviour
{
    [Tooltip("SO SheepSettings comes here.")]
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
    /// Applies damage to the sheep.
    /// Reduces current health and clamps it to a minimum of zero.
    /// </summary>
    /// <param name="damage">
    /// The amount of damage to apply.
    /// </param>
    public void TakeDamage(int damage)
    {
        _currentHealth-=damage;

        if(_currentHealth<=0)
        {
            _currentHealth = 0;            
        }
    }

    /// <summary>
    /// Heals the sheep by a specified amount.
    /// Healing only occurs if the sheep is alive.
    /// Clamps the value to the maximum health.
    /// </summary>
    /// <param name="heal">
    /// The amount of health to restore.
    /// </param>
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