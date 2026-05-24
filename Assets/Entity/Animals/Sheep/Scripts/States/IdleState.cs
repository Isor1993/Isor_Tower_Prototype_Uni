/*****************************************************************************
* Project : Isors Tower Prototype
* File    : IdleState.cs
* Date    : 20.02.2026
* Author  : Eric Rosenberg
*
* Description :
* Represents the idle behavior state of a sheep.
* Keeps the sheep waiting for a configured idle duration while continuously
* checking for threats, nearby players, hunger, and regroup conditions.
* Transitions to alert, eating, or regroup behavior when the corresponding
* condition is met.
*
* History :
* 20.02.2026 ER Created
******************************************************************************/
using UnityEngine;

/// <summary>
/// State in which the sheep remains idle until danger, hunger, player presence,
/// or the idle timer causes a transition to another state.
/// </summary>
public class IdleState : SheepStateBase
{
    private readonly Timer _timer = new Timer();   

    public IdleState(Sheep sheep, SheepFSM fSM) : base(sheep, fSM)
    {

    }

    /// <summary>
    /// Enters the idle state and resets the internal idle timer.
    /// </summary>
    public override void Enter()
    {
        Debug.Log($"{GetType().Name}: Change state => {nameof(IdleState)}");

        _timer.Reset();
    }

    /// <summary>
    /// Updates the idle timer and checks for conditions that should transition
    /// the sheep into alert, eating, or regroup behavior.
    /// </summary>
    public override void Tick()
    {
        _timer.Tick(Time.deltaTime);

        if (Sheep.Sense.HasThreat)
        {

            FSM.ChangeState(new OnAlertState(Sheep, FSM));
            return;
        }
        if(Sheep.Sense.HasPlayerInRange)
        {
            FSM.ChangeState(new OnAlertState(Sheep,FSM));
            return;
        }
        if (Sheep.Hunger.IsHungry)
        {
            FSM.ChangeState(new EatingState(Sheep, FSM));
            return;
        }
        if (_timer.IsFinished(Settings.IdleTime))
        {            
            FSM.ChangeState(new RegroupState(Sheep, FSM));
            return;
        }
    }

    /// <summary>
    /// Exits the idle state.
    /// </summary>
    public override void Exit()
    {
           
    }
}
