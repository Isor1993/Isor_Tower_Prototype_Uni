/*****************************************************************************
* Project : Isors Tower Prototype
* File    : Sheep.cs
* Date    : 20.02.2026
* Author  : Eric Rosenberg
*
* Description :
* Represents a sheep entity and acts as the central access point for its
* core components, including sensing, hunger, health, movement, and FSM logic.
* Handles lifecycle events such as damage, death, spawning, starvation, and
* day-night phase changes.
*
* History :
* 20.02.2026 ER Created
******************************************************************************/
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Defines the role of a sheep inside a herd.
/// </summary>
public enum SheepTyp
{
    Normal,
    Commander
}

/// <summary>
/// Central sheep entity that connects all sheep-related components and controls
/// state transitions based on health, hunger, damage, and day-night events.
/// </summary>
[RequireComponent(typeof(SheepSense))]
[RequireComponent(typeof(SheepHunger))]
[RequireComponent(typeof(SheepHealth))]
[RequireComponent(typeof(SheepMoveBehaviour))]
public class Sheep : MonoBehaviour, IDayNightListener
{
    [SerializeField] SheepTyp _typ;
    [Header("Settings")]
    [Tooltip("ScriptableObject that contains the base configuration for this sheep.")]
    [SerializeField] SheepSettings _settings;
    [Tooltip("Event manager used to subscribe this sheep to day-night phase change events.")]
    [SerializeField] private DayNightCycleEventManager _eventManager;
    [Tooltip("Determines whether this sheep is currently tamed by the player.")]
    [SerializeField] private bool _isTamed = false;
    [Tooltip("ScriptableObject that contains FSM state-related settings for this sheep.")]
    [SerializeField] private SheepStateSettings _stateSettings;
    [Tooltip("Herd manager that controls herd-level behavior and positioning for this sheep.")]
    [SerializeField] private HerdManager _herdManager;
    [Tooltip("World position where the sheep is moved after dying.")]
    [SerializeField] private Vector3 _graveyardPosition;
    [Tooltip("World position where the sheep is placed when it respawns.")]
    [SerializeField] private Vector3 _spawnPosition;
    [Tooltip("Time in seconds before the sheep should respawn after death."), Range(1, 1000)]
    [SerializeField] private float _spawnTime;


    /// <summary>
    /// Indicates whether this sheep is currently moving as part of herd movement.
    /// </summary>
    public bool IsHerdMoving;

    private NavMeshAgent _agent;
    private bool _isSleeping = false;

    /// <summary>
    /// Indicates whether this sheep is currently alive.
    /// </summary>
    public bool IsAlive => Health != null && Health.IsAlive;

    /// <summary>
    /// Gets the herd manager assigned to this sheep.
    /// </summary>
    public HerdManager HerdManager => _herdManager;

    /// <summary>
    /// Gets the movement component used by this sheep.
    /// </summary>
    public SheepMoveBehaviour Move { get; private set; }

    /// <summary>
    /// Gets the health component used by this sheep.
    /// </summary>
    public SheepHealth Health { get; private set; }

    /// <summary>
    /// Gets the hunger component used by this sheep.
    /// </summary>
    public SheepHunger Hunger { get; private set; }

    /// <summary>
    /// Gets the sensing component used by this sheep.
    /// </summary>
    public SheepSense Sense { get; private set; }

    /// <summary>
    /// Gets the finite state machine that controls this sheep's current behavior.
    /// </summary>
    public SheepFSM FSM { get; private set; }

    /// <summary>
    /// Gets the base settings assigned to this sheep.
    /// </summary>
    public SheepSettings Settings => _settings;

    /// <summary>
    /// Gets the type of this sheep inside the herd.
    /// </summary>
    public SheepTyp Typ => _typ;
    /// <summary>
    /// Gets the state settings assigned to this sheep.
    /// </summary>
    public SheepStateSettings StateSettings => _stateSettings;

