/*****************************************************************************
* Project : Isors Tower Prototype
* File    : SheepMoveBehaviour.cs
* Date    : 20.02.2026
* Author  : Eric Rosenberg
*
* Description :
* 
* 
* 
* 
*
* History :
* 20.02.2026 ER Created
******************************************************************************/
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class SheepMoveBehaviour : MonoBehaviour
{
    [SerializeField] private SheepSettings _settings;
    private NavMeshAgent _agent;
    NavMeshPath _fleepath;
    NavMeshPath _herdpath;
    private float _followDistance = 2f;




    [Tooltip(""), Range(1, 4)]
    [SerializeField] private float _sampleradius = 2f;
    [Tooltip(""), Range(4, 20)]
    [SerializeField] private float _minFleeDistance = 6f;
    [Tooltip(""), Range(4, 40)]
    [SerializeField] private float _maxFleeDistance = 10f;
    [Tooltip(""), Range(1, 20)]
    [SerializeField] private float _fleeDistanceSideOffset = 10f;
    [Tooltip(""), Range(1, 1000)]
    [SerializeField] private int _maxSearchTrys = 100;

    [Header("Movement Settings")]
    [SerializeField] private float _walkSpeed = 2f;
    [SerializeField] private float _walkAcceleration = 4f;
    [SerializeField] private float _walkAngularSpeed = 120f;

    [Header("Flee Movement Settings")]
    [SerializeField] private float _fleeSpeed = 6f;
    [SerializeField] private float _fleeAcceleration = 14f;
    [SerializeField] private float _fleeAngularSpeed = 360f;


    private void Awake()
    {

        _agent = GetComponent<NavMeshAgent>();
        _fleepath = new NavMeshPath();
        _herdpath = new NavMeshPath();
        SetBaseValues();
    }

    private void SetBaseValues()
    {
        _minFleeDistance = _settings.MinFleeDistance;
        _maxFleeDistance = _settings.MaxFleeDistance;
        _fleeDistanceSideOffset = _settings.FleeDistanceSideOffset;
        _walkSpeed = _settings.WalkSpeed;
        _walkAcceleration = _settings.WalkAcceleration;
        _walkAngularSpeed = _settings.WalkAngularSpeed;
        _fleeSpeed = _settings.FleeSpeed;
        _fleeAcceleration = _settings.FleeAcceleration;
        _fleeAngularSpeed = _settings.FleeAngularSpeed;
    }

    private void Update()
    {
        if (_maxFleeDistance < _minFleeDistance)
        {
            _maxFleeDistance = _minFleeDistance + 1;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetPosition"></param>
    public void MoveTo(Vector3 targetPosition)
    {
        _agent.isStopped = false;
        _agent.SetDestination(targetPosition);
    }

   
    public Vector3 GetValidHerdPosition(Sheep sheep)
    {
        Vector3 herdPosition = sheep.HerdManager.GetFormationPositionForSheep(sheep);

        if (!NavMesh.SamplePosition(herdPosition, out NavMeshHit hit, _sampleradius, _agent.areaMask))
        {
            return transform.position;
        }
        if (!_agent.CalculatePath(hit.position, _herdpath))
            return transform.position;

        if (_herdpath.status != NavMeshPathStatus.PathComplete)
            return transform.position;

        return hit.position;    
    }


public void StopMoving()
{
    _agent.isStopped = true;
    _agent.ResetPath();
}

public void Follow(Transform target)
{
    if (target == null)
        return;
    MoveTo(target.position);
}

public bool HasReachedDestination()
{
    if (_agent.pathPending)
        return false;

    return _agent.remainingDistance <= _agent.stoppingDistance;
}

/// <summary>
/// 
/// </summary>
/// <param name="threatPosition"></param>
public Vector3 AwayFrom(Vector3 threatPosition)
{
    Vector3 targetPosition = TryGetBestFleeTarget(threatPosition);
    MoveTo(targetPosition);


    return targetPosition;
}
private Vector3 GetFleeDirection(Vector3 threatPosition)
{
    Vector3 fleeDirection = (transform.position - threatPosition).normalized;
    return fleeDirection;
}
public void SetWalkMovement()
{
    _agent.speed = _settings.WalkSpeed;
    _agent.acceleration = _settings.WalkAcceleration;
    _agent.angularSpeed = _settings.WalkAngularSpeed;
}

public void SetFleeMovement()
{
    _agent.speed = _settings.FleeSpeed;
    _agent.acceleration = _settings.FleeAcceleration;
    _agent.angularSpeed = _settings.FleeAngularSpeed;
}

public Vector3 TryGetBestFleeTarget(Vector3 threat)
{
    float currentDistanceToThreat = Vector3.Distance(transform.position, threat);
    for (int i = 0; i < _maxSearchTrys; i++)
    {
        Vector3 randomPosition = GetRandomFleePosition(threat);

        if (!NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, _sampleradius, _agent.areaMask))
        {
            continue;
        }

        float distanceToSheep = Vector3.Distance(transform.position, hit.position);

        if (distanceToSheep < _minFleeDistance)
            continue;

        float targetDistanceToThreat = Vector3.Distance(threat, hit.position);

        if (targetDistanceToThreat <= currentDistanceToThreat)
            continue;

        if (!_agent.CalculatePath(hit.position, _fleepath))
            continue;

        if (_fleepath.status != NavMeshPathStatus.PathComplete)
            continue;

        return hit.position;

    }
    return transform.position;
}

private Vector3 GetRandomFleePosition(Vector3 threat)
{
    Vector3 fleeDirection = GetFleeDirection(threat);

    float distanceOffset = Random.Range(_minFleeDistance, _maxFleeDistance);

    Vector3 newFleePosition = transform.position + fleeDirection * distanceOffset;

    Vector3 sideDirection = Vector3.Cross(Vector3.up, fleeDirection).normalized;

    float sideOffset = Random.Range(-_fleeDistanceSideOffset, _fleeDistanceSideOffset);

    newFleePosition += sideDirection * sideOffset;

    newFleePosition.y = transform.position.y;

    Debug.Log($"Random flee candidate: {newFleePosition}");
    return newFleePosition;
}

}
