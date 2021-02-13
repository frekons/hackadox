using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }

    //

    public static void PlayOneShot(AudioClip clip)
    {
        Instance.audioSource.PlayOneShot(clip);
    }

    //

    public static UISoundManager Instance;
}