    /// <summary>
    /// Indicates whether this sheep is the commander of its herd.
    /// </summary>
    public bool IsCommander => _typ == SheepTyp.Commander;

    /// <summary>
    /// Indicates whether this sheep is currently asleep.
    /// </summary>
    public bool IsAsleep => _isSleeping;

    /// <summary>
    /// Indicates whether this sheep is currently tamed by the player.
    /// </summary>
    public bool IsTamed => _isTamed;
   

    private void Awake()
    {

        Health = GetComponent<SheepHealth>();
        Hunger = GetComponent<SheepHunger>();
        Sense = GetComponent<SheepSense>();
        Move = GetComponent<SheepMoveBehaviour>();
        _agent = GetComponent<NavMeshAgent>();
        FSM = new SheepFSM();
        _typ = Settings.Typ;

    }

    private void Start()
    {
        FSM.ChangeState(new RegroupState(this, FSM)); //TEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEESSSSSSSSSSSSSSSSSSSSSSSSSSSSSSTTTTTTTTTTTTTTTTTTTTTTTTTTT
    }

    private void OnEnable()
    {
        if (_eventManager != null)
            _eventManager.Subscribe(this);

        if (Health != null)
        {
            Health.OnDied += HandleDeath;
            Health.OnDamaged += HandleDamage;
        }

        if (Hunger != null)
            Hunger.OnStarving += HandleStarving;
    }

    private void OnDisable()
    {
        if (_eventManager != null)
            _eventManager.Unsubscribe(this);

        if (Health != null)
        {
            Health.OnDied -= HandleDeath;
            Health.OnDamaged -= HandleDamage;
        }

        if (Hunger != null)
            Hunger.OnStarving -= HandleStarving;
    }


    private void Update()
    {
        FSM.Tick();
    }

    /// <summary>
    /// Reacts to day-night phase changes and toggles the sheep's sleeping state.
    /// </summary>
    /// <param name="previousphase">The previous day phase before the change.</param>
    /// <param name="currentPhase">The new current day phase after the change.</param>
    public void OnDayPhaseChanged(DayPhase previousphase, DayPhase currentPhase)
    {
        Debug.Log("Sheep reacting on changed DayPhase");
        if (currentPhase == DayPhase.Night)
        {
            _isSleeping = true;
            Debug.Log("Sheep => Sleeping");
        }
        else
        {
            _isSleeping = false;
            Debug.Log("Sheep => Awake");
        }
    }

    /// <summary>
    /// Handles sheep death by disabling active behavior components,
    /// switching to the dead state, and moving the sheep to the graveyard position.
    /// </summary>
    private void HandleDeath()
    {
        _agent.enabled = false;
        Move.enabled = false;
        Sense.enabled = false;
        Hunger.enabled = false;
        FSM.ChangeState(new DeadState(this, FSM));
        transform.position = _graveyardPosition;

    }

    /// <summary>
    /// Handles sheep spawning by re-enabling behavior components,
    /// switching to the idle state, and moving the sheep to the spawn position.
    /// </summary>
    public void HandleSpawn()
    {
        _agent.enabled = true;
        Move.enabled = true;
        Sense.enabled = true;
        Hunger.enabled = true;        
        transform.position = _spawnPosition;        
    }

    /// <summary>
    /// Applies starvation damage to the sheep when the hunger system raises a starvation event.
    /// </summary>
    /// <param name="damageType">The damage type used for starvation damage.</param>
    private void HandleStarving(DamageType damageType)
    {
        Health.TakeDamage(Hunger.StarvationDamage, damageType);

    }

    /// <summary>
    /// Reacts to non-starvation damage by switching the sheep into the flee state.
    /// </summary>
    /// <param name="damage">The amount of damage received.</param>
    /// <param name="damageType">The type of damage received.</param>
    private void HandleDamage(int damage, DamageType damageType)
    {
        if (damageType == DamageType.Starvation)
            return;
        FSM.ChangeState(new FleeState(this, FSM, Sense.CurrentThreat));
    }
}