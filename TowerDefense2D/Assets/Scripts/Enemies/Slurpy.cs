using UnityEngine;

public class Slurpy : EnemyBase
{
    protected override void Start()
    {
        base.Start();
        moveSpeed = 1.25f;
        livesLost = 1f;
        enemyWeight = 2f;

        // Pas max health aan via het IHealth component
        if (health is BasicHealth basicHealth)
        {
            basicHealth.SetMaxHealth(3f);
        }

        SetColor(new Color(1f, 0.4f, 0.4f)); // lichtrood
    }
}
