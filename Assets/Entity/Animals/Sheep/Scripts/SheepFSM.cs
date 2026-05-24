/*****************************************************************************
* Project : Isors Tower Prototype
* File    : SheepFSM.cs
* Date    : 20.02.2026
* Author  : Eric Rosenberg
*
* Description :
* Manages the finite state machine for a sheep entity.
* Stores the currently active sheep state, handles state transitions by calling
* exit and enter lifecycle methods, and updates the active state every frame.
*
* History :
* 20.02.2026 ER Created
******************************************************************************/
using UnityEngine;

/// <summary>
/// Controls the current state of a sheep and manages transitions between sheep behavior states.
/// </summary>
public class SheepFSM
{
    private SheepStateBase _currentState;

    /// <summary>
    /// Changes the active sheep state.
    /// Exits the previous state, assigns the new state, and enters it.
    /// </summary>
    /// <param name="newState">The new state that should become active.</param>
    public void ChangeState(SheepStateBase newState)
    {

        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }

    /// <summary>
    /// Updates the currently active state.
    /// </summary>
    public void Tick()
    {
        _currentState?.Tick();
    }
}