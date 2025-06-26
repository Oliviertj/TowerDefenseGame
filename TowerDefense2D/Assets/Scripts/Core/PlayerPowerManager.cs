using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerPowerManager : MonoBehaviour
{
    [Header("Overdrive Settings")]
    [SerializeField] private float _overdriveMultiplier = 2f;
    [SerializeField] private float _duration = 5f;
    [SerializeField] private float _cooldown = 10f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _hintText;

    private bool _isOnCooldown = false;

    void Start()
    {
        UpdateHint();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !_isOnCooldown)
        {
            StartCoroutine(ActivateOverdrive());
        }
    }

    private IEnumerator ActivateOverdrive()
    {
        _isOnCooldown = true;
        UpdateHint();

        // Verhoog fire rate van alle torens
        foreach (var tower in FindObjectsOfType<TowerBase>())
            tower.ApplyFireRateMultiplier(_overdriveMultiplier);

        yield return new WaitForSeconds(_duration);

        // Reset fire rate
        foreach (var tower in FindObjectsOfType<TowerBase>())
            tower.ApplyFireRateMultiplier(1f);

        UpdateHint();

        yield return new WaitForSeconds(_cooldown);

        _isOnCooldown = false;
        UpdateHint();
    }

    private void UpdateHint()
    {
        if (_hintText == null) return;

        if (!_isOnCooldown)
            _hintText.text = "Overdrive ready! (Q)";
        else if (_isOnCooldown && _hintText.text != "Overdrive active")
            _hintText.text = "Overdrive cooldown...";
    }
}
