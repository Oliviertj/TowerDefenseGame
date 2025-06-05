using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class EnemyBase : MonoBehaviour, ITargetable
{
    [Header("Stats")]
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float livesLost = 1f;
    [SerializeField] protected float enemyWeight = 1f;

    [Header("Visual")]
    [SerializeField] protected Color enemyColor = Color.white;

    [Header("Path following")]
    [SerializeField] private List<Vector2> _path = new List<Vector2>();
    private int _currentPathIndex = 0;

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

        if (_path.Count == 0)
        {
            Debug.LogWarning("Path is not set for this enemy.");
        }
    }

    protected virtual void Update()
    {
        if (_path.Count > 0)
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
        _path = newPath;
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
            {
                _currentPathIndex++;
            }
        }
        else
        {
            Debug.Log("Enemy reached the end of the path.");
            Die(reachedGoal: true);
        }



    }

    public virtual void TakeDamage(float amount)
    {
        health.TakeDamage(amount);
    }



    protected virtual void Die(bool reachedGoal = false)
    {
        if (reachedGoal)
        {
            LifeManager.Instance?.LoseLives(livesLost);
        }

        Destroy(gameObject);
    }


    public Transform GetTransform()
    {
        return transform;
    }
}
