/*****************************************************************************
* Project : Isors Tower Prototype
* File    : FollowPlayerState.cs
* Date    : 20.02.2026
* Author  : Eric Rosenberg
*
* Description :
* Represents the follow-player behavior state of a sheep.
* Allows a tamed sheep to follow the currently detected player and, if the
* sheep is the commander, informs the herd manager that the herd is currently
* moving as a group.
*
* History :
* 20.02.2026 ER Created
******************************************************************************/
using UnityEngine;

/// <summary>
/// State in which a tamed sheep follows the detected player.
/// Commander sheep also notify the herd manager when herd movement starts or stops.
/// </summary>
public class FollowPlayerState : SheepStateBase
{
    public FollowPlayerState(Sheep sheep, SheepFSM fSM) : base(sheep, fSM)
    {

    }

    /// <summary>
    /// Enters the follow-player state and marks the herd as moving if this sheep is the commander.
    /// </summary>
    public override void Enter()
    {
        Debug.Log($"{GetType().Name}: Change state => {nameof(FollowPlayerState)}");
        if (Sheep.IsCommander)
        {
            Sheep.HerdManager.SetAllSheepHerdMoving(true);
        }
    }

    /// <summary>
    /// Updates the follow-player behavior.
    /// If the sheep is no longer tamed or the player reference is missing,
    /// the sheep returns to regroup behavior. Otherwise, it follows behind
    /// the detected player while the player remains within sensing range.
    /// </summary>
    public override void Tick()
    {
        if (!Sheep.IsTamed)
        {
            FSM.ChangeState(new RegroupState(Sheep, FSM));
            return;
        }
        if (Sheep.Sense.CurrentPlayer == null)
        {
            FSM.ChangeState(new RegroupState(Sheep, FSM));
            return;
        }
        if (!Sheep.Sense.HasPlayerInRange)
            return;

        Sheep.Move.FollowBehind(Sheep.Sense.CurrentPlayer);

    }

    /// <summary>
    /// Exits the follow-player state and clears herd movement if this sheep is the commander.
    /// </summary>
    public override void Exit()
    {
        if (Sheep.IsCommander)
        {
            Sheep.HerdManager.SetAllSheepHerdMoving(false);
        }
    }
}
