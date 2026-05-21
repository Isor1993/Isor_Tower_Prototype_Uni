
using System.Collections.Generic;
using UnityEngine;


public class HerdManager : MonoBehaviour
{
    [Header("Herd Members")]
    [Tooltip("The sheep that leads this herd.")]
    [SerializeField] private Sheep _commander;
    [Tooltip("All sheep that belong to this herd.")]
    [SerializeField] private List<Sheep> _herdPool = new List<Sheep>();

    [SerializeField] private Vector3[] _slotOffsets;
    [SerializeField] private float _regroupRadius;
    [SerializeField] private float _patrolRadius;
    [SerializeField] private float _spawnRadius;
    [Header("Gizmo Settings")]
    [SerializeField] private bool _showGizmo = false;
    [SerializeField] private Color _patrolRadiusColor;
    [SerializeField] private Color _regroupRadiusColor;
    [SerializeField] private Color _spawnRadiusColor;



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


    public Vector3 GetFormationPositionForSheep(Sheep sheep)
    {
        Vector3 anchor = GetHerdAnchorPosition();

        if (!IsHerdMember(sheep))
            return anchor;

        if (sheep == null)
            return anchor;
        if (!_herdPool.Contains(sheep))
        {
            Debug.LogWarning($"{name}: Sheep {sheep.name} is not part of this herd.");
            return anchor;
        }
        if (sheep == _commander)
            return anchor;

        if (!sheep.IsAlive)
            return sheep.transform.position;

        int aliveIndex = GetAliveNormalSheepIndex(sheep);

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
        return GetRandomPositionInRadius(_regroupRadius);
    }


    public Vector3 GetRandomPatrolPosition()
    {
        return GetRandomPositionInRadius(_patrolRadius);

    }
    public Vector3 GetRandomSpawnPosition()
    {
        return GetRandomPositionInRadius(_spawnRadius);

    }

    private Vector3 GetRandomPositionInRadius(float radius)
    {
        Vector3 anchor = GetHerdAnchorPosition();

        Vector2 randomCircle = Random.insideUnitCircle * radius;
        Vector3 randomOffset = new Vector3(randomCircle.x, 0f, randomCircle.y);

        return anchor + randomOffset;
    }

    private int GetAliveNormalSheepIndex(Sheep targetSheep)
    {
        int aliveIndex = 0;

        foreach (Sheep sheep in _herdPool)
        {
            if (sheep == null)
                continue;

            if (!sheep.IsAlive)
                continue;
            if (sheep == _commander)
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

    public Vector3 GetInactivePosition()
    {
        return Vector3.zero;
    }
    public void NotifySheepRespawned(Sheep sheep)
    {

    }
    public Vector3 GetRespawnPositionForSheep(Sheep sheep)
    {
        if (sheep == null)
            return GetHerdAnchorPosition();
        if (!IsHerdMember(sheep))
            return sheep.transform.position; 
        return GetRandomSpawnPosition();
    }
    private bool IsHerdMember(Sheep sheep)
    {
        return sheep != null && _herdPool.Contains(sheep);
    }


    private void OnDrawGizmos()
    {
        if (_showGizmo)
        {
            Gizmos.color = _patrolRadiusColor;
            Vector3 anchor = Application.isPlaying ? GetHerdAnchorPosition() : transform.position;
            Gizmos.DrawWireSphere(anchor, _patrolRadius);

            Gizmos.color = _spawnRadiusColor;
            Vector3 anchor = Application.isPlaying ? GetHerdAnchorPosition() : transform.position;
            Gizmos.DrawWireSphere(anchor, _spawnRadius);

            Gizmos.color = _regroupRadiusColor;
            Vector3 anchor = Application.isPlaying ? GetHerdAnchorPosition() : transform.position;
            Gizmos.DrawWireSphere(anchor, _regroupRadius);
        }
    }
}