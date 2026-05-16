/*****************************************************************************
* Project : Isors Tower Prototype
* File    : SheepHunger.cs
* Date    : 20.02.2026
* Author  : Eric Rosenberg
*
* Description :
* Handles the hunger system for a sheep entity.
* Loads hunger-related values from the SheepSettings ScriptableObject,
* decreases the current hunger value over time with a timer-based tick system,
* and provides state properties to determine whether the sheep is hungry or full.
*
* History :
* 20.02.2026 ER Created
******************************************************************************/
using UnityEngine;

/// <summary>
/// Controls the hunger values of a sheep and updates them over time.
/// </summary>
public class SheepHunger : MonoBehaviour
{
    [Tooltip("ScriptableObject that contains the base hunger configuration for this sheep.")]
    [SerializeField] SheepSettings settings;

    private int _maxHunger;
    private int _currentHunger;
    private float _hungerTickInterval;
    private int _hungerTick;
    private int _hungerTreshold;   
    private float _hungerTimer;

    /// <summary>
    /// Indicates whether the sheep is currently hungry.
    /// Returns true when the current hunger value is below the defined hunger threshold.
    /// </summary>
    public bool IsHungry => _currentHunger < _hungerTreshold;

    /// <summary>
    /// Indicates whether the sheep currently has enough hunger value to be considered full.
    /// Returns true when the current hunger value is greater than or equal to the hunger threshold.
    /// </summary>
    public bool IsFull => _currentHunger >= _hungerTreshold;

    private void Awake()
    {
        SetBaseValue();
    }

    /// <summary>
    /// Increases the current hunger value when the sheep eats.
    /// </summary>
    public void Eat()
    {
        //TODOO noch essenslogik machen per zeit
        _currentHunger += 20;
    }

    /// <summary>
    /// Loads the initial hunger configuration from the SheepSettings ScriptableObject.
    /// Sets the maximum hunger, current hunger, tick interval, tick amount, and hunger threshold.
    /// </summary>
    private void SetBaseValue()
    {
        _maxHunger = settings.HungerThreshold;
        _currentHunger = _maxHunger;
        _hungerTickInterval = settings.HungerTickInterval;
        _hungerTick = settings.HungerTick;
        _hungerTreshold = settings.HungerThreshold;
    }

    private void Update()
    {
        _hungerTimer += Time.deltaTime;
        if(_hungerTimer>=_hungerTickInterval)
        {
            _hungerTimer = 0;
            ApllyHungerTick();
        }     
    }

    /// <summary>
    /// Reduces the current hunger value by the configured hunger tick amount.
    /// Ensures that the hunger value does not drop below zero.
    /// </summary>
    private void ApllyHungerTick()
    {
        _currentHunger -= _hungerTick;
        if (_currentHunger < 0)
        {
            _currentHunger = 0;
        }
    } 
}
