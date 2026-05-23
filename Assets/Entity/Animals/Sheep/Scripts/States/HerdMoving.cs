using UnityEngine;

public class HerdMoving : SheepStateBase
{
    public HerdMoving(Sheep sheep, SheepFSM fSM) : base(sheep, fSM)
    {

    }

    public override void Enter()
    {
        Debug.Log($"{GetType().Name}: Change state => {nameof(HerdMoving)}");


    }


    public override void Tick()
    {
        if (Sheep.IsCommander)
        {
            Sheep.Move.Follow(Sheep.Sense.CurrentPlayer.transform);
        }
        if (!Sheep.IsCommander)
        {

            Vector3 newPos = Sheep.HerdManager.GetFormationPositionForSheep(Sheep);
            if (Sheep.Move.TryGetValidTargetPosition(newPos, out Vector3 validPos))
            {
                newPos = validPos;
            }
            Sheep.Move.MoveTo(newPos);
        }
    }

    public override void Exit()
    {
      
    }
}
