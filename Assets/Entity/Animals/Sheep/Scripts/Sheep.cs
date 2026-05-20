using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;


public enum SheepTyp
{
    Normal,
    Commander
}

[RequireComponent(typeof(SheepSense))]
[RequireComponent(typeof(SheepHunger))]
[RequireComponent(typeof(SheepHealth))]
[RequireComponent(typeof(SheepMoveBehaviour))]
public class Sheep : MonoBehaviour, IDayNightListener
{
    [SerializeField]private DayNightCycleEventManager _eventManager;
    [SerializeField] SheepSettings _settings;

    private bool _isSleeping = false;
    private bool _hasFear = false;
    [SerializeField] private bool _isTamed = false;
    [SerializeField] private SheepStateSettings _stateSettings;
    [SerializeField] private HerdManager _herdManager;
    private NavMeshAgent _agent;
    public bool IsHerdMoving;
    [SerializeField] private Vector3 _graveyardPosition;
    [SerializeField] private Vector3 _spawnPosition;
    [SerializeField] private float _spawnTime;

    public bool IsAlive=>Health.IsAlive;
    private float _elapsedTime;
    

    

    public HerdManager HerdManager => _herdManager;


    public SheepMoveBehaviour Move { get; private set; }
    public SheepHealth Health { get; private set; }

    public SheepHunger Hunger { get; private set; }

    public SheepSense Sense { get; private set; }

    public SheepFSM FSM { get; private set; }

    public SheepSettings Settings => _settings;

    public SheepTyp Typ { get; private set; }

    public SheepStateSettings StateSettings => _stateSettings;

    public bool IsCommander => Typ == SheepTyp.Commander;

    public bool IsAsleep => _isSleeping;

    public bool IsTamed => _isTamed;

    public bool HasFear => _hasFear;

    private void Awake()
    {
        
        Health = GetComponent<SheepHealth>();
        Hunger = GetComponent<SheepHunger>();
        Sense = GetComponent<SheepSense>();
        Move=GetComponent<SheepMoveBehaviour>();
        _agent = GetComponent<NavMeshAgent>();
        FSM = new SheepFSM();        
        Typ = Settings.Typ;

    }
    private void Start()
    {
        FSM.ChangeState(new RegroupState(this, FSM)); //TEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEESSSSSSSSSSSSSSSSSSSSSSSSSSSSSSTTTTTTTTTTTTTTTTTTTTTTTTTTT
    }

    private void OnEnable()
    {
        if (_eventManager == null) return;
        _eventManager.Subscribe(this);
    }

    private void OnDisable()
    {
        if (_eventManager == null) return;
        _eventManager.Unsubscribe(this);
    }

    private void Update()
    {        
        
        FSM.Tick();
        if(!IsAlive)
        {
            _elapsedTime = Time.deltaTime;
            FSM.ChangeState(new DeadState(this,FSM));
            HandleDeath();
            if(_elapsedTime>=_spawnTime)
            {
                HandleSpawn();
            }
        }
        


    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="previousphase"></param>
    /// <param name="currentPhase"></param>
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

    private void HandleDeath()
    {
        _agent.enabled = false;
        Move.enabled = false;
        Sense.enabled = false;
        Hunger.enabled = false;
        transform.position = _graveyardPosition;        

    }

    private void HandleSpawn()
    {
        _agent.enabled = true;
        Move.enabled = true;
        Sense.enabled = true;
        Hunger.enabled = true;
        transform.position = _spawnPosition;
    }
   
}
