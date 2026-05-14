/*****************************************************************************
* Project : Spielprojekt (K1, S1, S2, S3)
* File    : SheepHunger.cs
* Date    : 20.02.2026
* Author  : Eric Rosenberg
*
* Description :
* Handles the hunger system for a sheep entity.
* Reduces hunger over time using a timer-based tick system.
* Determines whether the sheep is considered hungry
* based on a configurable threshold.
*
* History :
* 20.02.2026 ER Created
******************************************************************************/
using UnityEngine;


public class SheepHunger : MonoBehaviour
{
    [Tooltip("SO SheepSettings comes here.")]
    [SerializeField] SheepSettings settings;

    private int _maxHunger;
    private int _currentHunger;
    private float _hungerTickInterval;
    private int _hungerTick;
    private int _hungerTreshold;   
    private float _hungerTimer;

    /// <summary>
    /// Indicates whether the sheep is currently hungry.
    /// Returns true if the current hunger value is below the defined threshold.
    /// </summary>
    public bool IsHungry => _currentHunger < _hungerTreshold;

    /// <summary>
    /// 
    /// </summary>
    public bool IsFull => _currentHunger >= _hungerTreshold;

    private void Awake()
    {
        SetBaseValue();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hunger">Reduces</param>
    public void Eat()
    {
        //TODOO noch essenslogik machen per zeit
        _currentHunger += 20;
    }

    /// <summary>
    /// Loads initial hunger configuration from the SheepSettings ScriptableObject.
    /// Sets max hunger, current hunger, tick interval,
    /// tick value, and hunger threshold.
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
    /// Reduces the current hunger value by the configured tick amount.
    /// Ensures the hunger value does not drop below zero.
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
