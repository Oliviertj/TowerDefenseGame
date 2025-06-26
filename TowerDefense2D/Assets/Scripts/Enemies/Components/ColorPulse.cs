using UnityEngine;
using TMPro;

public class ColorPulse : MonoBehaviour
{
    [Header("Kleuren")]
    [SerializeField] private Color _colorA = Color.white;
    [SerializeField] private Color _colorB = Color.gray;

    [Header("Overgangstijd in seconden")]
    [SerializeField] private float _duration = 1.5f;

    private SpriteRenderer _spriteRenderer;
    private TextMeshProUGUI _tmpUI;
    private TextMeshPro _tmpWorld;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _tmpUI = GetComponent<TextMeshProUGUI>();
        _tmpWorld = GetComponent<TextMeshPro>();

        if (_spriteRenderer == null && _tmpUI == null && _tmpWorld == null)
        {
            Debug.LogWarning($"ColorPulse op '{gameObject.name}': geen SpriteRenderer of TextMeshPro component gevonden!");
            enabled = false;
        }
    }

    /// <summary>
    /// Wisselt de kleur van verschillende componenten continu heen en weer tussen _colorA & _colorB
    /// </summary>
    void Update()
    {
        float t = Mathf.PingPong(Time.time / _duration, 1f);
        Color lerped = Color.Lerp(_colorA, _colorB, t);

        if (_spriteRenderer != null)
            _spriteRenderer.color = lerped;

        if (_tmpUI != null)
            _tmpUI.color = lerped;

        if (_tmpWorld != null)
            _tmpWorld.color = lerped;
    }

}
