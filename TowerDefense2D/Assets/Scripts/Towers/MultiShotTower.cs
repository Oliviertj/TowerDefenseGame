using UnityEngine;
using System.Collections.Generic;

public class MultiShotTower : TowerBase
{
    [SerializeField] private int shotsPerAttack = 3;
    private ProjectileShooter _projectileShooter;

    public override void Start()
    {
        _projectileShooter = GetComponent<ProjectileShooter>();
        base.Start();
    }

    protected override void Attack(ITargetable target)
    {
        List<EnemyBase> targets = FindMultipleTargets(shotsPerAttack);

        foreach (var t in targets)
        {
            _projectileShooter.Shoot(t, damage);
        }
    }

    protected override ITargetable FindTarget()
    {
        EnemyBase[] enemies = GameObject.FindObjectsOfType<EnemyBase>();

        foreach (var enemy in enemies)
        {
            if (enemy.IsAlive && InRange(enemy))
            {
                return enemy; // we gebruiken dit alleen als trigger
            }
        }

        return null;
    }


    private List<EnemyBase> FindMultipleTargets(int count)
    {
        EnemyBase[] enemies = GameObject.FindObjectsOfType<EnemyBase>();
        List<EnemyBase> result = new List<EnemyBase>();

        foreach (var enemy in enemies)
        {
            if (enemy.IsAlive && InRange(enemy))
            {
                result.Add(enemy);
                if (result.Count >= count)
                    break;
            }
        }

        return result;
    }
}
