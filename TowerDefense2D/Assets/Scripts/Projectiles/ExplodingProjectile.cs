using UnityEngine;

/// <summary>
/// Projectile die naar een doel beweegt, explodeert bij aankomst en schade toebrengt in een straal.
/// </summary>
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

    /// <summary>
    /// Stelt het doel van het projectile in.
    /// </summary>
    /// <param name="target">Het doelwit</param>
    public void SetTarget(ITargetable target)
    {
        _target = target;
    }

    /// <summary>
    /// Stelt de schade in die het projectile toebrengt aan de target.
    /// </summary>
    /// <param name="amount">De hoeveelheid schade.</param>
    public void SetDamage(float amount)
    {
        _damage = amount;
    }

    /// <summary>
    /// Beweegt het projectile richting het doel en vernietigt het als het doel is bereikt of de tijd is verlopen.
    /// </summary>
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

    /// <summary>
    /// Laat het projectile exploderen, brengt schade toe aan het hoofdtarget en aan andere vijanden binnen de explosiestraal.
    /// </summary>
    private void Explode()
    {
        // Spawn visueel effect van de explosie
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

        // Schade aan vijanden binnen de blast radius behalve het hoofdtarget
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _blastRadius, _enemyLayer);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out EnemyBase nearbyEnemy) && nearbyEnemy.gameObject != _target.GetTransform().gameObject)
            {
                nearbyEnemy.TakeDamage(_damageToOthers);
            }
        }
    }
}
