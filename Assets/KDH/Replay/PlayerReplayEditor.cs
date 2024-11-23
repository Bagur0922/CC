using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerReplay))]
public class PlayerReplayEditor : Editor
{
    PlayerReplay playerReplay;

    void OnEnable()
    {
        playerReplay = (PlayerReplay)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("≥Ï»≠ Ω√¿€"))
            playerReplay.StartRecording();
        if (GUILayout.Button("≥Ï»≠ ¡æ∑·"))
            playerReplay.EndRecording();
    }
}
