using UnityEngine;

[CreateAssetMenu(fileName = "SheepStateSettings", menuName = "Animals/Sheep/Sheep State Settings")]
public class SheepStateSettings : ScriptableObject
{
    [Header("Idle State")]
    [Tooltip("How long the sheep stays idle before changing state.")]
    [SerializeField] private float _idleTime = 5f;

    [Header("Alert State")]
    [Tooltip("How long the sheep stays alert before reacting.")]
    [SerializeField] private float _alertTime = 1.5f;
    [SerializeField] private float _reactionTime = 2f;

    [Header("Patrol State")]
    [Tooltip("Radius around the sheep for random patrol points.")]
    [SerializeField] private float _patrolRadius = 10f;
    [Tooltip("Max time before leaving State. Should be bigger than PatrolTimeMin!")]
    [SerializeField] private float _patrolTimeMax = 20f;
    [Tooltip("Min time before leaving State.")]
    [SerializeField] private float _patrolTimeMin = 10f;
    [Tooltip("Time before new position if reached.")]
    [SerializeField] private float _newTargetTime = 0.5f;


    [Header("Follow State")]
    [Tooltip("Minimum distance before the sheep stops following.")]
    [SerializeField] private float _followStoppingDistance = 2f;

    [Header("Flee State")]
    [Tooltip("")]
    [SerializeField] private float _updateTickNewPosition = 15f;

    [Header("Regroup State")]
    [Tooltip("Distance required before regroup is considered complete.")]
    [SerializeField] private float _regroupDistance = 3f;

    [Header("Eating State")]
    [Tooltip("How long the sheep eats before checking again.")]
    [SerializeField] private float _eatDuration = 4f;

    // ===== IDLE =====
    public float IdleTime => _idleTime;

    // ===== ALERT =====
    public float AlertTime => _alertTime;
    public float ReactionTime => _reactionTime;

    // ===== PATROL =====
    public float PatrolRadius => _patrolRadius;
    public float PatrolTimeMax => _patrolTimeMax;
    public float PatrolTimeMin  => _patrolTimeMin; 
    public float PatrolNewTargetTime  => _newTargetTime; 
  

    // ===== FOLLOW =====
    public float FollowStoppingDistance => _followStoppingDistance;

    // ===== FLEE =====
    public float UpdateTickNewPosition => _updateTickNewPosition;


    // ===== REGROUP =====
    public float RegroupDistance => _regroupDistance;

    // ===== EATING =====
    public float EatDuration => _eatDuration;

}