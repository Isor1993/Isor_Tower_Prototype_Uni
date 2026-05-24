/*****************************************************************************
* Project : Isors Tower Prototype
* File    : PatrolState.cs
* Date    : 20.02.2026
* Author  : Eric Rosenberg
*
* Description :
* Represents the patrol behavior state of a sheep.
* Moves the sheep between random valid patrol positions around the herd anchor
* for a limited patrol duration. The state reacts to threats, sleep state,
* and hunger by transitioning into alert, sleeping, or eating behavior.
*
* History :
* 20.02.2026 ER Created
******************************************************************************/
using UnityEngine;

/// <summary>
/// State in which the sheep patrols around the herd area and periodically receives new patrol targets.
/// </summary>
public class PatrolState : SheepStateBase
{
    private readonly Timer _patrolTimer = new Timer();
    private readonly Timer _newTargetTimer = new Timer();

    private float _patrolTime;
    private float _newTargetTime;
    private Vector3 _newPos;

    public PatrolState(Sheep sheep, SheepFSM fSM) : base(sheep, fSM)
    {
    }

    /// <summary>
    /// Enters the patrol state, randomizes the patrol duration,
    /// resets the patrol timers, and assigns the first patrol target.
    /// </summary>
    public override void Enter()
    {
        Debug.Log($"{GetType().Name}: Change state => {nameof(PatrolState)}");

        _patrolTime = Random.Range(Settings.PatrolTimeMin, Settings.PatrolTimeMax);
        _newTargetTime = Settings.PatrolNewTargetTime;

        _patrolTimer.Reset();
        _newTargetTimer.Reset();

        SetNewPatrolTarget();
    }

    /// <summary>
    /// Updates the patrol behavior.
    /// The sheep switches to alert when a threat is detected, to sleeping when it is asleep,
    /// to eating when it is hungry, or to idle when the patrol duration is finished.
    /// It also assigns a new patrol target after the configured target update interval
    /// once the current destination has been reached.
    /// </summary>
    public override void Tick()
    {
        _newTargetTimer.Tick(Time.deltaTime);
        _patrolTimer.Tick(Time.deltaTime);

        if (Sheep.Sense.HasThreat)
        {
            FSM.ChangeState(new OnAlertState(Sheep, FSM));
            return;
        }
        if(Sheep.Sense.IsPlayerInTameRange&&Sheep.IsTamed)
        {
            FSM.ChangeState(new FollowPlayerState(Sheep, FSM));
            return;
        }

        if (Sheep.IsAsleep)
        {
            FSM.ChangeState(new SleepingState(Sheep, FSM));
            return;
        }

        if (Sheep.Hunger.IsHungry)
        {
            FSM.ChangeState(new EatingState(Sheep, FSM));
            return;
        }

        if (_patrolTimer.IsFinished(_patrolTime))
        {
            FSM.ChangeState(new IdleState(Sheep, FSM));
            return;
        }

        if (_newTargetTimer.IsFinished(_newTargetTime))
        {
            if (Sheep.Move.HasReachedDestination())
            {
                _newTargetTimer.Reset();
                SetNewPatrolTarget();
            }
        }
    }

    /// <summary>
    /// Exits the patrol state.
    /// </summary>
    public override void Exit()
    {

    }


    /// <summary>
    /// Requests a random patrol position from the herd manager, validates it on the NavMesh,
    /// and moves the sheep toward the resulting valid target position.
    /// </summary>
    private void SetNewPatrolTarget()
    {
        _newPos = Sheep.HerdManager.GetRandomPatrolPosition();

        if (!Sheep.Move.TryGetValidTargetPosition(_newPos, out Vector3 validPos))
        {
            Debug.LogWarning($"{Sheep.name}: Could not find valid patrol target.");
            return;
        }

        _newPos = validPos;
        Sheep.Move.MoveTo(_newPos);
    }
}