using UnityEngine;

[CreateAssetMenu(fileName = "SheepSettings", menuName = "Animals/Sheep/Sheep Settings")]
public class SheepSettings : ScriptableObject
{
    [Header("General")]
    [SerializeField] private SheepTyp _typ;

    [Header("Health")]
    [SerializeField] private int _maxHealth = 100;

    [Header("Hunger")]
    [SerializeField] private float _hungerTickInterval = 10f;
    [SerializeField] private int _hungerTickRate = 4;
    [SerializeField] private int _eatTickRate = 4;
    [SerializeField] private int _hungerThreshhold = 50;
    [SerializeField] private int _maxHunger = 100;
    [SerializeField] private int _starvationDamage = 1;

    [Header("Move Behaviour")]
    [Header("Movement Settings")]
    [SerializeField] private float _walkSpeed = 1f;
    [SerializeField] private float _walkAcceleration = 4f;
    [SerializeField] private float _walkAngularSpeed = 120f;

    [Header("Flee Movement Settings")]
    [SerializeField] private float _fleeSpeed = 4f;
    [SerializeField] private float _fleeAcceleration = 14f;
    [SerializeField] private float _fleeAngularSpeed = 360f;
    [SerializeField] private float _minFleeDistance = 8f;
    [SerializeField] private float _maxFleeDistance = 10f;
    [SerializeField] private float _fleeDistanceSideOffset = 10f;

    [Header("Sense Radius")]
    [SerializeField] private float _threatRadius = 10f;
    [SerializeField] private float _sheepRadius = 8f;
    [SerializeField] private float _playerRadius = 12f;
    [SerializeField] private float _commanderRadius = 15f;
    [SerializeField] private float _fearRadiusforPlayer = 1f;

    [Header("Sense Layers")]
    [SerializeField] private LayerMask _threatLayer;
    [SerializeField] private LayerMask _sheepLayer;
    [SerializeField] private LayerMask _playerLayer;

    [Header("Debug Colors")]
    [SerializeField] private Color _colorThreat = Color.red;
    [SerializeField] private Color _colorSheep = Color.green;
    [SerializeField] private Color _colorPlayer = Color.blue;
    [SerializeField] private Color _colorCommander = Color.yellow;

    // ===== GENERAL =====
    public SheepTyp Typ => _typ;

    // ===== HEALTH =====
    public int MaxHealth => _maxHealth;

    // ===== HUNGER =====

    public float HungerTickInterval => _hungerTickInterval;
    public int HungerTick => _hungerTickRate;
    public int HungerThreshhold => _hungerThreshhold;
    public int MaxHunger => _maxHunger;
    public int StarvationDamage => _starvationDamage;
    public int EatTickRate => _eatTickRate;

    // ===== MOVE BEHAVIOUR =====
    public float WalkSpeed => _walkSpeed;
    public float WalkAcceleration => _walkAcceleration;
    public float WalkAngularSpeed => _walkAngularSpeed;

    public float FleeSpeed => _fleeSpeed;
    public float FleeAcceleration => _fleeAcceleration;
    public float FleeAngularSpeed => _fleeAngularSpeed;

    public float MinFleeDistance => _minFleeDistance;
    public float MaxFleeDistance => _maxFleeDistance;
    public float FleeDistanceSideOffset => _fleeDistanceSideOffset;


    // ===== SENSE RADIUS =====
    public float ThreatRadius => _threatRadius;
    public float SheepRadius => _sheepRadius;
    public float PlayerRadius => _playerRadius;
    public float CommanderRadius => _commanderRadius;
    public float FearRadiusforPlayer => _fearRadiusforPlayer;

    // ===== SENSE LAYERS =====
    public LayerMask ThreatLayer => _threatLayer;
    public LayerMask SheepLayer => _sheepLayer;
    public LayerMask PlayerLayer => _playerLayer;

    // ===== DEBUG COLORS =====
    public Color ColorThreat => _colorThreat;
    public Color ColorSheep => _colorSheep;
    public Color ColorPlayer => _colorPlayer;
    public Color ColorCommander => _colorCommander;

    
}