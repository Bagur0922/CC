using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSH
{
        
    public class SceneManager : SingleTon<SceneManager>
    {
        public void ChangeScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        public void EndGame()
        {
            Application.Quit();
        }
    }
}
