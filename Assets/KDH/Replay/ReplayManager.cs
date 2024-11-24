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
        base.Awake(); // SingletonBase의 Awake 호출
        // 추가적인 초기화가 필요하다면 여기에 작성
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