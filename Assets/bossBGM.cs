using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossBGM : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip audioBoss;

    public void playBoss()
    {
        Camera.main.GetComponent<AudioSource>().clip = audioBoss;
        Camera.main.GetComponent<AudioSource>().PlayOneShot(Camera.main.GetComponent<AudioSource>().clip);
    }
}
