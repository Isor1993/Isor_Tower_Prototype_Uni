using UnityEngine;

public class EatingState : SheepStateBase
{
    public EatingState(Sheep sheep, SheepFSM fSM) : base(sheep, fSM)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Enter()
    {
        Debug.Log($"{GetType().Name}:Change state => {nameof(EatingState)}");
        Sheep.Hunger.IsEating = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Tick()
    {


        if (Sheep.Sense.HasThreat)
        {
            FSM.ChangeState(new OnAlertState(Sheep, FSM));
            return;
        }
        if (Sheep.Hunger.IsFull)
        {
            FSM.ChangeState(new IdleState(Sheep, FSM));
            return;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Exit()
    {
        Sheep.Hunger.IsEating = false;

    }
}
