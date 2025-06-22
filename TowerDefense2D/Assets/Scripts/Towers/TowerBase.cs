using UnityEngine;

public abstract class TowerBase : MonoBehaviour, IStunnable
{
    [SerializeField] protected float range = 5f;
    [SerializeField] protected float fireRate = 1f;
    [SerializeField] protected float damage = 1f;

    protected float fireCooldown;
    protected ITargetable currentTarget;
    private BarrelRotator _barrelRotator;

    private bool _isStunned;
    private float _stunTimer;

    public virtual void Start()
    {
        _barrelRotator = GetComponent<BarrelRotator>();
    }
    protected virtual void Update()
    {
        if (_isStunned)
        {
            _stunTimer -= Time.deltaTime;
            if (_stunTimer <= 0)
                _isStunned = false;
            return;
        }
        fireCooldown -= Time.deltaTime;

        if (currentTarget == null || !currentTarget.IsAlive || !InRange(currentTarget))
            currentTarget = FindTarget();

        if (currentTarget != null && fireCooldown <= 0f)
        {
            Attack(currentTarget);
            fireCooldown = 1f / fireRate;
        }
        if (currentTarget != null && _barrelRotator != null)
        {
            _barrelRotator.SetTarget(currentTarget.GetTransform());
        }

    }

    public void Stun(float duration)
    {
        _isStunned = true;
        _stunTimer = duration;
        // optioneel: toon visueel dat hij gestunnd is
    }

    protected bool InRange(ITargetable target)
    {
        return Vector2.Distance(transform.position, target.GetTransform().position) <= range;
    }
    /// <summary>
    /// Abstract Methods om ervoor te zorgen dat ik die in elke subklasse niet vergeet te maken aangezien ze altijd nodig zijn als ik een tower maak.
    /// </summary>
    protected abstract void Attack(ITargetable target);
    protected abstract ITargetable FindTarget();
}
