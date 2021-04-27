using UnityEngine;

/// <summary>
/// A weapon projectile.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [Tooltip("The number of seconds after which this projectile will automatically destroy itself.")]
    public float Lifetime = 3.0f;

    private void Update()
    {
        // reduce lifetime by the time (in seconds) that has passed since the last Update
        Lifetime -= Time.deltaTime;

        if(Lifetime <= 0)
        {
            // the lifetime has expired - destroy the projectile!
            Destroy(gameObject);
        }
    }
}
