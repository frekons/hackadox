using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public bool DestroyOnSceneLoad = false;

    public bool UseSeconds = true;

    public float DestroyAfterSeconds = 3;

    void Start()
    {
        if (UseSeconds)
        {
            Destroy(gameObject, DestroyAfterSeconds);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        if (DestroyOnSceneLoad)
        {
            Destroy(gameObject);
        }
    }
}
