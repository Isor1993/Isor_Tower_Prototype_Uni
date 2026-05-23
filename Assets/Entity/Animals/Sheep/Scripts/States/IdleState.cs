/*****************************************************************************
* Project : Isors Tower Prototype
* File    : SheepMoveBehaviour.cs
* Date    : 20.02.2026
* Author  : Eric Rosenberg
*
* Description :
* 
* 
* 
* 
*
* History :
* 20.02.2026 ER Created
******************************************************************************/
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class IdleState : SheepStateBase
{
    private readonly Timer _timer = new Timer();
    private readonly SheepStateBase _returnState;

    public IdleState(Sheep sheep, SheepFSM fSM) : base(sheep, fSM)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public override void Enter()
    {
        Debug.Log($"{GetType().Name}: Change state => {nameof(IdleState)}");

        _timer.Reset();
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Tick()
    {
        _timer.Tick(Time.deltaTime);

        if (Sheep.Sense.HasThreat)
        {

            FSM.ChangeState(new OnAlertState(Sheep, FSM));
            return;
        }
        if (Sheep.Hunger.IsHungry)
        {

            Debug.Log("ISHungry");
            FSM.ChangeState(new EatingState(Sheep, FSM));
            return;
        }
        if (_timer.IsFinished(Settings.IdleTime))
        {
            Debug.Log("ISFinished");
            FSM.ChangeState(new RegroupState(Sheep, FSM));
            return;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Exit()
    {
           
    }
}
