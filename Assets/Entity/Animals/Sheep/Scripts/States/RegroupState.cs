using UnityEngine;

public class RegroupState : SheepStateBase
{
    public RegroupState(Sheep sheep, SheepFSM fSM) : base(sheep, fSM)
    {
        Debug.Log($"{GetType().Name}: Change state => {nameof(RegroupState)}");
    }

    public override void Enter()
    {
        if(!Sheep.IsCommander&&Sheep.IsHerdMoving)
        {
        Vector3 targtePos = Sheep.HerdManager.GetFormationPositionForNormalSheep(SheepS);
        Sheep.Move.MoveTo(targtePos);
        }
        if(!Sheep.IsCommander&&!Sheep.IsHerdMoving)
        {
            Sheep.Move.MoveTo(Sheep.HerdManager.CommanderPosition);
        }
        
    }

    public override void Tick()
    {
        Sheep.
        if (Sheep.IsCommander&&Sheep.is)
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
