using UnityEngine;

public class Slimey : EnemyBase
{
    protected override void Start()
    {
        base.Start();
        SetColor(new Color(0.4f, 1f, 0.4f)); // lichtgroen
    }
}
