using UnityEngine;

public class BarrelRotator : MonoBehaviour
{
    [SerializeField] private Transform _barrel;
    [SerializeField] private float _rotationSpeed = 5f;

    private Transform _target;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    void Update()
    {
        if (_target == null || _barrel == null) return;

        Vector3 direction = _target.position - _barrel.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle - 90f);

        _barrel.rotation = Quaternion.Lerp(_barrel.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
    }
}
