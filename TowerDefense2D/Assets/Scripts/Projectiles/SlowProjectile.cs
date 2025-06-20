using UnityEngine;

public class SlowProjectile : MonoBehaviour, IProjectile
{
    [SerializeField] private float _speed = 15f;
    [SerializeField] private float _lifetime = 5f;
    [SerializeField] private float _slowAmount = 0.5f;
    [SerializeField] private float _slowDuration = 2f;

    private ITargetable _target;
    private float _damage;

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

        Vector2 dir = (_target.GetTransform().position - transform.position).normalized;
        transform.position += (Vector3)(dir * _speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _target.GetTransform().position) < 0.3f)
        {
            if (_target is EnemyBase enemy)
            {
                enemy.ApplySlow(_slowAmount, _slowDuration);
                enemy.TakeDamage(_damage);
            }

            Destroy(gameObject);
        }

        _lifetime -= Time.deltaTime;
        if (_lifetime <= 0f)
            Destroy(gameObject);
    }
}
