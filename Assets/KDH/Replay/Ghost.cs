using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    List<FrameData> replayData = new ();
    bool isReplaying;

    int currentFrame;

    float lerpTime; // 전환중 지난 시간

    int ghostNumber;
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

            float time = lerpTime / ReplayManager.Instance.frameInterval; // 지난 시간 / 프레임간 간격으로 0~1의 값을 가지며 현재 프레임과 다음 프레임의 사이의 값을 출력시킴  

            transform.position = Vector3.Lerp(replayData[currentFrame].transform, replayData[currentFrame + 1].transform, time);
            transform.rotation = Quaternion.Slerp(replayData[currentFrame].rotation, replayData[currentFrame + 1].rotation, time);

            if (lerpTime >= ReplayManager.Instance.frameInterval) // 프레임 간격 만큼 지났으면 다음 프레임으로
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
        ghostNumber = ReplayManager.Instance.ghostCount;
        ReplayManager.Instance.ghostCount++;
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
