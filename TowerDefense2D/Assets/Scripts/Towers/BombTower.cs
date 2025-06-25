using UnityEngine;

public class BombTower : TowerBase
{
    private ProjectileShooter _projectileShooter;

    public override void Start()
    {
        _projectileShooter = GetComponent<ProjectileShooter>();
        base.Start();
    }

    protected override void Attack(ITargetable target)
    {
        _projectileShooter.Shoot(target, damage);
    }

    protected override ITargetable FindTarget()
    {
        EnemyBase[] enemies = GameObject.FindObjectsOfType<EnemyBase>();
        foreach (var enemy in enemies)
        {
            if (enemy.IsAlive && InRange(enemy))
                return enemy;
        }
        return null;
    }
}
