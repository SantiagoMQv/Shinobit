using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Waypoint))]
public class WaypointsEditor : Editor
{
    Waypoint WaypointTarget => target as Waypoint;

    private void OnSceneGUI()
    {
        Handles.color = Color.red;
        if(WaypointTarget.Points != null)
        {
            for (int i = 0; i < WaypointTarget.Points.Length; i++)
            {
                // Crea el Handle
                EditorGUI.BeginChangeCheck();
                Vector3 CurrentPoint = WaypointTarget.CurrentPosition + WaypointTarget.Points[i];
                Vector3 newPoint = Handles.FreeMoveHandle(CurrentPoint, Quaternion.identity, 0.5f, new Vector3(0.3f, 0.3f, 0.3f), Handles.SphereHandleCap);

                // Crea el texto
                GUIStyle text = new GUIStyle();
                text.fontStyle = FontStyle.Bold;
                text.fontSize = 16;
                text.normal.textColor = Color.black;
                Vector3 alignment = Vector3.down * 0.3f + Vector3.right * 0.3f;
                Handles.Label(WaypointTarget.CurrentPosition + WaypointTarget.Points[i] + alignment, $"{i + 1}", text);

                if (EditorGUI.EndChangeCheck()) // Si se han realizado cambios en el editor
                {
                    Undo.RecordObject(target, "Free Move Handle");
                    WaypointTarget.Points[i] = newPoint - WaypointTarget.CurrentPosition;
                }
            }
        }
    }

}
