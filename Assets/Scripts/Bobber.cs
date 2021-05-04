using UnityEngine;

/// <summary>
/// Bobbing about.
/// </summary>
public class Bobber : MonoBehaviour
{
    [Tooltip("The bobbing amplitude.")]
    public float Amplitude = 0.25f;

    [Tooltip("The bobbing frequency in radians per second.")]
    public float Frequency = 1.0f;

    private Vector3 _home;

    void Start()
    {
        _home = transform.position;
    }

    void Update()
    {
        var bob = Amplitude * Mathf.Sin(Frequency * Time.time);
        transform.position = _home + bob * Vector3.up;
    }
}
