using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
  
    public AudioSource musicAudioSource;
    public AudioSource vfxAudioSource;


    public AudioClip musicClip;
    public AudioClip doorClip;
    public AudioClip winClip;
    public AudioClip deadClip;

    void Start()
    {
        musicAudioSource.clip = musicClip;
        musicAudioSource.Play();
    }
 
}
