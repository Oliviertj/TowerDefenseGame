using UnityEngine;
using System.Collections;

public class StunOnDeath : MonoBehaviour
{
    [Header("Stun Instellingen")]
    [SerializeField] private float _stunRadius = 2f;
    [SerializeField] private float _stunDuration = 3f;
    [SerializeField] private LayerMask _towerLayer;
    [SerializeField] private GameObject _stunVisualPrefab;

    private IHealth _healthComp;

    private void Start()
    {
        Debug.Log("StunOnDeath gestart op " + gameObject.name);

        _healthComp = GetComponent<IHealth>();
        if (_healthComp is BasicHealth basicHealth)
        {
            basicHealth.OnDeath += OnStun;
        }
        else
        {
            Debug.LogWarning("Geen BasicHealth gevonden op " + gameObject.name);
        }
    }


    private void OnStun()
    {
        Debug.Log("OnStun CALLED!");
        if (_stunVisualPrefab != null)
        {
            GameObject visual = Instantiate(_stunVisualPrefab, transform.position, Quaternion.identity);

            // Set juiste radius voor het tonen
            HealingAuraVisual aura = visual.GetComponent<HealingAuraVisual>();
            if (aura != null)
            {
                aura.SetRadius(_stunRadius);
                aura.Show();
            }

        }


        // Zoek torens binnen radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _stunRadius, _towerLayer);
        Debug.Log($"Found {hits.Length} colliders in stun radius.");

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IStunnable stunnable))
            {
                Debug.Log($"Stunning {hit.name} for {_stunDuration} seconds!");
                stunnable.Stun(_stunDuration);
                Debug.Log($"{gameObject.name} stunned {hit.name} for {_stunDuration}s");
                Debug.DrawLine(transform.position, hit.transform.position, Color.red, 1.5f);

            }
            else
            {
                Debug.LogWarning($"{hit.name} heeft GEEN IStunnable component.");
            }

        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _stunRadius);
    }
#endif
}
