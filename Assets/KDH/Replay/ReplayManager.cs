using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReplayManager : SingleTon<ReplayManager>
{
    public int ghostCount;
    public UnityEvent replayEventHandler = new();
    public float frameInterval;

    public void StartReplay()
    {
        replayEventHandler.Invoke();
    }
}
