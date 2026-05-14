using System;
using UnityEngine;

public class SheepSense : MonoBehaviour
{
    [SerializeField] SheepSettings settings;

    [Header("Debug Settings")]
    [Tooltip("")]
    [SerializeField] private bool _useLocalOverrides = false;
    [Header("On selected Gizmos")]
    [Tooltip("")]
    [SerializeField] private bool _showClosestDetectionLine = false;
    [Tooltip("")]
    [SerializeField] private bool _showDetectionRangeWires = false;
    [Header("Permament on Gizmos")]
    [Tooltip("")]
    [SerializeField] private bool _showClosestDetectionLinePermanent = false;
    [Tooltip("")]
    [SerializeField] private bool _showDetectionRangeWiresPermanent = false;

    [Tooltip("")]
    [SerializeField] private Color _colorThreat;
    [Tooltip("")]
    [SerializeField] private Color _colorSheep;
    [Tooltip("")]
    [SerializeField] private Color _colorPlayer;
    [Tooltip("")]
    [SerializeField] private Color _colorCommander;


    [Header("Local Overrides (only if enabled)")]
    [Tooltip(""), Range(1, 200)]
    [SerializeField] private float _threatRadius;
    [Tooltip(""), Range(1, 200)]
    [SerializeField] private float _sheepRadius;
    [Tooltip(""), Range(1, 200)]
    [SerializeField] private float _playerRadius;
    [Tooltip(""), Range(1, 200)]
    [SerializeField] private float _commanderRadius;

    [Tooltip("")]
    [SerializeField] private LayerMask _threatLayer;
    [Tooltip("")]
    [SerializeField] private LayerMask _sheepLayer;
    [Tooltip("")]
    [SerializeField] private LayerMask _playerLayer;

    /// <summary>
    /// 
    /// </summary>
    private float ThreatRadius => _useLocalOverrides ? _threatRadius : settings.ThreatRadius;

    /// <summary>
    /// 
    /// </summary>
    private float SheepRadius => _useLocalOverrides ? _sheepRadius : settings.SheepRadius;

    /// <summary>
    /// 
    /// </summary>
    private float PlayerRadius => _useLocalOverrides ? _playerRadius : settings.PlayerRadius;

    /// <summary>
    /// 
    /// </summary>
    private float CommanderRadius => _useLocalOverrides ? _commanderRadius : settings.CommanderRadius;

    /// <summary>
    /// 
    /// </summary>
    private LayerMask ThreatLayer => _useLocalOverrides ? _threatLayer : settings.ThreatLayer;

    /// <summary>
    /// 
    /// </summary>
    private LayerMask SheepLayer => _useLocalOverrides ? _sheepLayer : settings.SheepLayer;

    /// <summary>
    /// 
    /// </summary>
    private LayerMask PlayerLayer => _useLocalOverrides ? _playerLayer : settings.PlayerLayer;

