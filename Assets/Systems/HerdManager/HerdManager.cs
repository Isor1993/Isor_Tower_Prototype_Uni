
using System.Collections.Generic;
using UnityEngine;


public class HerdManager : MonoBehaviour
{
    [Header("Herd Members")]
    [Tooltip("The sheep that leads this herd.")]
    [SerializeField] private Sheep _commander;
    [Tooltip("All sheep that belong to this herd.")]
    [SerializeField] private List<Sheep> _herdPool = new List<Sheep>();

    [SerializeField] private float _regroupRadius;
    [SerializeField] private float _patrolRadius;
    [SerializeField] private Vector3[] _slotOffsets;
    [Header("Gizmo Settings")]
    [SerializeField] private bool _showGizmo = false;
    [SerializeField] private Color _patrolRadiusColor;



    public bool IsCommanderAlive => _commander != null && _commander.IsAlive;
    public bool HasCommander => _commander != null;

    private Vector3 _lastHerdAnchorPosition;

    private void Awake()
    {
        ValidateHerd();

    }

    public Vector3 GetHerdAnchorPosition()
    {
        if (IsCommanderAlive)
        {
            return _lastHerdAnchorPosition = _commander.transform.position;
        }
        if (HasAliveSheep())
        {
            return _lastHerdAnchorPosition = CalculateHerdCenter();
        }
        return _lastHerdAnchorPosition;
    }

    private bool HasAliveSheep()
    {
        foreach (Sheep sheep in _herdPool)
        {
            if (sheep == null)
                continue;
            if (sheep.IsAlive)
            {
                return true;
            }
        }
        return false;
    }


    private Vector3 CalculateHerdCenter()
    {
        Vector3 sum = Vector3.zero;
        int sheepCount = 0;
        foreach (Sheep sheep in _herdPool)
        {
            if (sheep == null)
                continue;
            if (!sheep.IsAlive)
                continue;
            sheepCount++;
            sum += sheep.transform.position;
        }
        if (sheepCount <= 0)
        {
            return _lastHerdAnchorPosition;
        }
        Vector3 herdCenter = sum / sheepCount;
        return herdCenter;
    }
    private void ValidateHerd()
    {
        if (_commander == null)
        {
            Debug.LogWarning($"{name}: No commander assigned.");
        }
        else
        {
            if (!_commander.IsCommander)
            {
                Debug.LogWarning($"{name}: Assigned commander {_commander.name} is not marked as commander or is null.");
            }

            if (!_herdPool.Contains(_commander))
            {
                Debug.LogWarning($"{name}: Commander {_commander.name} is not part of the herd pool.");
            }
        }

        for (int i = 0; i < _herdPool.Count; i++)
        {
            if (_herdPool[i] == null)
            {
                Debug.LogWarning($"{name}: Herd pool contains an empty slot at index {i}.");
            }
        }
    }


    public Vector3 GetFormationPositionForNormalSheep(Sheep sheep)
    {
        Vector3 anchor = GetHerdAnchorPosition();

        if (sheep == null)
            return anchor;

        if (!sheep.IsAlive)
            return sheep.transform.position;

        int aliveIndex = GetAliveSheepIndex(sheep);

        if (aliveIndex < 0)
        {
            Debug.LogError($"{name}: Sheep is not an alive member of this herd.");
            return anchor;
        }

        if (_slotOffsets == null || _slotOffsets.Length == 0)
        {
            Debug.LogError($"{name}: No slot offsets assigned.");
            return anchor;
        }

        int slotIndex = aliveIndex;

        if (slotIndex >= _slotOffsets.Length)
        {
            Debug.LogError($"{name}: Slot index is outside of the slot offset array.");
            return anchor;
        }

        Vector3 targetPosition = anchor + _slotOffsets[slotIndex];
        return targetPosition;
    }

    public Vector3 GetRandomRegroupPosition()
    {
        Vector3 anchor = GetHerdAnchorPosition();

        Vector2 randomCircle = Random.insideUnitCircle * _regroupRadius;
        Vector3 randomOffset = new Vector3(randomCircle.x, 0f, randomCircle.y);

        return anchor + randomOffset;
    }
    public Vector3 GetRandomPatrolPosition()
    {
        Vector3 anchor = GetHerdAnchorPosition();
        Vector2 randomCircle = Random.insideUnitCircle * _patrolRadius;
        Vector3 randomOffset = new Vector3(randomCircle.x, 0, randomCircle.y);
        return anchor + randomOffset;

    }

    private int GetAliveSheepIndex(Sheep targetSheep)
    {
        int aliveIndex = 0;

        foreach (Sheep sheep in _herdPool)
        {
            if (sheep == null)
                continue;

            if (!sheep.IsAlive)
                continue;

            if (sheep == targetSheep)
                return aliveIndex;

            aliveIndex++;
        }

        return -1;
    }
    public bool AreAllSheepInPosition()
    {
        for (int i = 0; i < _herdPool.Count; i++)
        {
            Sheep sheep = _herdPool[i];

            if (sheep == null)
                continue;
            if (!sheep.IsAlive)
                continue;
            if (sheep.Move == null)
                continue;

            if (!sheep.Move.HasReachedDestination())
                return false;
        }

        return true;
    }

    private void OnDrawGizmos()
    {
        if (_showGizmo)
        {
            Gizmos.color = _patrolRadiusColor;
            Gizmos.DrawWireSphere(_lastHerdAnchorPosition, _patrolRadius);
        }
    }
}