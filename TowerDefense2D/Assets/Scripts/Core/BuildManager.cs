using UnityEngine;

/// <summary>
/// Beheert het bouwen van torens op het speelveld.
/// Zorgt voor plaatsing, preview en validatie.
/// </summary>
public class BuildManager : MonoBehaviour
{

    [SerializeField] private LayerMask _pathLayerMask;
    [SerializeField] private LayerMask _TowerlayerMask;
    [SerializeField] private Color _previewColor = new Color(1f, 1f, 1f, 0.5f); // Grijsachtige kleur voor preview

    private GameObject _turretToBuild;
    private GameObject _previewInstance;

    /// <summary>
    /// Stelt het torenprefab in dat gebouwd moet worden. Toont direct een preview.
    /// </summary>
    /// <param name="turretPrefab">De prefab van de toren die geplaatst moet worden</param>
    public void SetTurretToBuild(GameObject turretPrefab)
    {
        _turretToBuild = turretPrefab;

        if (_previewInstance != null)
            Destroy(_previewInstance);

        _previewInstance = Instantiate(_turretToBuild);
        SetLayerRecursive(_previewInstance, LayerMask.NameToLayer("Ignore Raycast"));
        SetColorRecursive(_previewInstance, _previewColor);

        // Scripts uitschakelen zodat de preview niet actief schiet
        SetActiveComponents(_previewInstance, false);
    }

    /// <summary>
    /// Zet alle scripts aan of uit op het object behalve de renderer.
    /// </summary>
    /// <param name="obj">Het object waarop de scripts gewijzigd worden</param>
    /// <param name="enabled">True = activeren, False = deactiveren</param>
    private void SetActiveComponents(GameObject obj, bool enabled)
    {
        foreach (var mono in obj.GetComponentsInChildren<MonoBehaviour>())
        {
            // SpriteRenderers zijn geen MonoBehaviour, dus die slaan we over
            if (!(mono is SpriteRenderer))
                mono.enabled = enabled;
        }
    }

    public GameObject GetTurretToBuild()
    {
        return _turretToBuild;
    }

    void Update()
    {
        if (_turretToBuild != null && _previewInstance != null)
        {
            // Volg muispositie met de preview
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _previewInstance.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0f);

            // Linkermuisklik om te plaatsen
            if (Input.GetMouseButtonDown(0))
            {
                // Check of je op pad of bestaande toren klikt
                Collider2D pathHit = Physics2D.OverlapPoint(mouseWorldPos, _pathLayerMask);
                Collider2D towerHit = Physics2D.OverlapPoint(mouseWorldPos, _TowerlayerMask);

                if (pathHit != null || towerHit != null)
                {
                    Debug.Log("Je kunt geen toren op het pad bouwen of op een andere toren!");
                    return;
                }

                // Plaats toren en activeer scripts
                GameObject placedTurret = Instantiate(_turretToBuild, mouseWorldPos, Quaternion.identity);
                SetActiveComponents(placedTurret, true);

                CancelBuild(); // Stop na één plaatsing
            }

            // Rechtermuisklik om annuleren
            if (Input.GetMouseButtonDown(1))
            {
                CancelBuild();
            }
        }
    }

    /// <summary>
    /// Annuleert het bouwproces en verwijdert de preview.
    /// </summary>
    private void CancelBuild()
    {
        Destroy(_previewInstance);
        _turretToBuild = null;
    }

    /// <summary>
    /// Zet de kleur van alle SpriteRenderers in het object op een gewenste previewkleur.
    /// </summary>
    /// <param name="obj">Het object waarvan de kleur aangepast moet worden</param>
    /// <param name="color">De kleur die moet worden toegepast</param>
    private void SetColorRecursive(GameObject obj, Color color)
    {
        foreach (SpriteRenderer renderer in obj.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.color = color;
        }
    }

    /// <summary>
    /// Zet de layer van het object en al zijn kinderen.
    /// </summary>
    /// <param name="obj">Het object dat van layer verandert</param>
    /// <param name="layer">De layer die ingesteld moet worden</param>
    private void SetLayerRecursive(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursive(child.gameObject, layer);
        }
    }
}
