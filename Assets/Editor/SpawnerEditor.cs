using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spawner))]
public class SpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var spawwner = (Spawner)target;

        EditorGUILayout.Space();
        if (GUILayout.Button("Force Respawn")) spawwner.ForceRespawn();
        if (GUILayout.Button("Try Respawn")) spawwner.TryRespawn();
        if (GUILayout.Button("Destroy Spawned")) DestroyImmediate(spawwner.transform.GetChild(0).gameObject);
    }
}
