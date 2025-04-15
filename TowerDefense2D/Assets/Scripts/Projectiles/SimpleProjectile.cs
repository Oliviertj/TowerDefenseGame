using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;
    private ITargetable target;

    public void SetTarget(ITargetable target)
    {
        this.target = target;
    }

    private void Update()
    {
        if (target == null || !target.IsAlive)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (target.GetTransform().position - transform.position).normalized;
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.GetTransform().position) < 0.1f)
        {
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
