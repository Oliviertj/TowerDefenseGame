using UnityEngine;

public class CannonTower : TowerBase
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _shootPoint;

    public override void Start()
    {
        base.Start();
    }


    protected override void Attack(ITargetable target)
    {
        GameObject projectile = Instantiate(_projectilePrefab, _shootPoint.position, Quaternion.identity);
        SimpleProjectile p = projectile.GetComponent<SimpleProjectile>();
        p.SetTarget(target);
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
