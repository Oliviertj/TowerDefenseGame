using UnityEngine;

public class SimpleProjectile : MonoBehaviour, IProjectile
{
    [SerializeField] private float _speed = 25f;
    [SerializeField] private float _lifetime = 5f;

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
            _target.TakeDamage(_damage);
            Destroy(gameObject);
        }

        _lifetime -= Time.deltaTime;
        if (_lifetime <= 0f)
            Destroy(gameObject);
    }
}
