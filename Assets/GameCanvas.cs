using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    public GameObject HackerPanel;

    private void Awake()
    {
        Instance = this;
    }

    //

    public static GameCanvas Instance;
}
