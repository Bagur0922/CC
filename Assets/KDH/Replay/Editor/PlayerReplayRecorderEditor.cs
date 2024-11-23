using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerReplayRecorder))]
// 레코더의 에디터 녹화 시작과 종료를 위한 버튼 생성
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

        if (GUILayout.Button("녹화 시작"))
            playerReplay.StartRecording();
        if (GUILayout.Button("녹화 종료"))
            playerReplay.EndRecording();
    }
}
