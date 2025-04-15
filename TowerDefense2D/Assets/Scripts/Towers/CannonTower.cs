using UnityEngine;

public class CannonTower : TowerBase
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;

    protected override void Attack(ITargetable target)
    {
        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        SimpleProjectile p = proj.GetComponent<SimpleProjectile>();
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
