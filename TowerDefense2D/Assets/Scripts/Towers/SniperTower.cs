using UnityEngine;

public class SniperTower : TowerBase
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

        EnemyBase highestHpEnemy = null;
        float highestHp = 0f;

        foreach (var enemy in enemies)
        {
            if (!enemy.IsAlive || !InRange(enemy)) continue;

            IHealth healthComponent = enemy.GetComponent<IHealth>();

            if (healthComponent != null && healthComponent.Current > highestHp)
            {
                highestHp = healthComponent.Current;
                highestHpEnemy = enemy;
            }
        }

        return highestHpEnemy;
    }

}
