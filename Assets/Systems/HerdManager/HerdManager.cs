
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HerdManager : MonoBehaviour
{
    [Header("Herd References")]
    [Tooltip("The sheep that leads this herd.")]
    [SerializeField] private Sheep _commander;

    [Tooltip("All sheep that belong to this herd.")]
    [SerializeField] private List<Sheep> _herdPool = new List<Sheep>();
    [SerializeField] private Vector3[] _slotOffsets;

    public Sheep CommanderSheep => _commander;
    public IReadOnlyList<Sheep> HerdPool => _herdPool;

    public bool HasCommander => _commander != null;

    public Vector3 CommanderPosition
    {
        get
        {
            if (_commander == null)
            {
                return transform.position;
            }

            return _commander.transform.position;
        }
    }

    private void Awake()
    {
        ValidateHerd();
    }

    private void ValidateHerd()
    {
        if (_commander == null)
        {
            Debug.LogWarning($"{name}: No commander assigned.");
            return;
        }

        if (!_commander.IsCommander)
        {
            Debug.LogWarning($"{name}: Assigned commander {_commander.name} is not marked as commander.");
        }

        if (!_herdPool.Contains(_commander))
        {
            Debug.LogWarning($"{name}: Commander {_commander.name} is not part of the herd pool.");
        }

        for (int i = 0; i < _herdPool.Count; i++)
        {
            if (_herdPool[i] == null)
            {
                Debug.LogWarning($"{name}: Herd pool contains an empty slot at index {i}.");
            }
        }
    }

    public bool IsSheepInHerd(Sheep sheep)
    {
        if (sheep == null)
        {
            return false;
        }

        return _herdPool.Contains(sheep);
    }
    public Vector3 GetHerdPositionForSheep(Sheep sheep)
    {
        if (sheep == null)
            return _commander.transform.position;

        int sheepIndex=_herdPool.IndexOf(sheep);
        if (sheepIndex < 0)
        {
            Debug.LogWarning($"{name}: Sheep is not part of this herd.");
            return _commander.transform.position;
        }

        Vector3 targetPosition= _commander.transform.position+_slotOffsets[sheepIndex];

        return targetPosition;
        

      

    }
}