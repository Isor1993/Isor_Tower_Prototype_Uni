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
    [Tooltip("Max time before leaving State. Should be bigger than PatrolTimeMin!")]
    [SerializeField] private float _patrolTimeMax = 20f;
    [Tooltip("Min time before leaving State.")]
    [SerializeField] private float _patrolTimeMin = 10f;
    [Tooltip("Time before new position if reached.")]
    [SerializeField] private float _newTargetTime = 0.5f;

    [Header("Sleep State")]
    [Tooltip(""),Range(1f,60f)]
    [SerializeField] private float _spawnTime = 10f;
      

    [Header("Flee State")]
    [Tooltip("")]
    [SerializeField] private float _updateTickNewPosition = 15f;
      

   

    // ===== IDLE =====
    public float IdleTime => _idleTime;

    // ===== ALERT =====
    public float AlertTime => _alertTime;
    public float ReactionTime => _reactionTime;

    // ===== PATROL =====    
    public float PatrolTimeMax => _patrolTimeMax;
    public float PatrolTimeMin  => _patrolTimeMin; 
    public float PatrolNewTargetTime  => _newTargetTime; 
  

    // ===== FOLLOW =====
   

    // ===== FLEE =====
    public float UpdateTickNewPosition => _updateTickNewPosition;


    // ===== REGROUP =====
   

    // ===== EATING =====
   
    // ===== SLEEP =====
    public float SpawnTime  => _spawnTime;
}