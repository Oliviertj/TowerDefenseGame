using UnityEngine;
using System.Collections.Generic;

public class EnemyBase : MonoBehaviour, ITargetable
{
    [Header("Stats")]
    [SerializeField] protected float moveSpeed = 1f;

    [Header("Visual")]
    [SerializeField] protected Color enemyColor = Color.white;

    [Header("Path following")]
    [SerializeField] private List<Vector2> path = new List<Vector2>();
    private int currentPathIndex = 0;

    protected SpriteRenderer spriteRenderer;
    protected IHealth health;

    public bool IsAlive => health != null && health.Current > 0f;

    protected virtual void Start()
    {
        health = GetComponent<IHealth>();
        if (health == null)
        {
            Debug.LogWarning($"{gameObject.name} heeft geen IHealth component!");
        }

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

        // Eventueel, stel je het pad hier in, anders stel het in via de editor of een andere manier
        if (path.Count == 0)
        {
            Debug.LogWarning("Path is not set for this enemy.");
        }
    }

    protected virtual void Update()
    {
        if (path.Count > 0)
        {
            MoveAlongPath();
        }
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

    public void SetPath(List<Vector2> newPath)
    {
        path = newPath;
    }

    protected virtual void MoveAlongPath()
    {
        // Als er een pad is, beweeg naar het volgende punt
        if (currentPathIndex < path.Count)
        {
            // Beweeg naar het huidige padpunt
            Vector2 targetPosition = path[currentPathIndex];
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Als de vijand het doelpunt bereikt heeft, ga dan naar het volgende punt
            if ((Vector2)transform.position == targetPosition)
            {
                currentPathIndex++;
            }
        }
        else
        {
            // Als de vijand het einde van het pad heeft bereikt, kan je hier code toevoegen om te zeggen dat de vijand het pad heeft verlaten.
            // Bijvoorbeeld: de vijand is bij het doel (zoals het verdedigen van de basis).
            Debug.Log("Enemy reached the end of the path.");
            Die();
        }
    }

    public virtual void TakeDamage(float amount)
    {
        health?.TakeDamage(amount);
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
