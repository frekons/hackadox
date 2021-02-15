using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    private void Awake()
    {
        Instance = this;
    }

    //

    public static CoroutineManager Instance;
}
