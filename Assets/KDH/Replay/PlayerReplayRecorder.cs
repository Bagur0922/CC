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

    // 레코딩 종료(경기 종료)시 고스트 생성 및 초기화
    public void EndRecording()
    {
        isRecording = false;
        gameObject.AddComponent<Ghost>();
        Ghost ghost = GetComponent<Ghost>();
        ghost.Initialize(replayData);
    }
}
