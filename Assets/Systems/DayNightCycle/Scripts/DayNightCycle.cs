/*****************************************************************************
* Project : Isors Tower Prototype
* File    : DayNightCycle.cs
* Date    : xx.xx.2026
* Author  : Eric Rosenberg
*
* Description :
* 
* 
* 
*
* History :
* xx.xx.2026 ER Created
******************************************************************************/
using System;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public enum DayPhase
{
    Morning,
    Afternoon,
    Evening,
    Night
}
public class DayNightCycle : MonoBehaviour
{

    [Header("Phase Settings")]
    [Tooltip(""),Range(1,24)]
    [SerializeField] private int _morningStartHour = 6;
    [Tooltip(""), Range(1, 24)]
    [SerializeField] private int _afternoonStartHour = 12;
    [Tooltip(""), Range(1, 24)]
    [SerializeField] private int _eveningStartHour = 18;
    [Tooltip(""), Range(1, 24)]
    [SerializeField] private int _nightStartHour = 24;

    [Header("Debug")]
    [Tooltip("")]
    [SerializeField] private DayPhase _currentPhase;
    [Tooltip("")]
    [SerializeField] private float _dayProgress;

    /// <summary>
    /// 
    /// </summary>
    public DayPhase CurrentDayPhase => _currentPhase;

    /// <summary>
    /// 
    /// </summary>
    public float DayProgress => _dayProgress;

    /// <summary>
    /// 
    /// </summary>
    public event Action<DayPhase, DayPhase> OnDayPhaseChanged;

    private DayPhase _previousPhase;
    private IngameTime _ingameTime;

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        _ingameTime = IngameTime.Instance;
        if(_ingameTime==null)
        {
            Debug.LogError($"{IngameTime.Instance} is missing!");
            enabled = false;
            return;
        }

        UpdateDayProgress();
        _currentPhase=CalculateCurrentPhase();
        _previousPhase = _currentPhase;
    }

    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        UpdateDayProgress();
        UpdateCurrentPhase();
    }   

    /// <summary>
    /// 
    /// </summary>
    private void UpdateDayProgress()
    {
        _dayProgress = (float)_ingameTime.CurrentTotalSeconds / _ingameTime.SecondsPerDay;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private DayPhase CalculateCurrentPhase()
    {
        int currentHour = _ingameTime.Hours;

       

        if (currentHour >= _morningStartHour &&
            currentHour < _afternoonStartHour)
        {
            return DayPhase.Morning;
        }
        if (currentHour >= _afternoonStartHour &&
                 currentHour < _eveningStartHour)
        {
            return DayPhase.Afternoon;
        }
        if (currentHour >= _eveningStartHour &&
                 currentHour < _nightStartHour)
        {
            return DayPhase.Evening;
        }        

        return DayPhase.Night;
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdateCurrentPhase()
    {
        DayPhase newPhase=CalculateCurrentPhase();

        if(_currentPhase==newPhase)
        {
            return;
        }
        _previousPhase = _currentPhase;
        _currentPhase = newPhase;

        OnDayPhaseChanged?.Invoke(_previousPhase, _currentPhase);

#if UNITY_EDITOR
        Debug.Log($"Phase Changed: {_previousPhase} -> {_currentPhase}");
#endif
    }
}