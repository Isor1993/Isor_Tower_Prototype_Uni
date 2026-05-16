using UnityEngine;


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
        FSM = new SheepFSM();        
        Typ = Settings.Typ;

    }
    private void Start()
    {
        FSM.ChangeState(new FleeState(this, FSM)); //TEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEESSSSSSSSSSSSSSSSSSSSSSSSSSSSSSTTTTTTTTTTTTTTTTTTTTTTTTTTT
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

    public void FollowPlayer()
    {
        Move.Follow(Sense.CurrentPlayer);
    }


}
