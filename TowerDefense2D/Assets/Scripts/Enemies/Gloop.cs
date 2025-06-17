using UnityEngine;

public class Gloop : EnemyBase
{
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount * 0.5f); // Gloop is tanky
    }
}
