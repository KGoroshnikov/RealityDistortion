
using _Project.Scripts.Saves;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SaveManager))]
public class SaveManagerEditor : Editor
{
    private bool StateChangesFoldout;
    private bool SavedStateFoldout;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var saveManager = (SaveManager)target;
        
        EditorGUILayout.Space();
        if(GUILayout.Button("Save")) saveManager.Commit();
        if(GUILayout.Button("Restart")) saveManager.Revert();
        EditorGUILayout.Space();
        
        StateChangesFoldout = EditorGUILayout.Foldout(StateChangesFoldout, "State Changes");
        if (StateChangesFoldout)
            foreach (var key in saveManager.StateChanges.Keys)
                EditorGUILayout.LabelField(key);
        
        SavedStateFoldout = EditorGUILayout.Foldout(SavedStateFoldout, "Saved State");
        if (SavedStateFoldout)
            foreach (var key in saveManager.SavedState.Keys)
                EditorGUILayout.LabelField(key);
    }
}