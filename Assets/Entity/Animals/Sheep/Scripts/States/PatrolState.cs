using UnityEngine;

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

    public override void Enter()
    {
        Debug.Log($"{GetType().Name}: Change state => {nameof(PatrolState)}");

        _patrolTime = Random.Range(Settings.PatrolTimeMin, Settings.PatrolTimeMax);
        _newTargetTime = Settings.PatrolNewTargetTime;

        _patrolTimer.Reset();
        _newTargetTimer.Reset();

        SetNewPatrolTarget();
    }

    public override void Tick()
    {
        _newTargetTimer.Tick(Time.deltaTime);
        _patrolTimer.Tick(Time.deltaTime);

        if (Sheep.Sense.HasThreat)
        {
            FSM.ChangeState(new OnAlertState(Sheep, FSM));
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
            _newTargetTimer.Reset();
            SetNewPatrolTarget();
        }
    }

    public override void Exit()
    {

    }

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