using UnityEngine;

/// <summary>
/// Provides extension methods for the LineRenderer class.
/// </summary>
public static class LineRendererExtensions
{
    /// <summary>
    /// Set the first two points of a line renderer in world space.
    /// </summary>
    /// <param name="lineRenderer">the line renderer</param>
    /// <param name="start">the start point</param>
    /// <param name="end">the end point</param>
    public static void SetWorldSpacePoints(this LineRenderer lineRenderer, Vector3 start, Vector3 end)
    {
        lineRenderer.useWorldSpace = true;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
