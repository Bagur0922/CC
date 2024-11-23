using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReplayRecorder : MonoBehaviour
{
    [SerializeField] bool isRecording;
    [SerializeField] List<FrameData> replayData;
    
    float replayTimer;

    void Update()
    {
        if (!isRecording)
            return;

        if (replayTimer <= 0)
        {
            replayTimer = ReplayManager.Instance.frameInterval;
            replayData.Add(new(transform.position, transform.rotation));
        }
        else
            replayTimer -= Time.deltaTime;
    }

    public void StartRecording()
    {
        isRecording = true;
        replayData = new();
    }

    // ���ڵ� ����(��� ����)�� ��Ʈ ���� �� �ʱ�ȭ
    public void EndRecording()
    {
        isRecording = false;
        gameObject.AddComponent<Ghost>();
        Ghost ghost = GetComponent<Ghost>();
        ghost.Initialize(replayData);
    }
}
