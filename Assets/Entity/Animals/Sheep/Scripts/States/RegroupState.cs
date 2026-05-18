using UnityEngine;

public class RegroupState : SheepStateBase
{
    public RegroupState(Sheep sheep, SheepFSM fSM) : base(sheep, fSM)
    {
        Debug.Log($"{GetType().Name}: Change state => {nameof(RegroupState)}");
    }

    public override void Enter()
    {
        if(!Sheep.IsCommander)
        {
        Vector3 targtePos = Sheep.HerdManager.GetHerdPositionForSheep(Sheep);
        Sheep.Move.MoveTo(targtePos);
        }
    }

    public override void Tick()
    {

        if (Sheep.IsHerdMoving)
        {

        }
        else
        {

        }

    }
    public override void Exit()
    {

    }
}
