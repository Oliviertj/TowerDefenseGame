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
                // _healingAuraInstance.SetColor(new Color(0f, 1f, 0.5f, 0.3f)); // optioneel
            }
        }
    }

    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            HealNearbyEnemies();
            _timer = _interval;
        }
    }

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
                    if (basicHealth.Current >= basicHealth.Max)
                        basicHealth.SetMaxHealth(basicHealth.Max + _healAmount);

                    basicHealth.Heal(_healAmount);
                    didHeal = true;
                }
            }
        }

        // Toon visual alleen als er gehealed is
        if (didHeal && _healingAuraInstance != null)
        {
            _healingAuraInstance.Show();
        }
    }

}
