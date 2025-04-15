using UnityEngine;

public class Splatty : EnemyBase
{
    protected override void Start()
    {
        base.Start();
        moveSpeed = 2f;
        maxHealth = 3f;
        currentHealth = maxHealth;

        SetColor(new Color(1f, 0.4f, 0.4f)); // lichtrood
    }
}
