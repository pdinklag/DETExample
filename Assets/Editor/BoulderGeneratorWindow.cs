using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder;

public class BoulderGeneratorWindow : EditorWindow
{
    [MenuItem("Tools/Boulder Generator")]
    private static void Open()
    {
        var window = GetWindow<BoulderGeneratorWindow>();
        window.titleContent = new GUIContent("Boulder Generator");
    }

    private Transform _parent;
    private float _radius = 1.0f;
    private int _subdivisions = 1;
    private Material _material;
    private float _randomness = 0.1f;
    private Vector3 _scale = Vector3.one;

    private void GenerateBoulder()
    {
        // generate a base sphere
        var shape = ShapeGenerator.GenerateIcosahedron(PivotLocation.Center, _radius, _subdivisions);

        // move around vertices randomly
        var numSharedVertices = shape.sharedVertices.Count;
        for (var i = 0; i < numSharedVertices; i++)
        {
            var sharedVertex = shape.sharedVertices[i];

            // generate random radius deviation
            var r = Random.Range(1.0f - _randomness, 1.0f + _randomness);

            // scale original position and randomize
            var pos = r * Vector3.Scale(shape.positions[sharedVertex[0]], _scale);

            // set position
            shape.SetSharedVertexPosition(i, r * pos);
        }

        // smooth shading
        Smoothing.ApplySmoothingGroups(shape, shape.faces, 22.5f);
        shape.Refresh();

        // generate mesh renderer
        shape.ToMesh();

        // parent
        var boulder = shape.gameObject;
        boulder.transform.SetParent(_parent, false);

        // register undo for editor
        Undo.RegisterCreatedObjectUndo(boulder, "Generated Boulder");

        // set material
        boulder.GetComponent<MeshRenderer>().sharedMaterial = _material;

        // create collider
        var collider = boulder.AddComponent<MeshCollider>();
        collider.sharedMesh = shape.GetComponent<MeshFilter>().sharedMesh;
        collider.convex = true;
    }

    private void OnGUI()
    {
        EditorGUILayout.Separator();
        GUILayout.Label("Settings", EditorStyles.boldLabel);
        _parent = EditorGUILayout.ObjectField("Parent", _parent, typeof(Transform), true) as Transform;
        _radius = EditorGUILayout.FloatField("Base Radius", _radius);
        _subdivisions = EditorGUILayout.IntSlider("Subdivisions", _subdivisions, 1, 4);
        _randomness = EditorGUILayout.FloatField("Randomness", _randomness);
        _scale = EditorGUILayout.Vector3Field("Scale", _scale);
        _material = EditorGUILayout.ObjectField("Material", _material, typeof(Material), false) as Material;

        if (GUILayout.Button("Generate"))
        {
            GenerateBoulder();
        }
    }
}
