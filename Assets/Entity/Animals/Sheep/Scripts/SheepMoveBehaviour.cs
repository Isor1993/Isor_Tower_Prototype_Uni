
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SheepMoveBehaviour : MonoBehaviour
{

    private NavMeshAgent _agent;
    private float _fleeDistance=2f;
    private float _followDistance=2f;


    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 targetPosition)
    {     
        _agent.isStopped = false;
        _agent.SetDestination(targetPosition);
    }

    public void FleeFrom(Vector3 threatPosition)
    {
        _agent.isStopped=false;
        Vector3 fleeDirection = (transform.position - threatPosition).normalized;
        Vector3 fleeTarget= transform.position -(fleeDirection*_fleeDistance);

        MoveTo(fleeTarget);
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
}
