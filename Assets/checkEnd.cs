using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class checkEnd : MonoBehaviour
{
    public VideoPlayer vid;

    void Start() { vid.loopPointReached += CheckOver; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SceneManager.LoadScene("StageSelect");
        }
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene("StageSelect");
    }

}

