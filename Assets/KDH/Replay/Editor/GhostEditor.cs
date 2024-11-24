using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GhostEditor : UnityEditor.Editor
{
    Ghost ghost;
    int replayIndex = 0; // ������ ���÷��� �ε���

    void OnEnable()
    {
        ghost = (Ghost)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // ���÷��� �ε��� �Է� �ʵ�
        replayIndex = EditorGUILayout.IntField("���÷��� �ε���", replayIndex);

        // ���� üũ �� ���÷��� ���� ��ư
        if (GUILayout.Button("���÷��� ����"))
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
                    Debug.LogError($"���÷��� �����Ͱ� {replayIndex}�� �ε������� null�Դϴ�!");
                }
            }
            else
            {
                Debug.LogError("��ȿ���� ���� ���÷��� �ε����Դϴ�!");
            }
        }
    }
}
