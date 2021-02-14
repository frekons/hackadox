using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Functions : MonoBehaviour
{
    [HideInInspector]
    public GameManager GameManager;

    private void Awake()
    {
        Instance = this;
    }

    public void ResetGame()
    {
        GameManager.ResetGame();
    }

    //

    private static Functions _instance;

    public static Functions Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<Functions>();
            }

            return _instance;
        }

        set
        {
            _instance = value;
        }
    }
}
