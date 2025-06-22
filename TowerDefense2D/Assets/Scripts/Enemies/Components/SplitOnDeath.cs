using UnityEngine;

public class SplitOnDeath : MonoBehaviour
{
    [SerializeField] private GameObject _miniPrefab;
    [SerializeField] private int _amount = 2;
    [SerializeField] private float _spread = 0.3f;

    private EnemyBase _enemy;

    private void Start()
    {
        _enemy = GetComponent<EnemyBase>();
        if (_enemy != null && _enemy.Health is BasicHealth basicHealth)
        {
            basicHealth.OnDeath += Split;
        }
    }

    private void Split()
    {
        if (_miniPrefab == null || _enemy == null)
            return;

        for (int i = 0; i < _amount; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-_spread, _spread), Random.Range(-_spread, _spread), 0f);
            GameObject clone = Instantiate(_miniPrefab, transform.position + offset, Quaternion.identity);

            if (clone.TryGetComponent(out EnemyBase miniEnemy))
            {
                var path = _enemy.GetPath();
                var index = _enemy.GetPathIndex();

                miniEnemy.SetPathWithProgress(path, index);

            }
        }
    }
}
