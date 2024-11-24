using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerReplayEditor : UnityEditor.Editor
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
            playerReplay.StartRecording(GameManager.Instance.day);
        if (GUILayout.Button("≥Ï»≠ ¡æ∑·"))
            playerReplay.EndRecording();
    }
}
