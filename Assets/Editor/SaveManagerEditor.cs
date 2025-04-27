using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Saves;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(SaveManager))]
public class SaveManagerEditor : Editor
{
    private bool StateChangesFoldout;
    private bool SavedStateFoldout;
    private Dictionary<string,bool> StateChangesSubFoldouts = new();
    private Dictionary<string,bool> SavedStateSubFoldouts = new();
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var saveManager = (SaveManager)target;
        
        EditorGUILayout.Space();
        if(GUILayout.Button("Save")) saveManager.Commit();
        if(GUILayout.Button("Restart")) saveManager.Revert();
        EditorGUILayout.Space();
        
        StateChangesFoldout = EditorGUILayout.Foldout(StateChangesFoldout, "Changed State");
        if (StateChangesFoldout)
        {
            EditorGUI.indentLevel++;
            foreach (var (key, value) in saveManager.StateChanges)
                DataField(key, value, StateChangesSubFoldouts);
            EditorGUI.indentLevel--;
        }


        SavedStateFoldout = EditorGUILayout.Foldout(SavedStateFoldout, "Saved State");
        if (SavedStateFoldout)
        {
            EditorGUI.indentLevel++;
            foreach (var (key, value) in saveManager.SavedState)
                DataField(key, value, SavedStateSubFoldouts);
            EditorGUI.indentLevel--;
        }
    }

    private void DataField(string label, object value, Dictionary<string,bool> subFoldouts, string path = "")
    {
        var p = $"{path}/{label}";
        switch (value)
        {
            case bool val:
                EditorGUILayout.Toggle(label, val);
                break;
            case int val:
                EditorGUILayout.IntField(label, val);
                break;
            case long val:
                EditorGUILayout.LongField(label, val);
                break;
            case float val:
                EditorGUILayout.FloatField(label, val);
                break;
            case Color val:
                EditorGUILayout.ColorField(label, val);
                break;
            case string val:
                EditorGUILayout.TextField(label, val);
                break;
            case Vector2 val:
                EditorGUILayout.Vector2Field(label, val);
                break;
            case Vector3 val:
                EditorGUILayout.Vector3Field(label, val);
                break;
            case Vector4 val:
                EditorGUILayout.Vector4Field(label, val);
                break;
            case Quaternion val:
                EditorGUILayout.Vector3Field(label, val.eulerAngles);
                break;
            case Rect val:
                EditorGUILayout.RectField(label, val);
                break;
            case Gradient val:
                EditorGUILayout.GradientField(label, val);
                break;
            case Enum val:
                EditorGUILayout.EnumFlagsField(label, val);
                break;
            case Object val:
                EditorGUILayout.ObjectField(label, val, val.GetType(), false);
                break;
            case IEnumerable val:
                subFoldouts[p] = EditorGUILayout.Foldout(subFoldouts.GetValueOrDefault(p), label);
                if (subFoldouts[p])
                {
                    EditorGUI.indentLevel++;
                    var i = 0;
                    foreach (var o in val)
                    {
                        DataField($"{i}", o, subFoldouts, p);
                        i++;
                    }
                    EditorGUI.indentLevel--;
                }
                break;
            default:
                subFoldouts[p] = EditorGUILayout.Foldout(subFoldouts.GetValueOrDefault(p), label);
                if (subFoldouts[p])
                {
                    EditorGUI.indentLevel++;
                    foreach (var info in value.GetType().GetFields())
                        DataField(info.Name, info.GetValue(value), subFoldouts, p);
                    foreach (var info in value.GetType().GetProperties())
                        DataField(info.Name, info.GetValue(value), subFoldouts, p);
                    EditorGUI.indentLevel--;
                }
                break;
        }
    }
}