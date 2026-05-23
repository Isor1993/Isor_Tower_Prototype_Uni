using UnityEngine;

public class SleepingState : SheepStateBase
{
    private readonly Timer _updateTimer = new Timer();
    private float _updateTime;
    public SleepingState(Sheep sheep, SheepFSM fSM) : base(sheep, fSM)
    {
    }

    public override void Enter()
    {
        Debug.Log($"{GetType().Name}: Change state => {nameof(SleepingState)}");
        _updateTime = Settings.SpawnTime;
        _updateTimer.Reset();
    }

    public override void Tick()
    {
        _updateTimer.Tick(Time.deltaTime);
        if (!_updateTimer.IsFinished(_updateTime))
            return;

        if (!Sheep.IsAsleep)
        {
            FSM.ChangeState(new PatrolState(Sheep, FSM));
            return;
        }
    }
    public override void Exit()
    {

    }
}
