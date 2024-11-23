using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReplayManager : SingleTon<ReplayManager>
{
    public List<List<FrameData>> replayDatas = new();
    public UnityEvent replayEventHandler = new();
    public float frameInterval;

    public void StartReplay()
    {
        replayEventHandler.Invoke();
    }
}
