using UnityEngine;

public class Gloop : EnemyBase
{
    protected override void Start()
    {
        base.Start();
        moveSpeed = 0.5f;

        if (health is BasicHealth basicHealth)
        {
            basicHealth.SetMaxHealth(15f);
        }

        SetColor(new Color(0.8f, 0.4f, 1f)); // paars
    }

    public override void TakeDamage(float amount)
    {
        float reducedDamage = amount * 0.5f;
        base.TakeDamage(reducedDamage);
    }
}
