using UnityEngine;

public class AreaHealer : MonoBehaviour
{
    [Header("Healer Instellingen")]
    [SerializeField] private float _healRadius = 5f;
    [SerializeField] private float _healAmount = 2f;
    [SerializeField] private float _interval = 2f;

    [Header("Aura Visual Prefab")]
    [SerializeField] private GameObject _healingAuraPrefab;

    private HealingAuraVisual _healingAuraInstance;
    private float _timer;

    /// <summary>
    /// Initialisatie van de healer.
    /// Zet de timer op het interval en maakt de healing aura visual aan, 
    /// waarbij de straal van de aura wordt ingesteld op de heal radius.
    /// </summary>
    void Start()
    {
        _timer = _interval;

        if (_healingAuraPrefab != null)
        {
            GameObject auraGO = Instantiate(_healingAuraPrefab, transform.position, Quaternion.identity, transform);
            _healingAuraInstance = auraGO.GetComponent<HealingAuraVisual>();

            if (_healingAuraInstance != null)
            {
                _healingAuraInstance.SetRadius(_healRadius);
            }
        }
    }

    /// <summary>
    /// Update wordt elke frame aangeroepen.
    /// Houdt bij wanneer de healer moet genezen aan de hand van de interval timer.
    /// </summary>
    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            HealNearbyEnemies();
            _timer = _interval;
        }
    }

    /// <summary>
    /// Geneest alle vijanden binnen de heal radius met de opgegeven heal amount.
    /// Verhoogt ook indien nodig de max health van de vijand.
    /// Toont de healing aura visual wanneer er genezen is.
    /// </summary>
    void HealNearbyEnemies()
    {
        bool didHeal = false;

        EnemyBase[] allEnemies = GameObject.FindObjectsOfType<EnemyBase>();
        foreach (var enemy in allEnemies)
        {
            if (enemy == null || enemy == GetComponent<EnemyBase>()) continue;

            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= _healRadius)
            {
                IHealth health = enemy.GetComponent<IHealth>();
                if (health != null && health is BasicHealth basicHealth)
                {
                    // Verhoog max health als huidige gezondheid al maximaal is
                    if (basicHealth.Current >= basicHealth.Max)
                        basicHealth.SetMaxHealth(basicHealth.Max + _healAmount);

                    basicHealth.Heal(_healAmount);
                    didHeal = true;
                }
            }
        }

        // Toon de healing aura alleen als er daadwerkelijk genezen is
        if (didHeal && _healingAuraInstance != null)
        {
            _healingAuraInstance.Show();
        }
    }

}
