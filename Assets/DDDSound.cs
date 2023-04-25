using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDDSound : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip audioAttack;
    public AudioClip audioDamage;
    public AudioClip audioKill;
    private void Awake()
    {
        TryGetComponent(out audioSource);
    }
    public void playSound(string name)
    {
        switch (name)
        {
            case "Attack":
                audioSource.clip = audioAttack;
                break;
            case "Damage":
                audioSource.clip = audioDamage;
                break;
            case "Kill":
                audioSource.clip = audioKill;
                break;
        }
        audioSource.PlayOneShot(audioSource.clip);
    }
}
