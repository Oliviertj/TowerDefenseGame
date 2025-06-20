using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform[] _shootPoints;

    private int _currentShootIndex = 0;

    public void Shoot(ITargetable target, float damage)
    {
        if (_shootPoints.Length == 0)
        {
            Debug.LogWarning("Geen shoot points ingesteld!");
            return;
        }

        Transform shootPoint = _shootPoints[_currentShootIndex % _shootPoints.Length];
        _currentShootIndex++;

        GameObject proj = Instantiate(_projectilePrefab, shootPoint.position, Quaternion.identity);

        if (proj.TryGetComponent(out IProjectile projectile))
        {
            projectile.SetTarget(target);
            projectile.SetDamage(damage);
        }
        else
        {
            Debug.LogWarning("Projectile prefab mist een component dat IProjectile implementeert.");
        }
    }
}
