using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterTrack : MonoBehaviour
{
    public void Pressed()
    {
        SceneManager.LoadScene("Track");
    }
}
