using UnityEngine;

public class PatrolState : SheepStateBase
{
    public PatrolState(Sheep sheep, SheepFSM fSM) : base(sheep, fSM)
    {
    }

    public override void Enter()
    {
        Debug.Log($"{GetType().Name}: Change state => {nameof(PatrolState)}");
    }

    public override void Tick()
    {
        // Entscheidungslogik   
    }
    public override void Exit()
    {
        // Cleanup (optional)
        Debug.Log("Exit Idle");
    }
}
