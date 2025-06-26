using UnityEngine;

/// <summary>
/// Abstracte basisclass voor alle torens. Zorgt voor schieten, target zoeken,
/// stun-effect en rotatie van de barrel.
/// </summary>
public abstract class TowerBase : MonoBehaviour, IStunnable
{
    [SerializeField] protected float range = 5f;
    [SerializeField] protected float fireRate = 1f;
    [SerializeField] protected float damage = 1f;

    protected float fireCooldown;
    protected float fireRateMultiplier = 1f;
    protected ITargetable currentTarget;

    private BarrelRotator _barrelRotator;
    private bool _isStunned;
    private float _stunTimer;

    /// <summary>
    /// Haalt de BarrelRotator component op bij de start van het spel.
    /// </summary>
    public virtual void Start()
    {
        _barrelRotator = GetComponent<BarrelRotator>();
    }

    /// <summary>
    /// Stuurt het gedrag van de toren aan zoals target zoeken, aanvallen en stun-timers.
    /// </summary>
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
            fireCooldown = 1f / (fireRate * fireRateMultiplier);
        }

        if (currentTarget != null && _barrelRotator != null)
        {
            _barrelRotator.SetTarget(currentTarget.GetTransform());
        }
    }

    /// <summary>
    /// Past tijdelijk een vermenigvuldiging toe op de vuursnelheid.
    /// </summary>
    /// <param name="multiplier">De factor waarmee de vuursnelheid wordt vermenigvuldigd.</param>
    public void ApplyFireRateMultiplier(float multiplier)
    {
        fireRateMultiplier = multiplier;
    }

    /// <summary>
    /// Stunt de toren voor een bepaalde duur, waardoor deze tijdelijk niet kan aanvallen.
    /// </summary>
    /// <param name="duration">De duur van de stun in seconden.</param>
    public void Stun(float duration)
    {
        _isStunned = true;
        _stunTimer = duration;
    }

    /// <summary>
    /// Controleert of het doelwit zich binnen het bereik van de toren bevindt.
    /// </summary>
    /// <param name="target">Het doelwit om te controleren.</param>
    /// <returns>True als het doelwit binnen bereik is, anders false.</returns>
    protected bool InRange(ITargetable target)
    {
        return Vector2.Distance(transform.position, target.GetTransform().position) <= range;
    }

    /// <summary>
    /// Abstracte methode voor het uitvoeren van een aanval op een doelwit.
    /// Moet geïmplementeerd worden door afgeleide torens.
    /// </summary>
    /// <param name="target">Het doelwit om aan te vallen.</param>
    protected abstract void Attack(ITargetable target);

    /// <summary>
    /// Abstracte methode voor het vinden van een geschikt doelwit.
    /// Moet geïmplementeerd worden door afgeleide torens.
    /// </summary>
    /// <returns>Een geldig doelwit (ITargetable) of null als er geen doel is.</returns>
    protected abstract ITargetable FindTarget();
}
