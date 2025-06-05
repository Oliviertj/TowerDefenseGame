using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;

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
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _target.GetTransform().position) < 0.1f)
        {
            _target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
