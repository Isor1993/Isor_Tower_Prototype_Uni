using UnityEngine;

public class IdleState : SheepStateBase
{
    private readonly Timer _timer = new Timer();


    public IdleState(Sheep sheep, SheepFSM fSM) : base(sheep, fSM)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public override void Enter()
    {
        Debug.Log($"{GetType().Name}: Change state => {nameof(IdleState)}");

        _timer.Reset();
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Tick()
    {
        _timer.Tick(Time.deltaTime);
        //Debug.Log($"Timer{_timer.ElapsedTime}");
        //Debug.Log($"HasTreat{Sheep.Sense.HasThreat}");
        //Debug.Log($"ISHungry{Sheep.Hunger.IsHungry}");
        //Debug.Log($"ISFinished{_timer.IsFinished(Settings.IdleTime)}");

        if (Sheep.Sense.HasThreat)
        {
            //Debug.Log("HasTreat");
            FSM.ChangeState(new OnAlertState(Sheep, FSM));
            return;
        }
        if (Sheep.Hunger.IsHungry)
        {
            //Debug.Log("ISHungry");
            //FSM.ChangeState(new EatingState(Sheep, FSM));
            //return;
        }
        if (_timer.IsFinished(Settings.IdleTime))
        {
            //Debug.Log("ISFinished");
            //FSM.ChangeState(new RegroupState(Sheep, FSM));
            //return;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public override void Exit()
    {
        // Cleanup (optional)       
    }
}
