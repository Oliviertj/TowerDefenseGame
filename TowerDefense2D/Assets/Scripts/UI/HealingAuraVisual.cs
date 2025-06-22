using UnityEngine;

public class HealingAuraVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _auraRenderer;
    [SerializeField] private float _showDuration = 0.5f;

    private float _timer;

    void Awake()
    {
        if (_auraRenderer == null)
            _auraRenderer = GetComponent<SpriteRenderer>();

        _auraRenderer.enabled = false;
    }

    public void SetRadius(float radius)
    {
        transform.localScale = Vector3.one * radius * 2f; // omdat de circle unit 1 is
    }

    public void Show()
    {
        if (_auraRenderer == null) return;

        _auraRenderer.enabled = true;
        _timer = _showDuration;
    }

    void Update()
    {
        if (_auraRenderer.enabled)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
                _auraRenderer.enabled = false;
        }
    }
}
