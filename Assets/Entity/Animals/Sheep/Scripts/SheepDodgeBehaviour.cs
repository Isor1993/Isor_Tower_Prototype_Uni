
using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SheepDodgeBehaviour : DodgeBehaviourBase
{
    [Serializable]
    private struct DodgeRaycastSetting
    {
        public string Name;

        public Vector3 LocalDirection;

        public float Distance;

        public LayerMask SearchLayer;
    }


    [Header("Raycast Settings")]
    [SerializeField]
    private List<DodgeRaycastSetting> _raycastSettings = new List<DodgeRaycastSetting>();

    [SerializeField] private float _leftDistance = 3f;
    [SerializeField] private float _forwardDistance = 3f;


    [Header("Gizmos Settings")]
    [SerializeField] private bool _debugGizmosON;
    [SerializeField] private Color _rayColor;




    /// <summary>
    /// 
    /// </summary>
    public override bool ShouldDodge => _obstacleDetected;

    /// <summary>
    /// 
    /// </summary>
    public override bool IsDodging => _isDodging;

    private bool _obstacleDetected = false;
   
    private RaycastHit _obstacleHit;
    private Transform _obstacle;

    private NavMeshAgent _agent;
    private NavMeshPath _navPath;
    private SheepMoveBehaviour _move;

    private Vector3 DrawPoint;
    private bool _isDodging = false;
    private bool _hasDrawPoint = false;

    private Vector3 _previousDestination;
    private bool _hasPreviousDestination;


    private void OnDisable()
    {
        _hasDrawPoint = false;
        DrawPoint = Vector3.zero;
    }



    private void Awake()
    {
        _navPath = new NavMeshPath();
        _agent = GetComponent<NavMeshAgent>();
        _move = GetComponent<SheepMoveBehaviour>();


    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        _obstacleDetected = DetectObstacle();

        if (_isDodging && HasReachedDestination())
        {
            _isDodging = false;
            _hasDrawPoint = false;
            

        }

    }

    public override void StartDodge(out Vector3 previousTarget)
    {
        if(_agent==null)
        {
            previousTarget = transform.position;
            return;
        }

        previousTarget = _agent.destination;
        _hasPreviousDestination = _agent.hasPath;
        if (_isDodging)
            return;
        if (_obstacleHit.transform == null)
            return;
        _isDodging = true;

        _obstacle = _obstacleHit.transform;
        Vector3 obstacleDir = (_obstacle.position - transform.position).normalized;
        obstacleDir.y = 0;

        float sideValue = Vector3.Dot(transform.right, obstacleDir);
        Vector3 dodgeDir = Vector3.zero;
        if (sideValue > 0)
        {
            Debug.Log(sideValue);
            dodgeDir = -transform.right;
        }
        else
        {
            Debug.Log(sideValue);
            dodgeDir = transform.right;
        }


        Vector3 newOffset = dodgeDir * _leftDistance + transform.forward * _forwardDistance;
        Vector3 newPos = transform.position + newOffset;
        if(!NavMesh.SamplePosition(newPos,out NavMeshHit hit,1f,_agent.areaMask))
        {
            Debug.Log("No position to dodge found!");
            return;
        }
        Vector3 validPos = hit.position;
        if(!_agent.CalculatePath(validPos,_navPath))
        {
            Debug.Log("No caluclated path to dodge found!");
            return;
        }
        if(_navPath.status!=NavMeshPathStatus.PathComplete)
        {
            Debug.Log("No complete path to dodge found!");
            return;
        }

        DrawPoint = validPos;
        _hasDrawPoint = true;
        _agent.SetDestination(validPos);
    }














    public bool HasReachedDestination()
    {
        if (_agent == null || !_agent.enabled)
            return true;
        if (_agent.pathPending)
            return false;
        if (_agent.remainingDistance > _agent.stoppingDistance)
            return false;

        return !_agent.hasPath || _agent.velocity.sqrMagnitude <= 0.01f;
    }

    private bool DetectObstacle()
    {
        if (_raycastSettings == null)
        {
            Debug.LogError("Settings Nullreferenze!");
            return false;
        }

        foreach (DodgeRaycastSetting setting in _raycastSettings)
        {
            Vector3 Dir = transform.TransformDirection(setting.LocalDirection.normalized);

            if (Physics.Raycast(transform.position, Dir, out RaycastHit hit, setting.Distance, setting.SearchLayer))
            {
                //Debug.Log($"{setting.Name} hit {hit.collider.name}");
                _obstacleHit = hit;
                return true;
            }
        }
        _obstacleHit = default;
        _obstacle = null;
        return false;
    }

    private void OnDrawGizmos()
    {
        if (_raycastSettings == null)
            return;

        if (_debugGizmosON)
        {
            Gizmos.color = Color.red;
            foreach (DodgeRaycastSetting setting in _raycastSettings)
            {
                Vector3 Dir = transform.TransformDirection(setting.LocalDirection.normalized);
                Vector3 endPoint = transform.position + Dir * setting.Distance;

                Gizmos.DrawLine(transform.position, endPoint);
            }
            if (_hasDrawPoint)
            {

                Gizmos.color = Color.yellow;

                Gizmos.DrawSphere(DrawPoint, 1);
            }
        }
    }
}
