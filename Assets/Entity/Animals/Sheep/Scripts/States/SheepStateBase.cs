using UnityEngine;

public abstract class SheepStateBase
{

    protected Sheep Sheep;
    protected SheepFSM FSM;
    protected SheepStateSettings Settings;
     
    protected SheepStateBase(Sheep sheep, SheepFSM fSM)
    {
        Sheep = sheep;
        FSM = fSM;
        Settings = sheep.StateSettings;
    }

    public virtual void Enter() { }
    public virtual void Tick() { }
    public virtual void Exit() { }
}

