using UnityEngine;

public class SplitOnDeath : MonoBehaviour
{
    [SerializeField] private GameObject _miniPrefab;
    [SerializeField] private int _amount = 2;
    [SerializeField] private float _spread = 0.3f;

    private EnemyBase _enemy;

    /// <summary>
    /// Haalt de EnemyBase component op en koppelt de Split methode aan de OnDeath event van BasicHealth.
    /// </summary>
    private void Start()
    {
        _enemy = GetComponent<EnemyBase>();
        if (_enemy != null && _enemy.Health is BasicHealth basicHealth)
        {
            basicHealth.OnDeath += Split;
        }
    }

    /// <summary>
    /// Maakt meerdere mini vijanden aan rondom de positie van deze vijand wanneer deze sterft.
    /// </summary>
    private void Split()
    {
        if (_miniPrefab == null || _enemy == null)
            return;

        for (int i = 0; i < _amount; i++)
        {
            // Bepaal een willekeurige offset voor spreiding van mini vijanden
            Vector3 offset = new Vector3(Random.Range(-_spread, _spread), Random.Range(-_spread, _spread), 0f);

            // Instantieer de mini vijand met offset positie
            GameObject clone = Instantiate(_miniPrefab, transform.position + offset, Quaternion.identity);

            // Als de clone een EnemyBase component heeft, geef het pad en de huidige padindex door om vanuit die plek weer het pad te vervolgen
            if (clone.TryGetComponent(out EnemyBase miniEnemy))
            {
                var path = _enemy.GetPath();
                var index = _enemy.GetPathIndex();

                miniEnemy.SetPathWithProgress(path, index);
            }
        }
    }
}
