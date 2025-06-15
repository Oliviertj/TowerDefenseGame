using UnityEngine;

public class BarrelRotator : MonoBehaviour
{
    [SerializeField] private Transform _barrelPivot;
    [SerializeField] private float _rotationSpeed = 5f;

    private Transform _target;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    void Update()
    {
        if (_target == null || _barrelPivot == null) return;

        Vector3 direction = _target.position - _barrelPivot.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        angle -= 90f;

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        _barrelPivot.rotation = Quaternion.Lerp(_barrelPivot.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
    }
}
