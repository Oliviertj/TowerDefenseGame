using UnityEngine;

public class ExplodingProjectile : MonoBehaviour, IProjectile
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifetime = 5f;
    [SerializeField] private float _blastRadius = 2f;
    [SerializeField] private float _damageToOthers = 0.5f;
    [SerializeField] private GameObject _explosionEffectPrefab;
    [SerializeField] private LayerMask _enemyLayer;

    private float _damage;
    private ITargetable _target;

    public void SetTarget(ITargetable target)
    {
        _target = target;
    }

    public void SetDamage(float amount)
    {
        _damage = amount;
    }

    private void Update()
    {
        if (_target == null || !_target.IsAlive)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (_target.GetTransform().position - transform.position).normalized;
        transform.position += (Vector3)(direction * _speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _target.GetTransform().position) < 0.3f)
        {
            Explode();
            Destroy(gameObject);
        }

        _lifetime -= Time.deltaTime;
        if (_lifetime <= 0f)
            Destroy(gameObject);
    }

    private void Explode()
    {
        // Spawn visueel effect
        if (_explosionEffectPrefab != null)
        {
            GameObject effect = Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);
            if (effect.TryGetComponent(out HealingAuraVisual aura))
            {
                aura.SetRadius(_blastRadius);
                aura.Show();
            }
            else
            {
                Debug.LogWarning("ExplosionEffect mist HealingAuraVisual component.");
            }
        }

        // Schade aan hoofdtarget
        if (_target is EnemyBase mainTarget)
        {
            mainTarget.TakeDamage(_damage);
        }

        // Schade aan vijanden in blast radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _blastRadius, _enemyLayer);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out EnemyBase nearbyEnemy) && nearbyEnemy.gameObject != _target.GetTransform().gameObject)
            {
                nearbyEnemy.TakeDamage(_damageToOthers);
            }
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _blastRadius);
    }
#endif
}
