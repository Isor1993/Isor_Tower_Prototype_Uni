

using UnityEngine;

public class DodgeState : SheepStateBase
{
    private Vector3 _previousTarget;
    private SheepStateBase _returnState;
    public DodgeState(Sheep sheep, SheepFSM fSM, SheepStateBase returnState) : base(sheep, fSM)
    {
        _returnState = returnState;
    }

    public override void Enter()
    {
        Debug.Log($"{GetType().Name}:{Sheep.gameObject.name}: Change state => {nameof(DodgeState)}");

        Sheep.Dodge.StartDodge(out _previousTarget);
    }
    public override void Tick()
    {
        if (!Sheep.Dodge.IsDodging)
        {
            if (_returnState != null)
            {
                FSM.ChangeState(_returnState);
            }
            else
            {
                FSM.ChangeState(new IdleState(Sheep, FSM));
            }
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
