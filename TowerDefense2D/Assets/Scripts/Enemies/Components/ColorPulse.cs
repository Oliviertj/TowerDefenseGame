using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ColorPulse : MonoBehaviour
{
    [Tooltip("Eerste kleur in de overgang (bijv. groen)")]
    [SerializeField] private Color _colorA = Color.white;

    [Tooltip("Tweede kleur in de overgang (bijv. blauw)")]
    [SerializeField] private Color _colorB = Color.white;

    [Tooltip("Tijd (seconden) om tussen de kleuren te faden")]
    [SerializeField] private float _duration = 1.5f;

    private SpriteRenderer _renderer;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();

        if (_renderer == null)
        {
            Debug.LogWarning("ColorPulse: SpriteRenderer niet gevonden!");
            enabled = false;
        }
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time / _duration, 1f);
        _renderer.color = Color.Lerp(_colorA, _colorB, t);
    }
}
