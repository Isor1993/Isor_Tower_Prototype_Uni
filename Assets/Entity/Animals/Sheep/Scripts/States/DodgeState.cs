using UnityEngine;

public class DodgeState : SheepStateBase
{
    public DodgeState(Sheep sheep, SheepFSM fSM) : base(sheep, fSM)
    {
    }

    public override void Enter()
    {
        // Start-Logik      
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
