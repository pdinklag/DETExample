using UnityEngine;

/// <summary>
/// A camera that smoothly follows a player.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Tooltip("A point in the scene for initially determining the offset from the player.")]
    public Transform InitiallyRelativeTo;

    [Tooltip("The time after which the camera should have reached the player when they move.")]
    public float MovementSmoothTime = 1.0f;

    /// <summary>
    /// The player to follow.
    /// </summary>
    public PlayerCharacter Player { get; set; }

    /// <summary>
    /// The position offset from the player.
    /// </summary>
    private Vector3 _offset;

    /// <summary>
    /// The current velocity.
    /// </summary>
    private Vector3 _velocity;

    private void Start()
    {
        _offset = transform.position - InitiallyRelativeTo.position;
    }

    private void Update()
    {
        if (Player)
        {
            var targetPosition = Player.transform.position + _offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, MovementSmoothTime);
        }
    }
}
