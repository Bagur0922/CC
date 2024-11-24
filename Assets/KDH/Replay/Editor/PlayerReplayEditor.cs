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

        if (GUILayout.Button("��ȭ ����"))
            playerReplay.StartRecording(GameManager.Instance.day);
        if (GUILayout.Button("��ȭ ����"))
            playerReplay.EndRecording();
    }
}
