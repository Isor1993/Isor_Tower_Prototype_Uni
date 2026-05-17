using UnityEngine;

public class FleeState : SheepStateBase
{
    private readonly Transform _threat;
    private readonly Timer _updateTimer = new Timer();   
    public FleeState(Sheep sheep, SheepFSM fSM,Transform threat) : base(sheep, fSM)
    {
        _threat = threat;   
    }

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

    public override void Tick()
    {
       
        if (_threat==null)
        {
            Sheep.FSM.ChangeState(new RegroupState(Sheep, FSM));
            return;
        }

        if (Sheep.Move.HasReachedDestination())
        {
            Sheep.FSM.ChangeState(new RegroupState(Sheep, FSM));
            return;
        }
        _updateTimer.Tick(Time.deltaTime);

        if (_updateTimer.IsFinished(Settings.UpdateTickNewPosition))
        {
            _updateTimer.Reset();
            Sheep.Move.FleeFrom(_threat.position);
            return;
        }

    }
    public override void Exit()
    {
     Sheep.Move.SetWalkMovement();
    }
}
