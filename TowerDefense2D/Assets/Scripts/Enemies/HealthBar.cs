using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private IHealth _target;
    private Vector3 _offset = new Vector3(0, 0.6f, 0);
    private Transform _transform;

    void Awake()
    {
        _transform = transform;
    }

    public void Initialize(IHealth target)
    {
        _target = target;
    }

    void Update()
    {
        if (_target == null) return;

        _transform.position = _target.GetTransform().position + _offset;

        float percent = Mathf.Clamp01(_target.Current / _target.Max);
        _transform.localScale = new Vector3(percent, 0.2f, 1f); // schaal direct de balk zelf
    }
}
