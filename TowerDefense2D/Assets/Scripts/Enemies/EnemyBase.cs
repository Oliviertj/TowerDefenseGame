using UnityEngine;

public class EnemyBase : MonoBehaviour, ITargetable
{
    [Header("Stats")]
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float maxHealth = 5f;

    [Header("Visual")]
    [SerializeField] protected Color enemyColor = Color.white;

    protected float currentHealth;
    protected SpriteRenderer spriteRenderer;

    public bool IsAlive => currentHealth > 0f;

    protected virtual void Start()
    {
        currentHealth = maxHealth;

        // Haal de SpriteRenderer op
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = enemyColor;
        }
        else
        {
            Debug.LogWarning($"Enemy '{gameObject.name}' mist een SpriteRenderer component.");
        }
    }

    protected virtual void Update()
    {
        Move();
    }

    protected void SetColor(Color color)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
        else
        {
            Debug.LogWarning($"Enemy '{gameObject.name}' mist een SpriteRenderer component.");
        }
    }

    protected virtual void Move()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }

    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
