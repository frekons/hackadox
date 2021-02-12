using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackerCharacter : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    private void Awake()
    {
        Instance = this;
    }


    public void SetIdle()
    {
        _animator.SetInteger("State", (int)HACKER_STATE.IDLE);
    }

    public void SetHacking()
    {
        _animator.SetInteger("State", (int)HACKER_STATE.HACKING);
    }


    enum HACKER_STATE
    {
        IDLE,
        HACKING
    }

    //

    public static HackerCharacter Instance;
}
