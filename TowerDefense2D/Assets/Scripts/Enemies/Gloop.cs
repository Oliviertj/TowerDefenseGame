using UnityEngine;

public class Gloop : EnemyBase
{
    protected override void Start()
    {
        base.Start();
        moveSpeed = 0.5f;   // Langzaam
        maxHealth = 15f;    // Veel HP
        currentHealth = maxHealth;

        SetColor(new Color(0.8f, 0.4f, 1f)); // paars
    }

    public override void TakeDamage(float amount)
    {
        float reducedDamage = amount * 0.5f; // Neem maar de helft van de schade aangezien Gloop een tanky enemy is
        base.TakeDamage(reducedDamage);
    }
}
