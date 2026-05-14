using UnityEngine;

public class OnAlertState : SheepStateBase
{
    private readonly Timer _timer = new Timer();
 
    public OnAlertState(Sheep sheep, SheepFSM fSM) : base(sheep, fSM)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Enter()
    {
        Debug.Log($"{GetType().Name}: Change state => {nameof(OnAlertState)}");
        _timer.Reset();
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Tick()
    {       
        _timer.Tick(Time.deltaTime);
        Debug.Log($"{Sheep.name} | Player: {Sheep.Sense.HasPlayerInRange} | Tamed: {Sheep.IsTamed} | Threat: {Sheep.Sense.HasThreat}");
        if (!_timer.IsFinished(Settings.AlertTime))
            return;

        if (Sheep.Sense.HasThreat)
        {
           
           // FSM.ChangeState(new FleeState(Sheep, FSM));
            //return;
        }
        if (Sheep.Sense.HasPlayerInRange && Sheep.IsTamed)
        {
            FSM.ChangeState(new FollowPlayerState(Sheep, FSM));
            return;
        }
        Debug.Log("Kurz vor change Idle");
        FSM.ChangeState(new IdleState(Sheep, FSM));
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Exit()
    {
        // Cleanup (optional)        
    }
}
