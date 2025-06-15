using UnityEngine;

public class Splatty : EnemyBase
{
    protected override void Start()
    {
        base.Start();
        moveSpeed = 3f;
        livesLost = 2f;
        enemyWeight = 2f;

        // Pas max health aan via het IHealth component
        if (health is BasicHealth basicHealth)
        {
            basicHealth.SetMaxHealth(2f);
        }

        SetColor(new Color(1f, 0.4f, 0.4f)); // lichtrood
    }
}
