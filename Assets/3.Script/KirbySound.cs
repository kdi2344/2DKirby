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
    public AudioClip audioHub;
    public AudioClip audioSplitStar;
    public AudioClip audioLifeup;
    public AudioClip audioDamage;
    public AudioClip audioHeal;
    public AudioClip audioFly;
    public AudioClip audioFlySplit;
    public AudioClip audioKill;
    public AudioClip audioCut;
    public AudioClip audioDie;
    public AudioClip audioRun;
    public AudioClip audioPlazma;
    public AudioClip audioMario;
    public AudioClip audioWheel;

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
            case "Hub":
                audioSource.clip = audioHub;
                audioSource.loop = false;
                break;
            case "Split":
                audioSource.clip = audioSplitStar;
                audioSource.loop = false;
                break;
            case "Lifeup":
                audioSource.clip = audioLifeup;
                audioSource.loop = false;
                break;
            case "Damage":
                audioSource.clip = audioDamage;
                audioSource.loop = false;
                break;
            case "Heal":
                audioSource.clip = audioHeal;
                audioSource.loop = false;
                break;
            case "Fly":
                audioSource.clip = audioFly;
                audioSource.loop = false;
                break;
            case "FlySplit":
                audioSource.clip = audioFlySplit;
                audioSource.loop = false;
                break;
            case "Kill":
                audioSource.clip = audioKill;
                audioSource.loop = false;
                break;
            case "Cut":
                audioSource.clip = audioCut;
                audioSource.loop = false;
                break;
            case "Die":
                audioSource.clip = audioDie;
                audioSource.loop = false;
                break;
            case "Run":
                audioSource.clip = audioRun;
                audioSource.loop = false;
                break;
            case "Plazma":
                audioSource.clip = audioPlazma;
                audioSource.loop = false;
                break;
            case "Mario":
                audioSource.clip = audioMario;
                audioSource.loop = false;
                break;
            case "Wheel":
                audioSource.clip = audioWheel;
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
