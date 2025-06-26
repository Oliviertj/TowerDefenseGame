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

    /// <summary>
    /// Voert een stun-effect uit op alle torens binnen een bepaalde straal.
    /// 
    /// - Instantiate een effect op de positie van dit object.
    /// - Detecteer alle torens binnen de stun-radius met behulp van Physics2D.OverlapCircleAll.
    /// - Voor elke toren die het IStunnable heeft, wordt de Stun methode aangeroepen met de opgegeven duur.
    /// </summary>
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

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IStunnable stunnable))
            {
                stunnable.Stun(_stunDuration);
                Debug.Log($"{gameObject.name} stunned {hit.name} for {_stunDuration}s");

            }
            else
            {
                Debug.LogWarning($"{hit.name} heeft GEEN IStunnable component.");
            }

        }
    }
}
