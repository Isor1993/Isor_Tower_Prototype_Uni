using UnityEngine;

public class RegroupState : SheepStateBase
{
    public RegroupState(Sheep sheep, SheepFSM fSM) : base(sheep, fSM)
    {
    }

    private Vector3 targetPos;
    public override void Enter()
    {
        Debug.Log($"{GetType().Name}: Change state => {nameof(RegroupState)}");

        if (Sheep.IsHerdMoving)
        {
             targetPos = Sheep.HerdManager.GetFormationPositionForSheep(Sheep);
            if (Sheep.Move.TryGetValidTargetPosition(targetPos, out Vector3 validPosition))
            {
                targetPos = validPosition;
            }         
        }
        else
        {
             targetPos = Sheep.HerdManager.GetRandomRegroupPosition();
            if (Sheep.Move.TryGetValidTargetPosition(targetPos, out Vector3 validPos))
            {
                targetPos = validPos;
            }            
        }
        Sheep.Move.MoveTo(targetPos);
    }

    public override void Tick()
    {
        if(Sheep.IsHerdMoving)
        {
            
            if (Sheep.HerdManager.AreAllSheepInPosition())
            {
                Sheep.FSM.ChangeState(new HerdMoving(Sheep, FSM));
            }
        }
        else
        {           
            if (Sheep.Move.HasReachedDestination())
            {
                Sheep.FSM.ChangeState(new PatrolState(Sheep, FSM));
            }
        }
       

    }
    public override void Exit()
    {
       

    }
}
