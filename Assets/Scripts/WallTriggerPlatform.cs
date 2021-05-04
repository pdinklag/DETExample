using UnityEngine;

/// <summary>
/// A platform that triggers a wall when a player steps on it.
/// </summary>
public class WallTriggerPlatform : MonoBehaviour
{
    [Tooltip("The wall to be triggered.")]
    public MovableWall Wall;
}
