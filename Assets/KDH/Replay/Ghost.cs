using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    List<FrameData> replayData = new();
    bool isReplaying;

    int currentFrame;
    float lerpTime;

    void Start()
    {
        DontDestroyOnLoad(this);
        ReplayManager.Instance.replayEventHandler.AddListener(InitializeAndStartReplay);
    }

    void Update()
    {
        if (!isReplaying)
            return;

        if (replayData.Count > currentFrame + 1)
        {
            lerpTime += Time.deltaTime;

            float time = lerpTime / ReplayManager.Instance.frameInterval;

            transform.position = Vector3.Lerp(replayData[currentFrame].transform, replayData[currentFrame + 1].transform, time);
            transform.rotation = Quaternion.Slerp(replayData[currentFrame].rotation, replayData[currentFrame + 1].rotation, time);

            if (lerpTime >= ReplayManager.Instance.frameInterval)
            {
                currentFrame++;
                lerpTime = 0;

                if (currentFrame >= replayData.Count)
                {
                    StopReplay();
                }
            }
        }
    }

    public void Initialize(List<FrameData> replayData)
    {
        this.replayData = replayData; // 전달받은 리플레이 데이터를 설정
    }

    public void StartReplay()
    {
        currentFrame = 0;
        isReplaying = true;
        lerpTime = 0;
    }

    public void StopReplay()
    {
        isReplaying = false;
    }

    private void InitializeAndStartReplay(int index)
    {
        if (index >= 0 && index < ReplayManager.Instance.replayDatas.Count)
        {
            replayData = ReplayManager.Instance.replayDatas[index];
            StartReplay();
        }
        else
        {
            Debug.LogError("Invalid replay index or data is null!");
        }
    }
}
