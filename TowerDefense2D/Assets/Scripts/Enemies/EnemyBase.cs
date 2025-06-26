using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyBase : MonoBehaviour, ITargetable
{
    [Header("Stats")]
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float livesLost = 1f;
    [SerializeField] protected float enemyWeight = 1f;
    [SerializeField] public float maxHealth = 5f;

    [Header("SlowProjectile")]
    private float _originalSpeed;
    private float _slowTimer;

    public float EnemyWeight => enemyWeight;

    [Header("Visual")]
    [SerializeField] protected Color enemyColor = Color.white;

    [Header("Path following")]
    [SerializeField] private List<Vector2> _path = new List<Vector2>();
    protected int _currentPathIndex = 0;

    protected SpriteRenderer spriteRenderer;
    protected IHealth health;

    public IHealth Health => health;

    private Coroutine slowRoutine;
    public bool IsAlive => health != null && health.Current > 0f;

    private void Awake()
    {
    }

    /// <summary>
    /// Wordt 1 keer aangeroepen bij het starten.
    /// Initialiseert de gezondheid, kleur, pad en voegt een healthbar toe.
    /// </summary>
    protected virtual void Start()
    {
        _originalSpeed = moveSpeed;

        health = GetComponent<IHealth>();
        if (health == null)
            Debug.LogWarning($"{gameObject.name} heeft geen IHealth component!");
        if (health is BasicHealth basicHealth)
            basicHealth.SetMaxHealth(maxHealth);
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.color = enemyColor;
        else
            Debug.LogWarning($"Enemy '{gameObject.name}' mist een SpriteRenderer component.");

        if (_path.Count == 0)
            Debug.LogWarning("Path is not set for this enemy.");

        GameObject healthBar = Instantiate(Resources.Load<GameObject>("HealthBar"), transform);
        healthBar.GetComponent<HealthBar>().Initialize(health);
    }

    /// <summary>
    /// Wordt elke frame aangeroepen.
    /// Zorgt voor het bewegen langs het pad en reset de snelheid na slow effecten.
    /// </summary>
    protected virtual void Update()
    {
        if (_path.Count > 0)
            MoveAlongPath();

        // Reset snelheid na vertraging
        if (_slowTimer > 0f)
        {
            _slowTimer -= Time.deltaTime;
            if (_slowTimer <= 0f)
            {
                moveSpeed = _originalSpeed;
            }
        }
    }

    /// <summary>
    /// Beweegt de vijand stap voor stap naar het volgende punt in het pad.
    /// Als het pad voltooid is, sterft de vijand met 'reachedGoal' op true.
    /// </summary>
    protected virtual void MoveAlongPath()
    {
        if (_currentPathIndex < _path.Count)
        {
            Vector3 currentPos = transform.position;
            Vector2 target2D = _path[_currentPathIndex];
            Vector3 targetPos = new Vector3(target2D.x, target2D.y, currentPos.z);

            transform.position = Vector3.MoveTowards(currentPos, targetPos, moveSpeed * Time.deltaTime);

            if ((Vector2)transform.position == target2D)
                _currentPathIndex++;
        }
        else
        {
            Die(reachedGoal: true);
        }
    }

    /// <summary>
    /// Past een slow effect toe op de vijand, mits deze niet immuun is.
    /// </summary>
    /// <param name="amount">Hoeveel procent de snelheid wordt verminderd (0-1).</param>
    /// <param name="duration">Duur van het slow effect in seconden.</param>
    public virtual void ApplySlow(float amount, float duration)
    {
        if (GetComponent<SlowResistance>() != null)
        {
            Debug.Log($"{gameObject.name} is immuun voor slow.");
            return;
        }

        if (slowRoutine != null)
            StopCoroutine(slowRoutine);

        slowRoutine = StartCoroutine(ApplySlowCoroutine(amount, duration));
    }

    /// <summary>
    /// Coroutine die de snelheid tijdelijk verlaagt en daarna terugzet naar origineel.
    /// </summary>
    /// <param name="amount">Percentage waarmee snelheid verlaagd wordt.</param>
    /// <param name="duration">Duur van het slow effect in seconden.</param>
    private IEnumerator ApplySlowCoroutine(float amount, float duration)
    {
        moveSpeed = _originalSpeed * (1f - Mathf.Clamp01(amount));

        yield return new WaitForSeconds(duration);

        moveSpeed = _originalSpeed;
        slowRoutine = null;
    }

    /// <summary>
    /// Veroorzaakt schade aan de vijand en controleert of deze dood is.
    /// </summary>
    /// <param name="amount">Hoeveelheid schade die wordt toegebracht.</param>
    public virtual void TakeDamage(float amount)
    {
        if (health == null) return;

        health.TakeDamage(amount);

        // Check handmatig of dood, en sterf als dat zo is
        if (health.Current <= 0f)
        {
            Die(reachedGoal: false);
        }
    }

    /// <summary>
    /// Verwijdert de vijand uit de scene. Indien bereikt doel, wordt LoseLives getriggerd.
    /// </summary>
    /// <param name="reachedGoal">Geeft aan of de vijand het doel heeft bereikt.</param>
    public virtual void Die(bool reachedGoal = false)
    {
        Debug.Log($"Enemy {gameObject.name} died. ReachedGoal = {reachedGoal}");
        if (reachedGoal)
            LifeManager.Instance?.LoseLives(livesLost);

        Destroy(gameObject);
    }

    /// <summary>
    /// Stelt het pad in dat de vijand moet volgen.
    /// </summary>
    /// <param name="newPath">Lijst van punten waarlangs bewogen wordt.</param>
    public void SetPath(List<Vector2> newPath)
    {
        _path = newPath;
    }

    /// <summary>
    /// Stelt het pad in en geeft de index aan vanaf waar bewogen moet worden.
    /// </summary>
    /// <param name="path">Lijst van punten waarlangs bewogen wordt.</param>
    /// <param name="startIndex">Index in het pad waar begonnen wordt.</param>
    public void SetPathWithProgress(List<Vector2> path, int startIndex)
    {
        _path = path;
        _currentPathIndex = startIndex;
    }

    /// <summary>
    /// Haalt het pad dat de vijand volgt op.
    /// </summary>
    /// <returns>Lijst van Vector2-punten van het pad.</returns>
    public List<Vector2> GetPath()
    {
        return _path;
    }

    /// <summary>
    /// Haalt de huidige index in het pad op.
    /// </summary>
    /// <returns>Huidige padindex.</returns>
    public int GetPathIndex()
    {
        return _currentPathIndex;
    }

    /// <summary>
    /// Haalt de transform van de vijand op.
    /// </summary>
    /// <returns>Transform component.</returns>
    public Transform GetTransform()
    {
        return transform;
    }
}
