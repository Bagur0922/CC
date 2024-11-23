using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    List<FrameData> replayData = new ();
    bool isReplaying;

    int currentFrame;

    float lerpTime;

    void Start()
    {
        ReplayManager.Instance.replayEventHandler.AddListener(StartReplay);
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
        this.replayData = replayData;
    }

    public void StartReplay()
    {
        currentFrame = 1;
        isReplaying = true;
    }

    public void StopReplay()
    {
        isReplaying = false;
    }
}
