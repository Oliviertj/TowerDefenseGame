using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuUI : MonoBehaviour
{
    [Header("Turrets")]
    [SerializeField] private List<GameObject> _turretPrefabs;

    [Header("UI")]
    [SerializeField] private Transform _buttonContainer;
    [SerializeField] private Button _turretButtonPrefab;

    private BuildManager _buildManager;

    void Start()
    {
        _buildManager = FindObjectOfType<BuildManager>();

        foreach (GameObject turret in _turretPrefabs)
        {
            Button btn = Instantiate(_turretButtonPrefab, _buttonContainer);
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
