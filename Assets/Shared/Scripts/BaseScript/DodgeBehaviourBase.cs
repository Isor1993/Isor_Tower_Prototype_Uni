using UnityEngine;

public abstract class DodgeBehaviourBase:MonoBehaviour
{
    public abstract bool ShouldDodge { get; }
    public abstract bool IsDodging { get; }

    public abstract void StartDodge(out Vector3 previousTarget); 
    
}
