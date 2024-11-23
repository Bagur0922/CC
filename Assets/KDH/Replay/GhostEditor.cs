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

        if (GUILayout.Button("���÷��� ����"))
            ghost.StartReplay();
    }
}
