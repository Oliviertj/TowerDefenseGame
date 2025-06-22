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

    private IEnumerator ApplySlowCoroutine(float amount, float duration)
    {
        moveSpeed = _originalSpeed * (1f - Mathf.Clamp01(amount));

        yield return new WaitForSeconds(duration);

        moveSpeed = _originalSpeed;
        slowRoutine = null;
    }


    public virtual void TakeDamage(float amount)
    {
        health?.TakeDamage(amount);
    }

    public virtual void Die(bool reachedGoal = false)
    {
        Debug.Log($"Enemy {gameObject.name} died. ReachedGoal = {reachedGoal}");
        if (reachedGoal)
            LifeManager.Instance?.LoseLives(livesLost);

        Destroy(gameObject);
    }

    public void SetPath(List<Vector2> newPath)
    {
        _path = newPath;
    }
    public void SetPathWithProgress(List<Vector2> path, int startIndex)
    {
        _path = path;
        _currentPathIndex = startIndex;
    }


    public List<Vector2> GetPath()
    {
        return _path;
    }

    public int GetPathIndex()
    {
        return _currentPathIndex;
    }


    public Transform GetTransform()
    {
        return transform;
    }
}
