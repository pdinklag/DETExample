using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerCharacter))]
public class PlayerCharacterEditor : Editor
{
    private bool _cheatsFoldout;
    private PlayerCharacter _playerCharacter;
    private Weapon _weaponToGive;

    private void OnEnable()
    {
        _playerCharacter = serializedObject.targetObject as PlayerCharacter;
    }

    public override void OnInspectorGUI()
    {
        // default inspector
        DrawDefaultInspector();

        // only show in play mode
        if (EditorApplication.isPlaying)
        {
            // leave some space
            EditorGUILayout.Separator();

            _cheatsFoldout = EditorGUILayout.BeginFoldoutHeaderGroup(_cheatsFoldout, "Cheats");
            if (_cheatsFoldout)
            {
                EditorGUI.BeginChangeCheck();

                // allow choosing a non-scene object of type Weapon
                _weaponToGive = EditorGUILayout.ObjectField("Give Weapon", _weaponToGive, typeof(Weapon), false) as Weapon;

                if (EditorGUI.EndChangeCheck())
                {
                    if(_weaponToGive != null)
                    {
                        _playerCharacter.GiveWeapon(_weaponToGive);
                        Debug.Log("CHEAT: Gave " + _weaponToGive + " to " + _playerCharacter, _playerCharacter);
                    }
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }

    private void OnDisable()
    {
        // nothing to do here
    }
}
