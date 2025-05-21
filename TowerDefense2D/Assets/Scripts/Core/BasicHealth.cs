using UnityEngine;

public class BasicHealth : MonoBehaviour, IHealth
{
    [SerializeField] private float maxHealth = 5f;
    private float currentHealth;

    public float Current => currentHealth;
    public float Max => maxHealth;

    public System.Action OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void SetMaxHealth(float value)
    {
        maxHealth = value;
        currentHealth = value;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            OnDeath?.Invoke();
            Destroy(gameObject); // of: gameObject.SetActive(false);
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

}
