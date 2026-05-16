using UnityEngine;

public class OnAlertState : SheepStateBase
{
    private readonly Timer _alertTimer = new Timer();
    private readonly Timer _reactionTimer = new Timer();

    public OnAlertState(Sheep sheep, SheepFSM fSM) : base(sheep, fSM)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Enter()
    {
        Debug.Log($"{GetType().Name}: Change state => {nameof(OnAlertState)}");
        _alertTimer.Reset();
        _reactionTimer.Reset();
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Tick()
    {
        _alertTimer.Tick(Time.deltaTime);
        //Debug.Log($"{Sheep.name} | Player: {Sheep.Sense.HasPlayerInRange} | Tamed: {Sheep.IsTamed} | Threat: {Sheep.Sense.HasThreat}");

        if (Sheep.Sense.HasThreat)
        {
            FSM.ChangeState(new FleeState(Sheep, FSM));
            return;
        }
        if (Sheep.Sense.HasPlayerInRange && Sheep.IsTamed)
        {
            FSM.ChangeState(new FollowPlayerState(Sheep, FSM));
            return;
        }
        if (!Sheep.IsTamed && Sheep.Sense.IsPlayerTooClose)
        {
            _reactionTimer.Tick(Time.deltaTime);

            if (_reactionTimer.IsFinished(Settings.ReactionTime))
            {
                FSM.ChangeState(new FleeState(Sheep, FSM));
                return;
            }
        }
        else
        {
            _reactionTimer.Reset();
        }
        if (!_alertTimer.IsFinished(Settings.AlertTime))
            return;
       
        FSM.ChangeState(new PatrolState(Sheep, FSM));
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Exit()
    {
        // Cleanup (optional)        
    }
}
