using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerReplayRecorder))]
// ���ڴ��� ������ ��ȭ ���۰� ���Ḧ ���� ��ư ����
public class PlayerReplayRecorderEditor : Editor
{
    PlayerReplayRecorder playerReplay;

    void OnEnable()
    {
        playerReplay = (PlayerReplayRecorder)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("��ȭ ����"))
            playerReplay.StartRecording();
        if (GUILayout.Button("��ȭ ����"))
            playerReplay.EndRecording();
    }
}
