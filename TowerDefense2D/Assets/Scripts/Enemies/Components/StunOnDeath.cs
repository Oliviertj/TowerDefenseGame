using UnityEngine;
using System.Collections;

public class StunOnDeath : MonoBehaviour
{
    [Header("Stun Instellingen")]
    [SerializeField] private float stunRadius = 2f;
    [SerializeField] private float stunDuration = 3f;
    [SerializeField] private LayerMask towerLayer;
    [SerializeField] private GameObject stunVisualPrefab;

    private IHealth healthComp;

    private void Start()
    {
        Debug.Log("StunOnDeath gestart op " + gameObject.name);

        healthComp = GetComponent<IHealth>();
        if (healthComp is BasicHealth basicHealth)
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
        // Toon visueel effect, indien toegewezen
        if (stunVisualPrefab != null)
            Instantiate(stunVisualPrefab, transform.position, Quaternion.identity, transform);

        // Zoek torens binnen radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, stunRadius, towerLayer);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out IStunnable stunnable))
            {
                stunnable.Stun(stunDuration);
                Debug.Log($"{gameObject.name} stunned {hit.name} for {stunDuration}s");
                Debug.DrawLine(transform.position, hit.transform.position, Color.red, 1.5f);

            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, stunRadius);
    }
#endif
}
