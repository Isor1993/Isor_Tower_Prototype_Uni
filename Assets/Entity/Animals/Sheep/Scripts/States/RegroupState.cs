/*****************************************************************************
* Project : Isors Tower Prototype
* File    : RegroupState.cs
* Date    : 20.02.2026
* Author  : Eric Rosenberg
*
* Description :
* Represents the regroup behavior state of a sheep.
* Moves the sheep either to its herd formation position while the herd is
* moving, or to a random valid regroup position around the herd anchor.
* If no valid target can be found, the sheep returns to patrol behavior.
*
* History :
* 20.02.2026 ER Created
******************************************************************************/
using UnityEngine;

/// <summary>
/// State in which the sheep moves back toward the herd or into its assigned herd formation position.
/// </summary>
public class RegroupState : SheepStateBase,IResumeTargetState
{
    private const int MAX_TRIES = 100;

    private Vector3 targetPos;
    private bool _hasValidTarget;
    private bool _hasResumeTarget;
    private Vector3 _resumeTarget;
    private bool _isFormationTarget;

    public RegroupState(Sheep sheep, SheepFSM fSM) : base(sheep, fSM)
    {
    }

    /// <summary>
    /// Enters the regroup state and searches for a valid regroup target.
    /// If the herd is moving, the sheep tries to move to its assigned formation position.
    /// Otherwise, it searches for a random valid regroup position around the herd anchor.
    /// </summary>
    public override void Enter()
    {
        Debug.Log($"{GetType().Name}:{Sheep.gameObject.name}: Change state => {nameof(RegroupState)}");

        _hasValidTarget = false;

        if (_hasResumeTarget)
        {
            targetPos = _resumeTarget;
            _hasValidTarget = true;
            _hasResumeTarget = false;

            Sheep.Move.MoveTo(targetPos);
            return;
        }

        bool foundTarget = Sheep.IsHerdMoving
            ? TrySetFormationTarget()
            : TrySetRandomRegroupTarget();

        if (!foundTarget)
        {
            Debug.LogWarning($"{Sheep.name}: Did not find a valid regroup position after {MAX_TRIES} tries!");
        }
    }

    /// <summary>
    /// Updates the regroup behavior.
    /// If no valid target exists, the sheep returns to patrol behavior.
    /// While the herd is moving, the state waits until all sheep are in position
    /// before switching to herd movement. Otherwise, the sheep returns to patrol
    /// once it reaches its regroup destination.
    /// </summary>
    public override void Tick()
    {
        if (Sheep.Sense.HasThreat)
        {
            FSM.ChangeState(new OnAlertState(Sheep, FSM));
            return;
        }

        if (!_hasValidTarget)
        {
            FSM.ChangeState(new PatrolState(Sheep, FSM));
            return;
        }

        if (Sheep.IsHerdMoving)
        {
            if (!_isFormationTarget)
            {
                if (!TrySetFormationTarget())
                {
                    Debug.LogWarning($"{Sheep.name}: Could not switch regroup target to formation position.");
                    return;
                }

                return;
            }

            if (Sheep.HerdManager.IsSheepInPosition(Sheep))
            {
                FSM.ChangeState(new HerdMoving(Sheep, FSM));
                return;
            }

            return;
        }

        if (Sheep.Move.HasReachedDestination())
        {
            FSM.ChangeState(new PatrolState(Sheep, FSM));
            return;
        }
    }

    /// <summary>
    /// Exits the regroup state.
    /// </summary>
    public override void Exit()
    {
    }

    public void ResumeTarget(Vector3 target)
    {
        _resumeTarget = target;
        _hasResumeTarget = true;
    }

    private bool TrySetFormationTarget()
    {
        Debug.Log("Format pos");
        targetPos = Sheep.HerdManager.GetFormationPositionForSheep(Sheep);

        if (!Sheep.Move.TryGetValidTargetPosition(targetPos, out Vector3 validPosition))
            return false;

        targetPos = validPosition;
        _hasValidTarget = true;
        _isFormationTarget = true;

        Sheep.Move.MoveTo(targetPos);
        return true;
    }

    private bool TrySetRandomRegroupTarget()
    {
        Debug.Log("Random pos pos");
        for (int i = 0; i < MAX_TRIES; i++)
        {
            targetPos = Sheep.HerdManager.GetRandomRegroupPosition();

            if (Sheep.Move.TryGetValidTargetPosition(targetPos, out Vector3 validPos))
            {
                targetPos = validPos;
                _hasValidTarget = true;
                _isFormationTarget = false;

                Sheep.Move.MoveTo(targetPos);
                return true;
            }
        }

        return false;
    }
}