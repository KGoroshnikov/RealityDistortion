
using _Project.Scripts.Saves;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveManager))]
public class SaveManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var saveManager = (SaveManager)target;
        
        EditorGUILayout.Space();
        if(GUILayout.Button("Save")) saveManager.Commit();
        if(GUILayout.Button("Restart")) saveManager.Revert();
    }
}