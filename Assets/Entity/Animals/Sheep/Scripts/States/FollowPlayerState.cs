using UnityEngine;

public class FollowPlayerState : SheepStateBase
{
    public FollowPlayerState(Sheep sheep, SheepFSM fSM) : base(sheep, fSM)
    {

    }

    public override void Enter()
    {
        Debug.Log($"{GetType().Name}: Change state => {nameof(FollowPlayerState)}");
    }

    public override void Tick()
    {
        if(!Sheep.IsTamed)
        {
            FSM.ChangeState(new RegroupState(Sheep,FSM));
            return;
        }
        if(!Sheep.Sense.HasPlayerInRange||Sheep.Sense.CurrentPlayer==null)
        {
            FSM.ChangeState(new RegroupState(Sheep, FSM));
            return;
        }
        Sheep.Move.Follow(Sheep.Sense.CurrentPlayer);
            
    }
    public override void Exit()
    {
        // Cleanup (optional)
        
    }
}
