using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public bool LoadNextScene = false;

    public bool LoadWithSceneName;

    public bool LoadAfterSeconds = false;

    public float Seconds = 2.0f;

    public string SceneName;

    void Awake()
    {
        if (!LoadAfterSeconds)
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

    private IEnumerator Start()
    {
        if (LoadAfterSeconds)
        {
            yield return new WaitForSeconds(Seconds);

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
}
