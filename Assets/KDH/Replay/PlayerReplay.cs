using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReplay : MonoBehaviour
{
    [SerializeField] bool isRecording;
    public List<FrameData> replayData;

    float replayTimer;
    int replayIndex = -1; // 저장될 리플레이의 인덱스

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
        replayData = new List<FrameData>(); // 새로운 데이터 초기화
    }

    public void EndRecording()
    {
        isRecording = false;

        if (replayIndex >= 0)
        {
            // 특정 인덱스에 데이터 저장
            if (ReplayManager.Instance.replayDatas.Count <= replayIndex)
            {
                // 인덱스가 현재 리스트 범위를 벗어나면 확장
                while (ReplayManager.Instance.replayDatas.Count <= replayIndex)
                {
                    ReplayManager.Instance.replayDatas.Add(null);
                }
            }

            ReplayManager.Instance.replayDatas[replayIndex] = replayData;
        }
    }
}
