using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class FadingRay : MonoBehaviour
{
    [Tooltip("How long to let the ray live at full alpha.")]
    public float Lifetime = 0.75f;

    [Tooltip("How long to fade the ray out.")]
    public float FadeTime = 0.25f;

    private LineRenderer _lineRenderer;
    private float _fadeSpeed;
    private float _alpha;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
        _alpha = 1.0f;
        _fadeSpeed = 1.0f / FadeTime;
    }

    void Update()
    {
        if (Lifetime > 0)
        {
            // decrease lifetime while still alive
            Lifetime -= Time.deltaTime;
        }
        else
        {
            // decrease fade time
            FadeTime -= Time.deltaTime;
            if (FadeTime <= 0)
            {
                // fade time expired, destroy
                Destroy(gameObject);
            }
            else
            {
                // decrease alpha blend value
                _alpha -= _fadeSpeed * Time.deltaTime;

                // apply alpha blending to line render color
                var color = _lineRenderer.startColor;
                color.a = _alpha;
                _lineRenderer.startColor = color;
                _lineRenderer.endColor = color;
                // EXCERCISE: why can't we just set _lineRenderer.startColor.a = _alpha (and the same for endColor)?
            }
        }
    }
}
