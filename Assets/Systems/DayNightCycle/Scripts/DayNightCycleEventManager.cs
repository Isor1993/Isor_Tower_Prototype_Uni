/*****************************************************************************
* Project : Isors Tower Prototype
* File    : DayNightCycleEventManager.cs
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
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycleEventManager : MonoBehaviour
{
    private List<IDayNightListener> _subscribers = new List<IDayNightListener>();
    private DayNightCycle _dayNightCycle;

    private void Awake()
    {
        _dayNightCycle = FindAnyObjectByType<DayNightCycle>();

        if (_dayNightCycle == null)
        {
#if (UNITY_EDITOR)
            Debug.LogError("DayNightCycle instance not found.");
#endif
            enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        if (_dayNightCycle == null)
        {
            return;
        }

        _dayNightCycle.OnDayPhaseChanged += HandlePhaseChanged;
    }

    private void OnDisable()
    {
        if (_dayNightCycle == null)
        {
            return;
        }

        _dayNightCycle.OnDayPhaseChanged -= HandlePhaseChanged;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="previous"></param>
    /// <param name="current"></param>
    private void HandlePhaseChanged(DayPhase previous, DayPhase current)
    {
        foreach (IDayNightListener listener in _subscribers)
        {
            listener.OnDayPhaseChanged(previous, current);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="listener"></param>
    public void Subscribe(IDayNightListener listener)
    {
        if (listener == null || _subscribers.Contains(listener))
        {
            return;
        }

        _subscribers.Add(listener);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="listener"></param>
    public void Unsubscribe(IDayNightListener listener)
    {
        if (listener == null || !_subscribers.Contains(listener))
        {
            return;
        }

        _subscribers.Remove(listener);
    }
}