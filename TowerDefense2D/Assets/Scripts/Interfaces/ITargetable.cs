using UnityEngine;
public interface ITargetable
{
    void TakeDamage(float amount);
    Transform GetTransform();
    bool IsAlive { get; }
}
