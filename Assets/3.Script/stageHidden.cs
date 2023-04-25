using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stageHidden : MonoBehaviour
{
    private GameObject stage4image;
    private GameObject stage4star;
    private GameObject stage4num;

    private void Awake()
    {
        stage4image = transform.GetChild(0).gameObject;
        stage4star = transform.GetChild(1).gameObject;
        stage4num = transform.GetChild(2).gameObject;
    }
    private void Start()
    {
        if (!GameManager._instance.stage3Clear)
        {
            stage4image.SetActive(false);
            stage4star.SetActive(false);
            stage4num.SetActive(false);
        }
        else
        {
            stage4image.SetActive(true);
            stage4star.SetActive(true);
            stage4num.SetActive(true);
        }
    }
    private void Update()
    {

    }
}