    /// <summary>
    /// 
    /// </summary>
    public Transform CurrentThreat { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public Transform CurrentSheep { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public Transform CurrentPlayer { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public Transform CurrentCommander { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public bool HasThreat { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public bool HasSheepInRange { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public bool HasPlayerInRange { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public bool HasCommanderInRange { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public Collider[] ThreatHits { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public Collider[] SheepHits { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public Collider[] PlayerHits { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public Collider[] CommanderHits { get; private set; }

    private void Awake()
    {
        SetBaseValues();

    }

    /// <summary>
    /// 
    /// </summary>
    private void SetBaseValues()
    {
        _threatRadius = settings.ThreatRadius;
        _sheepRadius = settings.SheepRadius;
        _playerRadius = settings.PlayerRadius;
        _commanderRadius = settings.CommanderRadius;
        _threatLayer = settings.ThreatLayer;
        _sheepLayer = settings.SheepLayer;
        _playerLayer = settings.PlayerLayer;
        _colorThreat = settings.ColorThreat;
        _colorSheep = settings.ColorSheep;
        _colorPlayer = settings.ColorPlayer;
        _colorCommander = settings.ColorCommander;
    }

    private void Update()
    {
        CheckThreat();
        CheckSheepInRange();
        CheckPlayerInRange();
        CheckCommanderInRange();
    }

    /// <summary>
    /// 
    /// </summary>
    private void CheckThreat()
    {
        ThreatHits = Physics.OverlapSphere(transform.position, ThreatRadius, ThreatLayer);

        CurrentThreat = TryGetClosest(ThreatHits);
        HasThreat = CurrentThreat != null;
    }

    /// <summary>
    /// 
    /// </summary>
    private void CheckSheepInRange()
    {
        SheepHits = Physics.OverlapSphere(transform.position, SheepRadius, SheepLayer);

        CurrentSheep = TryGetClosest(SheepHits);
        HasSheepInRange = CurrentSheep != null;


    }

    /// <summary>
    /// 
    /// </summary>
    private void CheckPlayerInRange()
    {
        PlayerHits = Physics.OverlapSphere(transform.position, PlayerRadius, PlayerLayer);

        CurrentPlayer = TryGetClosest(PlayerHits);
        HasPlayerInRange = CurrentPlayer != null;
    }

    /// <summary>
    /// 
    /// </summary>
    private void CheckCommanderInRange()
    {
        CommanderHits = Physics.OverlapSphere(transform.position, CommanderRadius, SheepLayer);

        CurrentCommander = TryGetClosestCommander(CommanderHits);
        HasCommanderInRange = CurrentCommander != null;
    }

    /// <summary>
    /// 
    /// </summary>
    private Transform TryGetClosest(Collider[] hits)
    {
        Transform closest = null;

        float closestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            if (hit.transform == transform)
                continue;

            float distance = Vector3.Distance(transform.position, hit.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = hit.transform;
            }
        }
        return closest;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="hits"></param>
    /// <returns></returns>
    private Transform TryGetClosestCommander(Collider[] hits)
    {
        Transform closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            if (hit.transform == transform)
                continue;

            if (!hit.TryGetComponent(out Sheep sheep))
                continue;

            if (!sheep.IsCommander)
                continue;

            float distance = Vector3.Distance(transform.position, hit.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = sheep.transform;
            }
        }

        return closest;
    }

#if(UNITY_EDITOR)
    /// <summary>
    /// 
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (settings == null)
            return;

        var currentThreatPosition = GetTargetPosition(CurrentThreat);
        var currentSheepPosition = GetTargetPosition(CurrentSheep);
        var currentPlayerPosition = GetTargetPosition(CurrentPlayer);
        var currentCommanderPosition = GetTargetPosition(CurrentCommander);

        if (_showDetectionRangeWires)
        {
            ShowDetectionAreaOnGizmos();

        }
        if (_showClosestDetectionLine)
        {
            ShowClosestDetectionOnGizmos(currentThreatPosition, currentSheepPosition, currentPlayerPosition, currentCommanderPosition);
        }
    }
    private void OnDrawGizmos()
    {
        var currentThreatPosition = GetTargetPosition(CurrentThreat);
        var currentSheepPosition = GetTargetPosition(CurrentSheep);
        var currentPlayerPosition = GetTargetPosition(CurrentPlayer);
        var currentCommanderPosition = GetTargetPosition(CurrentCommander);

        if (_showDetectionRangeWiresPermanent)
        {
            ShowDetectionAreaOnGizmos();

        }
        if (_showClosestDetectionLinePermanent)
        {
            ShowClosestDetectionOnGizmos(currentThreatPosition, currentSheepPosition, currentPlayerPosition, currentCommanderPosition);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="currentThreatPosition"></param>
    /// <param name="currentSheepPosition"></param>
    /// <param name="currentPlayerPosition"></param>
    /// <param name="currentCommanderPosition"></param>
    private void ShowClosestDetectionOnGizmos(Vector3 currentThreatPosition, Vector3 currentSheepPosition, Vector3 currentPlayerPosition, Vector3 currentCommanderPosition)
    {
        Gizmos.color = _colorThreat;
        Gizmos.DrawLine(transform.position, currentThreatPosition);

        Gizmos.color = _colorSheep;
        Gizmos.DrawLine(transform.position, currentSheepPosition);

        Gizmos.color = _colorPlayer;
        Gizmos.DrawLine(transform.position, currentPlayerPosition);

        Gizmos.color = _colorCommander;
        Gizmos.DrawLine(transform.position, currentCommanderPosition);
    }

    /// <summary>
    /// 
    /// </summary>
    private void ShowDetectionAreaOnGizmos()
    {
        Gizmos.color = _colorThreat;
        Gizmos.DrawWireSphere(transform.position, ThreatRadius);

        Gizmos.color = _colorSheep;
        Gizmos.DrawWireSphere(transform.position, SheepRadius);

        Gizmos.color = _colorPlayer;
        Gizmos.DrawWireSphere(transform.position, PlayerRadius);

        Gizmos.color = _colorCommander;
        Gizmos.DrawWireSphere(transform.position, CommanderRadius);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private Vector3 GetTargetPosition(Transform target)
    {
        return target != null ? target.position : transform.position;
    }
#endif

}
