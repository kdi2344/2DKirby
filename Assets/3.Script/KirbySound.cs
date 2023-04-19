using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KirbySound : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip audioJump;
    public AudioClip audioInhale;
    public AudioClip audioCopy;
    public AudioClip audioInhaling;
    public AudioClip audioBeam;
    public AudioClip audioCopyOff;
    public AudioClip audioDoor;
    public AudioClip audioHurt;
    public AudioClip audio;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void playSound(string name)
    {
        switch (name)
        {
            case "Jump":
                audioSource.clip = audioJump;
                audioSource.loop = false;
                break;
            case "Inhale":
                audioSource.clip = audioInhale;
                audioSource.loop = false;
                break;
            case "Inhaling":
                audioSource.clip = audioInhaling; 
                audioSource.loop = true;
                break;
            case "Copy":
                audioSource.clip = audioCopy;
                audioSource.loop = false;
                break;
            case "Beam":
                audioSource.clip = audioBeam;
                audioSource.loop = false;
                break;
            case "Off":
                audioSource.clip = audioCopyOff;
                audioSource.loop = false;
                break;
            case "Door":
                audioSource.clip = audioDoor;
                audioSource.loop = false;
                break;
            case "Hurt":
                audioSource.clip = audioHurt;
                audioSource.loop = false;
                break;
        }
        if (name == "Inhale")
        {
            audioSource.Play();
        }
        else
        {
                audioSource.PlayOneShot(audioSource.clip);
        }
    }
    public void stopSound()
    {
        audioSource.Stop();
        audioSource.loop = false;
    }
}
