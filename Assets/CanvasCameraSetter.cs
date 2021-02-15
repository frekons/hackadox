using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCameraSetter : MonoBehaviour
{
    private Canvas _canvas;

    private void Update()
    {
        if (_canvas == null)
        {
            _canvas = GetComponent<Canvas>();
            return;
        }

        _canvas.worldCamera = Camera.main;

        _canvas.planeDistance = 1;
    }
}
