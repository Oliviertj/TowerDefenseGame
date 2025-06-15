using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _damage = 10f;

    [SerializeField] private float _lifetime = 5f;


    private ITargetable _target;

    public void SetTarget(ITargetable target)
    {
        this._target = target;
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

        if (Vector2.Distance(transform.position, _target.GetTransform().position) < 0.1f)
        {
            _target.TakeDamage(_damage);
            Destroy(gameObject);
        }

        _lifetime -= Time.deltaTime;
        if (_lifetime <= 0f)
        {
            Destroy(gameObject);
        }

    }
}
