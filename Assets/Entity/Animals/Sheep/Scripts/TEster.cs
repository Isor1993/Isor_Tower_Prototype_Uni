using UnityEditor;
using UnityEngine;

public class TEster : MonoBehaviour
{
    private  Timer _timer;
    [SerializeField] private bool isUpdtate=false;
    [SerializeField,Range(0.01f,2f)] private float UpdateTime =1f;
    [SerializeField] private Transform _threat;
    [SerializeField] private SheepMoveBehaviour _move;
    [SerializeField] private SheepSense _sense;
    private Vector3 _fleePosition=Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        _timer=new Timer();
        _timer.Reset();
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timer.Tick(Time.deltaTime);
        if (isUpdtate)
        {
            if (_sense.HasThreat)
            {
                
                
                    _timer.Reset();
                    _fleePosition = _move.AwayFrom(_threat.position);
                
            }
        }
    }



    private void OnDrawGizmos()
    {
        if (_fleePosition == Vector3.zero)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_fleePosition, 0.2f);
    }
}
