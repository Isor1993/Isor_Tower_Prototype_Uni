/*****************************************************************************
* Project : Isors Tower Prototype
* File    : HerdMoving.cs
* Date    : 20.02.2026
* Author  : Eric Rosenberg
*
* Description :
* Represents the herd movement behavior state of a sheep.
* Allows the commander sheep to follow the player while normal sheep move
* into their assigned herd formation positions. The state keeps the herd
* moving as a group until herd movement ends or required conditions are lost.
*
* History :
* 20.02.2026 ER Created
******************************************************************************/
using UnityEngine;

/// <summary>
/// State in which the sheep participates in active herd movement.
/// The commander follows the player, while normal sheep follow their assigned formation positions.
/// </summary>
public class HerdMoving : SheepStateBase
{ 
    public HerdMoving(Sheep sheep, SheepFSM fSM) : base(sheep, fSM)
    {

    }

    /// <summary>
    /// Enters the herd movement state.
    /// </summary>
    public override void Enter()
    {
        Debug.Log($"{GetType().Name}: Change state => {nameof(HerdMoving)}");
    }

    /// <summary>
    /// Updates herd movement behavior.
    /// The commander continues following the player while it is tamed and has a valid player target.
    /// Normal sheep continue moving toward their assigned formation positions while herd movement is active.
    /// If required conditions are lost, the sheep returns to patrol behavior.
    /// </summary>
    public override void Tick()
    {
        if(!Sheep.IsTamed&&Sheep.IsCommander)
        {
            FSM.ChangeState(new PatrolState(Sheep,FSM));
            return;
        }
        if(!Sheep.IsHerdMoving&&!Sheep.IsCommander)
        {
            FSM.ChangeState(new PatrolState(Sheep, FSM));
            return;
        }
        if (Sheep.IsCommander)
        {
            if (Sheep.Sense.CurrentPlayer == null)
            {
                FSM.ChangeState(new PatrolState(Sheep, FSM));
                return;
            }
            Sheep.Move.FollowBehind(Sheep.Sense.CurrentPlayer);
            return;
        }
        if (!Sheep.IsCommander)
        {
            Vector3 newPos = Sheep.HerdManager.GetFormationPositionForSheep(Sheep);
            if (Sheep.Move.TryGetValidTargetPosition(newPos, out Vector3 validPos))
            {
                newPos = validPos;
            }
            Sheep.Move.MoveTo(newPos);
        }
    }

    /// <summary>
    /// Exits the herd movement state.
    /// </summary>
    public override void Exit()
    {
      
    }
}