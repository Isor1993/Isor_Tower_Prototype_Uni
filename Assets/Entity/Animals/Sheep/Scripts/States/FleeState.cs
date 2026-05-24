/*****************************************************************************
* Project : Isors Tower Prototype
* File    : FleeState.cs
* Date    : 20.02.2026
* Author  : Eric Rosenberg
*
* Description :
* Represents the flee behavior state of a sheep.
* Moves the sheep away from a detected threat, periodically recalculates a new
* flee target position, and returns the sheep to regroup behavior when the
* threat is gone or the flee destination has been reached.
*
* History :
* 20.02.2026 ER Created
******************************************************************************/
using UnityEngine;

/// <summary>
/// State in which the sheep flees from a given threat transform and updates
/// its flee destination at configured intervals.
/// </summary>
public class FleeState : SheepStateBase
{
    private readonly Transform _threat;
    private readonly Timer _updateTimer = new Timer();   
    public FleeState(Sheep sheep, SheepFSM fSM,Transform threat) : base(sheep, fSM)
    {
        _threat = threat;   
    }

    /// <summary>
    /// Enters the flee state, switches the sheep to flee movement settings,
    /// and starts moving away from the current threat position.
    /// </summary>
    public override void Enter()
    {
        Debug.Log($"{GetType().Name}: Change state => {nameof(FleeState)}");

        _updateTimer.Reset();

        Sheep.Move.SetFleeMovement();

        if (_threat != null)
        {
            Sheep.Move.FleeFrom(_threat.position);
        }
    }

    /// <summary>
    /// Updates the flee behavior.
    /// If the threat is missing or no longer detected after reaching the destination,
    /// the sheep returns to regroup behavior. Otherwise, the flee target is recalculated
    /// at the configured update interval.
    /// </summary>
    public override void Tick()
    {
       
        if (_threat==null)
        {
            Sheep.FSM.ChangeState(new RegroupState(Sheep, FSM));
            return;
        }

        if (Sheep.Move.HasReachedDestination()&& !Sheep.Sense.HasThreat)
        {
            Sheep.FSM.ChangeState(new RegroupState(Sheep, FSM));
            return;
        }
        _updateTimer.Tick(Time.deltaTime);

        if (_updateTimer.IsFinished(Settings.UpdateTickNewPosition)&& Sheep.Move.HasReachedDestination())
        {           
            _updateTimer.Reset();
            Sheep.Move.FleeFrom(_threat.position);
            return;
        }

    }

    /// <summary>
    /// Exits the flee state and restores the sheep's normal walking movement settings.
    /// </summary>
    public override void Exit()
    {
     Sheep.Move.SetWalkMovement();
    }
}