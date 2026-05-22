using UnityEngine;

public class RegroupState : SheepStateBase
{
    public RegroupState(Sheep sheep, SheepFSM fSM) : base(sheep, fSM)
    {
        Debug.Log($"{GetType().Name}: Change state => {nameof(RegroupState)}");
    }

    public override void Enter()
    {
      
        
    }

    public override void Tick()
    {
       

    }
    public override void Exit()
    {

    }
}
