using UnityEngine;
using System.Collections.Generic;
using TMPro;

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
        if (currentPathIndex < path.Count)
        {
            Vector3 currentPos = transform.position;
            Vector2 target2D = path[currentPathIndex];
            Vector3 targetPos = new Vector3(target2D.x, target2D.y, currentPos.z);

            transform.position = Vector3.MoveTowards(currentPos, targetPos, moveSpeed * Time.deltaTime);

            if ((Vector2)transform.position == target2D)
            {
                currentPathIndex++;
            }
        }
        else
        {
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
