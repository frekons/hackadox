using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCameraSetter : MonoBehaviour
{
    private void OnLevelWasLoaded(int level)
    {
        var canvas = GetComponent<Canvas>();

        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            canvas.worldCamera = Camera.main;
        }
    }
}
