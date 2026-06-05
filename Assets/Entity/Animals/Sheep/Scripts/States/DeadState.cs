/*****************************************************************************
* Project : Isors Tower Prototype
* File    : DeadState.cs
* Date    : 20.02.2026
* Author  : Eric Rosenberg
*
* Description :
* Represents the dead behavior state of a sheep.
* Keeps the sheep inactive for a configured respawn duration and triggers
* the spawn process once the respawn timer has finished.
*
* History :
* 20.02.2026 ER Created
******************************************************************************/

using UnityEngine;

/// <summary>
/// State in which the sheep remains dead until the respawn timer finishes.
/// </summary>
public class DeadState : SheepStateBase
{
    private readonly Timer _spawnTimer = new Timer();
    private float _spawnTime;

    public DeadState(Sheep sheep, SheepFSM fSM) : base(sheep, fSM)
    {
    }

    /// <summary>
    ///  Enters the dead state, loads the respawn duration, and resets the respawn timer.
    /// </summary>
    public override void Enter()
    {
#if UNITY_EDITOR
        Debug.Log($"{GetType().Name}:{Sheep.gameObject.name}: Change state => {nameof(DeadState)}");
#endif
        _spawnTime = Settings.SpawnTime;
        _spawnTimer.Reset();
    }

    /// <summary>
    /// Updates the respawn timer and starts the sheep spawn process once the timer is finished.
    /// </summary>
    public override void Tick()
    {
        _spawnTimer.Tick(Time.deltaTime);
        if (_spawnTimer.IsFinished(_spawnTime))
        {
            Sheep.HandleSpawn();
        }
    }

    /// <summary>
    /// Exits the dead state.
    /// </summary>
    public override void Exit()
    {
    }
}