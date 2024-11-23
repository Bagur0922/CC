using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    List<FrameData> replayData = new ();
    bool isReplaying;

    int currentFrame;

    float lerpTime; // ��ȯ�� ���� �ð�

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

            float time = lerpTime / ReplayManager.Instance.frameInterval; // ���� �ð� / �����Ӱ� �������� 0~1�� ���� ������ ���� �����Ӱ� ���� �������� ������ ���� ��½�Ŵ  

            transform.position = Vector3.Lerp(replayData[currentFrame].transform, replayData[currentFrame + 1].transform, time);
            transform.rotation = Quaternion.Slerp(replayData[currentFrame].rotation, replayData[currentFrame + 1].rotation, time);

            if (lerpTime >= ReplayManager.Instance.frameInterval) // ������ ���� ��ŭ �������� ���� ����������
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
