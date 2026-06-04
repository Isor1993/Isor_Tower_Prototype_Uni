using UnityEngine;

public abstract class DodgeBehaviourBase:MonoBehaviour
{
    public abstract bool ShouldDodge { get; }
    public abstract bool IsDodging { get; }

    public abstract bool TryStartDodge(out Vector3 previousTarget); 
    
}
