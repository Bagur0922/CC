using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReplayManager : SingletonBase<ReplayManager>
{
    public List<List<FrameData>> replayDatas = new List<List<FrameData>>();
    public UnityEvent<int> replayEventHandler = new UnityEvent<int>();
    public float frameInterval;

    protected override void Awake()
    {
        base.Awake(); // SingletonBase�� Awake ȣ��
        // �߰����� �ʱ�ȭ�� �ʿ��ϴٸ� ���⿡ �ۼ�
    }

    public void StartReplay(int index)
    {
        if (index >= 0 && index < replayDatas.Count && replayDatas[index] != null)
        {
            replayEventHandler.Invoke(index);
        }
        else
        {
            Debug.LogError("Invalid replay index or data is null!");
        }
    }
}