using UnityEngine;

public class BasicHealth : MonoBehaviour, IHealth
{
    private float _maxHealth = 1f;

    private float _currentHealth;
    public float Current => _currentHealth;
    public float Max => _maxHealth;

    public System.Action OnDeath;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void SetMaxHealth(float value)
    {
        _maxHealth = value;
        _currentHealth = value;
    }

    public void TakeDamage(float amount)
    {
        _currentHealth -= amount;
        if (_currentHealth <= 0f)
        {
            OnDeath?.Invoke();
            Destroy(gameObject); // of: gameObject.SetActive(false);
        }
    }

    public void Heal(float amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
    }

}
