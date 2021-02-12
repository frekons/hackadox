using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerController : MonoBehaviour
{
    private GameObject localPlayer;
    public Vector3 camOffset = Vector3.zero;
    public float followSpeed = 1f;

    void Start()
    {
        localPlayer = GameObject.FindWithTag("Player");    
    }

    void Update()
    { 
        Vector3 _camPos = localPlayer.transform.position;
        _camPos.z = transform.position.z;

        transform.position = Vector3.Lerp(transform.position, _camPos + camOffset,Time.deltaTime* followSpeed);
    }
}
