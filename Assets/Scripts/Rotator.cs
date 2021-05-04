using UnityEngine;

/// <summary>
/// Rotating around.
/// </summary>
public class Rotator : MonoBehaviour
{
    [Tooltip("The rotation speed in degrees per second, per axis.")]
    public Vector3 Speed = new Vector3(0, 90, 0);

    void Update()
    {
        transform.Rotate(Speed * Time.deltaTime, Space.World);
    }
}
