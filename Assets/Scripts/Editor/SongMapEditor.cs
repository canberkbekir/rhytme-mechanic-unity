using Beats.Base;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SongMap))]
public class SongMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SongMap songMap = (SongMap)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Calculated Song Length", $"{songMap.SongLength:F2} seconds", EditorStyles.boldLabel);
    }
}