using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    [SerializeField] private LayerMask _pathLayerMask;
    [SerializeField] private LayerMask _TowerlayerMask;
    [SerializeField] private Color _previewColor = new Color(1f, 1f, 1f, 0.5f); // alpha 0.5 to make turret greyish

    private GameObject _turretToBuild;
    private GameObject _previewInstance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetTurretToBuild(GameObject turretPrefab)
    {
        _turretToBuild = turretPrefab;

        if (_previewInstance != null)
            Destroy(_previewInstance);

        _previewInstance = Instantiate(_turretToBuild);
        SetLayerRecursive(_previewInstance, LayerMask.NameToLayer("Ignore Raycast"));
        SetColorRecursive(_previewInstance, _previewColor);

        // Alle componenten die updaten uitschakelen zodat hij niet kan schieten tijdens het plaatsen
        SetActiveComponents(_previewInstance, false);
    }
    private void SetActiveComponents(GameObject obj, bool enabled)
    {
        foreach (var mono in obj.GetComponentsInChildren<MonoBehaviour>())
        {
            // Schakel alles uit behalve Transform en SpriteRenderer
            // Spriterenderer is niet direct geerft van MonoBehaviour daarom een warning
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
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _previewInstance.transform.position = new Vector3(mouseWorldPos.x, mouseWorldPos.y, 0f);

            if (Input.GetMouseButtonDown(0))
            {
                // Check path blocking
                Collider2D pathHit = Physics2D.OverlapPoint(mouseWorldPos, _pathLayerMask);
                Collider2D towerHit = Physics2D.OverlapPoint(mouseWorldPos, _TowerlayerMask);

                if (pathHit != null || towerHit != null)
                {
                    Debug.Log("Je kunt geen toren op het pad bouwen of op een andere toren!");
                    return;
                }

                // Build turret
                GameObject placedTurret = Instantiate(_turretToBuild, mouseWorldPos, Quaternion.identity);
                SetActiveComponents(placedTurret, true); // zet scripts nu weer aan
                CancelBuild(); // annuleer direct na plaatsen

            }

            if (Input.GetMouseButtonDown(1)) // rechter muisklik annuleert als je niet meer wilt plaatsen
            {
                CancelBuild();
            }
        }
    }

    private void CancelBuild()
    {
        Destroy(_previewInstance);
        _turretToBuild = null;
    }

    /// <summary>
    /// Zet het object op een grijzige kleur om aan te tonen dat de turret nog in preview mode is.
    /// </summary>
    /// <param name="obj">De turret die geplaatst wordt</param>
    /// <param name="color">De grijzige kleur</param>
    private void SetColorRecursive(GameObject obj, Color color)
    {
        foreach (SpriteRenderer renderer in obj.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.color = color;
        }
    }

    private void SetLayerRecursive(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursive(child.gameObject, layer);
        }
    }
}
