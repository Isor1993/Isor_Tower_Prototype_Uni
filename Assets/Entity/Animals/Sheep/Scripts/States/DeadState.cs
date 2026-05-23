using UnityEngine;

public class DeadState : SheepStateBase
{
    private readonly Timer _spawnTimer = new Timer();
    private float _spawnTime;
    public DeadState(Sheep sheep, SheepFSM fSM) : base(sheep, fSM)
    {
    }

    public override void Enter()
    {
        Debug.Log($"{GetType().Name}: Change state => {nameof(DeadState)}");
        _spawnTime = Settings.SpawnTime;
        _spawnTimer.Reset();

    }

    public override void Tick()
    {
        _spawnTimer.Tick(Time.deltaTime);
        if (_spawnTimer.IsFinished(_spawnTime))
        {
            Sheep.HandleSpawn();
            FSM.ChangeState(new IdleState(Sheep, FSM));
        }
    }
    public override void Exit()
    {

    }
}
