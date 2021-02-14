using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public bool LoadNextScene = false;

    public bool LoadWithSceneName;

    public string SceneName;

    void Start()
    {
        if (LoadNextScene)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (LoadWithSceneName)
        {
            SceneManager.LoadScene(SceneName);
        }
    }

}
