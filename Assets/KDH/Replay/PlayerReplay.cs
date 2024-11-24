using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReplay : MonoBehaviour
{
    [SerializeField] bool isRecording;
    public List<FrameData> replayData;

    float replayTimer;
    int replayIndex = -1; // ����� ���÷����� �ε���

    void Update()
    {
        if (!isRecording)
            return;

        if (replayTimer <= 0)
        {
            replayTimer = ReplayManager.Instance.frameInterval;
            replayData.Add(new FrameData(transform.position, transform.rotation));
        }
        else
            replayTimer -= Time.deltaTime;
    }

    public void StartRecording(int index)
    {
        isRecording = true;
        replayIndex = index;
        replayData = new List<FrameData>(); // ���ο� ������ �ʱ�ȭ
    }

    public void EndRecording()
    {
        isRecording = false;

        if (replayIndex >= 0)
        {
            // Ư�� �ε����� ������ ����
            if (ReplayManager.Instance.replayDatas.Count <= replayIndex)
            {
                // �ε����� ���� ����Ʈ ������ ����� Ȯ��
                while (ReplayManager.Instance.replayDatas.Count <= replayIndex)
                {
                    ReplayManager.Instance.replayDatas.Add(null);
                }
            }

            ReplayManager.Instance.replayDatas[replayIndex] = replayData;
        }
    }
}
