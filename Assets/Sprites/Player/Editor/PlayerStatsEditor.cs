using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

 [CustomEditor(typeof(PlayerStats))]
public class PlayerStatsEditor : Editor
{
    public PlayerStats StatsTarget => target as PlayerStats;

    //Para añadir un boton en el inspector
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Reset values"))
        {
            StatsTarget.ResetValues();
        }
    }

}
