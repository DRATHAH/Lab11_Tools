using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Pathfinding))]
public class PathfindingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        Pathfinding pathfinding = (Pathfinding)target;

        // Add obstacle button
        SerializedProperty obstacleProp = serializedObject.FindProperty("newObstacle");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(obstacleProp);
        if (GUILayout.Button("Add", GUILayout.Width(50)))
        {
            pathfinding.AddObstacle();
        }
        EditorGUILayout.EndHorizontal();

        // Apply changes before generating the grid
        serializedObject.ApplyModifiedProperties();
        // Exclude drawing the newObstacle button
        DrawPropertiesExcluding(serializedObject, "newObstacle");

        GUILayout.Space(10);

        if (GUILayout.Button("Generate New Grid"))
        {
            pathfinding.RegenerateGrid();

            // Mark object as changed
            EditorUtility.SetDirty(pathfinding);
        }
    }
}
