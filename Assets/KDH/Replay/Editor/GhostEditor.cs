using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GhostEditor : UnityEditor.Editor
{
    Ghost ghost;
    int replayIndex = 0; // 선택할 리플레이 인덱스

    void OnEnable()
    {
        ghost = (Ghost)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // 리플레이 인덱스 입력 필드
        replayIndex = EditorGUILayout.IntField("리플레이 인덱스", replayIndex);

        // 범위 체크 및 리플레이 시작 버튼
        if (GUILayout.Button("리플레이 시작"))
        {
            if (ReplayManager.Instance.replayDatas.Count > replayIndex && replayIndex >= 0)
            {
                var replayData = ReplayManager.Instance.replayDatas[replayIndex];
                if (replayData != null)
                {
                    ghost.Initialize(replayData);
                    ghost.StartReplay();
                }
                else
                {
                    Debug.LogError($"리플레이 데이터가 {replayIndex}번 인덱스에서 null입니다!");
                }
            }
            else
            {
                Debug.LogError("유효하지 않은 리플레이 인덱스입니다!");
            }
        }
    }
}
