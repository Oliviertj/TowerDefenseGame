using UnityEngine;

public class Slimey : EnemyBase
{
    protected override void Start()
    {
        base.Start();
        moveSpeed = 1.5f;
        SetColor(new Color(0.4f, 1f, 0.4f)); // lichtgroen
    }
}
