/*****************************************************************************
* Project : Isors Tower Prototype
* File    : IdayNightListener.cs
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
using UnityEngine;

public interface IDayNightListener
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="previousPhase"></param>
    /// <param name="currentPhase"></param>
    public void OnDayPhaseChanged(DayPhase previousPhase, DayPhase currentPhase);
}