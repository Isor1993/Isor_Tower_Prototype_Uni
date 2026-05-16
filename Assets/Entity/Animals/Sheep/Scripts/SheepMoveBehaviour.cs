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
    NavMeshPath _path;
    private float _followDistance = 2f;
    //_______Auslagern in setting viel._____________    
    [SerializeField] private float _sampleradius = 2f;
    [SerializeField] private float _minFleeDistance = 6f;
    [SerializeField] private float _maxFleeDistance = 10f;
    [SerializeField] private float _fleeDistanceSideOffset = 10f;
    [SerializeField] private int _maxSearchTrys = 100;


    private Vector3 _fleeDirection;







    //_____________________

    private void Awake()
    {

        _agent = GetComponent<NavMeshAgent>();
        _path = new NavMeshPath();
        _minFleeDistance = _settings.MinFleeDistance;
        _maxFleeDistance = _settings.MaxFleeDistance;
        _fleeDistanceSideOffset = _settings.FleeDistanceSideOffset;
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
    public Vector3 FleeFrom(Vector3 threatPosition)
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

            if (!_agent.CalculatePath(hit.position, _path))
                continue;

            if (_path.status != NavMeshPathStatus.PathComplete)
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
