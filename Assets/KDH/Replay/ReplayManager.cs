using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReplayManager : MonoBehaviour
{
    static public ReplayManager Instance;
    public List<List<FrameData>> replayDatas = new();
    public UnityEvent replayEventHandler = new();
    public float frameInterval;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void StartReplay()
    {
        replayEventHandler.Invoke();
    }
}
