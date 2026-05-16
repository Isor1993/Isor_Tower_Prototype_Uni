using UnityEngine;

/// <summary>
/// 
/// </summary>
public class SheepFSM
{
    private SheepStateBase _currentState;  
    public void ChangeState(SheepStateBase newState)
    {

        _currentState?.Exit();       
        _currentState = newState;
        _currentState?.Enter();
    }

    public void Tick()
    {
        _currentState?.Tick();
    }

}
