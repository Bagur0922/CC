using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Ghost))]
public class GhostEditor : Editor
{
    Ghost ghost;

    void OnEnable()
    {
        ghost = (Ghost)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("리플레이 시작"))
            ghost.StartReplay();
    }
}
