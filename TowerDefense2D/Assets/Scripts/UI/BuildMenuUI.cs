using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuUI : MonoBehaviour
{
    [Header("Turrets")]
    [SerializeField] private List<GameObject> turretPrefabs;

    [Header("UI")]
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private Button turretButtonPrefab;

    private BuildManager _buildManager;

    void Start()
    {
        _buildManager = FindObjectOfType<BuildManager>();

        foreach (GameObject turret in turretPrefabs)
        {
            Button btn = Instantiate(turretButtonPrefab, buttonContainer);
            TextMeshProUGUI label = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (label != null)
                label.text = turret.name;

            btn.onClick.AddListener(() =>
            {
                _buildManager.SetTurretToBuild(turret);
            });
        }
    }
}
