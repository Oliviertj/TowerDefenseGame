public interface IHealth
{
    float Current { get; }
    float Max { get; }

    void TakeDamage(float amount);
    void Heal(float amount);
}
