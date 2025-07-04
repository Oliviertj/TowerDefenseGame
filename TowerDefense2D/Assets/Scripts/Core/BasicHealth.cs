using UnityEngine;

public class BasicHealth : MonoBehaviour, IHealth
{
    private float _maxHealth;

    private float _currentHealth;
    public float Current => _currentHealth;
    public float Max => _maxHealth;

    public System.Action OnDeath;

    public Transform GetTransform()
    {
        return transform;
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
            OnDeath?.Invoke(); // Alleen triggeren, geen destroy
        }
    }


    public void Heal(float amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
    }

}
