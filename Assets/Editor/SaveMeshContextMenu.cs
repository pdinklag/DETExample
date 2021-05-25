using UnityEditor;

public static class SaveMeshContextMenu
{
    [MenuItem("CONTEXT/Mesh/Create Mesh Asset...")]
    public static void SaveMesh(MenuCommand command)
    {
        // get the edited object
        var obj = command.context;

        // show a save dialog
        var path = EditorUtility.SaveFilePanelInProject("Create Asset", obj.name, "asset", "Save Mesh");

        // if user pressed "Cancel", path will be empty
        if (path.Length > 0)
        {
            // create asset in the given path
            AssetDatabase.CreateAsset(obj, path);

            // select it in the project browser
            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(path);
        }
    }

    [MenuItem("CONTEXT/Mesh/Create Mesh Asset...", true)]
    public static bool ValidateSaveMesh(MenuCommand command)
    {
        return !AssetDatabase.IsMainAsset(command.context);
    }
}
