using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReplay : MonoBehaviour
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

    public void EndRecording()
    {
        isRecording = false;
        ReplayManager.Instance.replayDatas.Add(replayData);
    }
}
