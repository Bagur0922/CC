using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FrameData
{
    public Vector3 transform;
    public Quaternion rotation;

    public FrameData(Vector3 transform, Quaternion rotation)
    {
        this.transform = transform;
        this.rotation = rotation;
    }
}
