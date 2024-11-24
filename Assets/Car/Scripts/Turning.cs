using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turning : MonoBehaviour
{
    [SerializeField] float turnspeed;

    private void FixedUpdate()
    {
        transform.Rotate(0, turnspeed, 0);
    }
}
