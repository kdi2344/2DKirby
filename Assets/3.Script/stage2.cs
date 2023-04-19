using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stage2 : MonoBehaviour
{
    private GameObject stage2image;
    private GameObject stage2star;
    private GameObject stage2num;
    [SerializeField] private Sprite stage2clearimage;

    private void Awake()
    {
        stage2image = transform.GetChild(0).gameObject;
        stage2star = transform.GetChild(1).gameObject;
        stage2num = transform.GetChild(2).gameObject;
    }
    private void Start()
    {
        if (!GameManager._instance.stage1Clear)
        {
            stage2image.SetActive(false);
            stage2star.SetActive(false);
            stage2num.SetActive(false);
        }
        else
        {
            stage2image.SetActive(true);
            stage2star.SetActive(true);
            stage2num.SetActive(true);
        }
        if (GameManager._instance.stage2Clear)
        {
            stage2image.GetComponent<SpriteRenderer>().sprite = stage2clearimage;
        }
    }
    private void Update()
    {
        
    }
}
