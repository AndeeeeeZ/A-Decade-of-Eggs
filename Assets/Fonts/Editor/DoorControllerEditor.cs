using UnityEngine;
using UnityEditor;

// Generated with help of ChatGPT
// Lowkey have no idea how to do this

[CustomEditor(typeof(DoorController))]
public class DoorControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DoorController door = (DoorController)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Set Start Position"))
        {
            Undo.RecordObject(door, "Set Start Position");
            door.transform.position = door.transform.position; // ensures transform is recorded
            SerializedObject so = new SerializedObject(door);
            so.FindProperty("startLocation").vector3Value = door.transform.position;
            so.ApplyModifiedProperties();
        }

        if (GUILayout.Button("Set End Position"))
        {
            Undo.RecordObject(door, "Set End Position");
            SerializedObject so = new SerializedObject(door);
            so.FindProperty("endLocation").vector3Value = door.transform.position;
            so.ApplyModifiedProperties();
        }

        if (GUILayout.Button("Move To Start"))
        {
            Undo.RecordObject(door.transform, "Move To Start");
            door.transform.position = GetVector3(door, "startLocation");
        }
    }

    private Vector3 GetVector3(DoorController door, string fieldName)
    {
        SerializedObject so = new SerializedObject(door);
        return so.FindProperty(fieldName).vector3Value;
    }
}