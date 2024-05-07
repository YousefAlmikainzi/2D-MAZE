using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script manages audio
public class Audio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip; 

    private void Start()
    {
        PlaySound(); 
    }

    public void PlaySound()
    {
        audioSource.clip = audioClip; 
        audioSource.Play();
    }
}
